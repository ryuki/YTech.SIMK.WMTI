using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Reporting.WebForms;
using SharpArch.Core;
using SharpArch.Data.NHibernate;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Data.Repository;
using YTech.SIMK.WMTI.Enums;
using YTech.SIMK.WMTI.Web.Controllers.ViewModel;

namespace YTech.SIMK.WMTI.Web.Controllers.Transaction
{
    [HandleError]
    public class ClosingController : Controller
    {
        private readonly ITLoanRepository _loanRepository;
        private readonly ITInstallmentRepository _installmentRepository;
        private readonly IMEmployeeRepository _mEmployeeRepository;
        private readonly ITRecPeriodRepository _tRecPeriodRepository;
        private readonly IMCommissionRepository _mCommissionRepository;
        private readonly IMCommissionDetRepository _mCommissionDetRepository;
        private readonly ITLoanSurveyRepository _tLoanSurveyRepository;
        private readonly ITCommissionRepository _tCommissionRepository;
        private readonly IMZoneEmployeeRepository _mZoneEmployeeRepository;

        public ClosingController(ITLoanRepository loanRepository, ITInstallmentRepository installmentRepository, IMEmployeeRepository mEmployeeRepository, ITRecPeriodRepository tRecPeriodRepository, IMCommissionRepository mCommissionRepository, IMCommissionDetRepository mCommissionDetRepository, ITLoanSurveyRepository tLoanSurveyRepository, ITCommissionRepository tCommissionRepository, IMZoneEmployeeRepository mZoneEmployeeRepository)
        {
            Check.Require(loanRepository != null, "loanRepository may not be null");
            Check.Require(installmentRepository != null, "installmentRepository may not be null");
            Check.Require(mEmployeeRepository != null, "mEmployeeRepository may not be null");
            Check.Require(tRecPeriodRepository != null, "tRecPeriodRepository may not be null");
            Check.Require(mCommissionRepository != null, "mCommissionRepository may not be null");
            Check.Require(mCommissionDetRepository != null, "mCommissionDetRepository may not be null");
            Check.Require(tLoanSurveyRepository != null, "tLoanSurveyRepository may not be null");
            Check.Require(tCommissionRepository != null, "tCommissionRepository may not be null");
            Check.Require(mZoneEmployeeRepository != null, "mZoneEmployeeRepository may not be null");

            this._loanRepository = loanRepository;
            this._installmentRepository = installmentRepository;
            this._mEmployeeRepository = mEmployeeRepository;
            this._tRecPeriodRepository = tRecPeriodRepository;
            this._mCommissionRepository = mCommissionRepository;
            this._mCommissionDetRepository = mCommissionDetRepository;
            this._tLoanSurveyRepository = tLoanSurveyRepository;
            this._tCommissionRepository = tCommissionRepository;
            this._mZoneEmployeeRepository = mZoneEmployeeRepository;
        }

        public ActionResult Index()
        {
            ClosingViewModel viewModel = ClosingViewModel.Create();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(ClosingViewModel viewModel, FormCollection formCollection)
        {
            string Message = string.Empty;
            bool Success = true;

            try
            {
                _tRecPeriodRepository.DbContext.BeginTransaction();

                TRecPeriod recPeriod = new TRecPeriod();
                recPeriod.SetAssignedIdTo(Guid.NewGuid().ToString());
                recPeriod.PeriodFrom = Helper.CommonHelper.ConvertToDate(formCollection["StartDate"]);
                recPeriod.PeriodTo = Helper.CommonHelper.ConvertToDate(formCollection["EndDate"]);
                recPeriod.PeriodType = EnumPeriodType.Custom.ToString();
                recPeriod.PeriodDesc = string.Format("{0} s/d {1}", formCollection["StartDate"], formCollection["EndDate"]);
                recPeriod.CreatedBy = User.Identity.Name;
                recPeriod.CreatedDate = DateTime.Now;

                CalculateCommission(recPeriod);

                _tRecPeriodRepository.Save(recPeriod);
                _tRecPeriodRepository.DbContext.CommitChanges();
                Success = true;
                Message = "Tutup buku Berhasil Disimpan.";
            }
            catch (Exception ex)
            {
                Success = false;
                Message = "Error :\n" + ex.GetBaseException().Message;
                _tRecPeriodRepository.DbContext.RollbackTransaction();
            }
            var e = new
            {
                Success,
                Message
            };
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        private void CalculateCommission(TRecPeriod recPeriod)
        {
            IList<TLoan> listLoan = _loanRepository.GetListByAccDate(recPeriod.PeriodFrom, recPeriod.PeriodTo);
            CalculateCommissionSurveyor(recPeriod, listLoan);
            CalculateCommissionTLS(recPeriod, listLoan);
            CalculateCommissionSales(recPeriod, listLoan);
            CalculateCommissionCollector(recPeriod, listLoan);
        }

        private void CalculateCommissionCollector(TRecPeriod recPeriod, IList<TLoan> listLoan)
        {
            MCommission comm = _mCommissionRepository.GetCommissionByDate(EnumDepartment.COL, recPeriod.PeriodFrom, recPeriod.PeriodTo);
            if (comm == null)
                throw new Exception("Data komisi untuk kolektor tidak tersedia.");

            IList<MCommissionDet> listDets = comm.CommissionDets;

            IList<MZoneEmployee> zoneEmployees = _mZoneEmployeeRepository.GetListByDate(recPeriod.PeriodFrom, recPeriod.PeriodTo);
            //get installment group by tls that status is OK
            IList<TInstallment> listInstallment = _installmentRepository.GetListByMaturityDate(recPeriod.PeriodFrom, recPeriod.PeriodTo);

            //save collector commission
            MCommissionDet det;
            foreach (MZoneEmployee ze in zoneEmployees)
            {
                //get target and real
                decimal? target = (from ins in listInstallment
                                   where ins.LoanId.ZoneId == ze.ZoneId
                                   select ins.InstallmentBasic).Sum();
                decimal? real = (from ins in listInstallment
                                 where ins.LoanId.ZoneId == ze.ZoneId && ins.InstallmentStatus == EnumInstallmentStatus.Paid.ToString() && ins.InstallmentPaymentDate <= ins.InstallmentMaturityDate
                                 select ins.InstallmentBasic).Sum();

                //if (real.HasValue)
                {
                    decimal percentReal = Math.Ceiling(target.HasValue && real.HasValue ? real.Value / target.Value * 100 : 0);

                    //get incentive
                    det = (from d in listDets
                           where d.DetailLowTarget <= percentReal && d.DetailHighTarget >= percentReal
                           select d).FirstOrDefault();

                    //check if it has commission to calculate
                    if (det != null)
                    {
                        //normal commission
                        SaveCommission(recPeriod, EnumCommissionType.Commission, EnumDepartment.COL, ze.EmployeeId, 1, real, det.DetailValue.Value / 100);
                        //transport commission
                        SaveCommission(recPeriod, EnumCommissionType.TransportAllowance, EnumDepartment.COL, ze.EmployeeId, 2, 30, det.DetailTransportAllowance);
                    }
                }

            }

            //var listIns = from ins in listInstallment
            //              select new
            //              {
            //                  EmployeeId = ins.EmployeeId,
            //                  ZoneId = ins.LoanId.ZoneId,
            //                  InstallmentBasic = ins.InstallmentBasic
            //              };

            //var listInsZe = from ins in listIns
            //                join ze in zoneEmployees on new { ins.EmployeeId, ins.ZoneId } equals new { ze.EmployeeId, ze.ZoneId } into joinInsZe
            //                from ze in joinInsZe.DefaultIfEmpty()
            //                select new
            //                {
            //                    EmployeeId = ins.EmployeeId ?? ze.EmployeeId,
            //                    ZoneId = ins.ZoneId,
            //                    InstallmentBasic = ins != null ? ins.InstallmentBasic : null
            //                };

            //var recapIns = from ins in listIns
            //               group ins by ins.EmployeeId into grouped
            //               select new
            //               {
            //                   EmployeeId = grouped.Key,
            //                   SumInstallment = grouped != null ? grouped.Sum(x => x.InstallmentBasic) : 0
            //               };

            //var recapPaidIns = from ins in listInstallment
            //                   where ins.InstallmentStatus == EnumInstallmentStatus.Paid.ToString() && ins.InstallmentPaymentDate <= ins.InstallmentMaturityDate
            //                   select ins;

            //var recapJoined = from ins in recapIns
            //                  join paid in recapPaidIns on ins.EmployeeId equals paid.EmployeeId into joinIns
            //                  from paid in joinIns.DefaultIfEmpty()
            //                  select new
            //                    {
            //                        ins.EmployeeId,
            //                        ins.SumInstallment,
            //                        paid.SumPaidInstallment
            //                    };

            ////save collector commission
            //MCommissionDet det;
            //foreach (var recap in recapJoined)
            //{
            //    decimal target = recap.SumInstallment.Value;
            //    decimal real = recap.SumPaidInstallment.Value;

            //    decimal percentReal = Math.Ceiling(target != 0 ? real / target * 100 : 0);

            //    //get incentive
            //    det = (from d in listDets
            //           where d.DetailLowTarget <= percentReal && d.DetailHighTarget >= percentReal
            //           select d).FirstOrDefault();

            //    //check if it has commission to calculate
            //    if (det != null)
            //    {
            //        //normal commission
            //        SaveCommission(recPeriod, EnumCommissionType.Commission, EnumDepartment.COL, recap.EmployeeId, 1, real, det.DetailValue.Value / 100);
            //        //transport commission
            //        SaveCommission(recPeriod, EnumCommissionType.TransportAllowance, EnumDepartment.COL, recap.EmployeeId, 2, 30, det.DetailTransportAllowance);
            //    }
            //}
        }

        private void CalculateCommissionSales(TRecPeriod recPeriod, IList<TLoan> listLoan)
        {
            MCommission comm = _mCommissionRepository.GetCommissionByDate(EnumDepartment.SA, recPeriod.PeriodFrom, recPeriod.PeriodTo);
            if (comm == null)
                throw new Exception("Data komisi untuk SA tidak tersedia.");

            IList<MCommissionDet> listDets = (from det in comm.CommissionDets
                                              orderby det.DetailCustomerNumber descending
                                              select det).ToList();

            //get loan group by SA that status is OK
            var recapLoan = from loan in listLoan
                            where loan.LoanStatus == EnumLoanStatus.OK.ToString() && loan.SalesmanId != null
                            group loan by loan.SalesmanId into grouped
                            select new
                            {
                                SalesmanId = grouped.Key,
                                SumLoan = grouped.Sum(x => x.LoanBasicPrice),
                                CountLoan = grouped.Count(x => x.SalesmanId != null)
                            };


            IList<TLoan> listOrderedLoan = (from loan in listLoan
                                            where loan.LoanStatus == EnumLoanStatus.OK.ToString() && loan.SalesmanId != null
                                            orderby loan.LoanAccDate
                                            select loan).ToList();

            TLoan loanToCalc;
            int startLoanNo = 0;
            int endLoanNo = 0;
            decimal commissionRun = 0;
            int sisaLoan = 0;
            decimal? incentive = 0;
            decimal? transport = 0;
            bool hasInsertAddedIncentive = false;
            //get commission value
            foreach (var recap in recapLoan)
            {
                //get loan by salesman
                listOrderedLoan = (from loan in listLoan
                                   where loan.LoanStatus == EnumLoanStatus.OK.ToString() && loan.SalesmanId != null && loan.SalesmanId == recap.SalesmanId
                                   orderby loan.LoanAccDate
                                   select loan).ToList();

                sisaLoan = recap.CountLoan;
                startLoanNo = 0;
                endLoanNo = 0;
                incentive = 0;
                transport = 0;
                hasInsertAddedIncentive = false;
                //loop commission detail
                foreach (MCommissionDet det in listDets)
                {
                    //search commission
                    commissionRun = det.DetailCustomerNumber != 0 ? sisaLoan / det.DetailCustomerNumber : 0;
                    if (commissionRun >= 1)
                    {
                        //get number of last loan to calculate with current commission
                        endLoanNo = endLoanNo + Convert.ToInt32(Math.Ceiling(commissionRun) * det.DetailCustomerNumber);
                        //get sisa loan
                        sisaLoan = sisaLoan - endLoanNo;
                        //loop loan
                        for (int j = startLoanNo; j < endLoanNo; j++)
                        {
                            //save commission
                            loanToCalc = listOrderedLoan[j];
                            SaveCommission(recPeriod, EnumCommissionType.Commission, EnumDepartment.SA, recap.SalesmanId, j, loanToCalc.LoanBasicPrice, det.DetailValue / 100);
                        }
                        startLoanNo = endLoanNo;

                        //insert other incentive
                        if (!hasInsertAddedIncentive)
                        {
                            SaveCommission(recPeriod, EnumCommissionType.TransportAllowance, EnumDepartment.SA, recap.SalesmanId, endLoanNo, 30, det.DetailTransportAllowance);
                            SaveCommission(recPeriod, EnumCommissionType.IncentiveCredit, EnumDepartment.SA, recap.SalesmanId, endLoanNo + 1, 1, det.DetailIncentive);
                            hasInsertAddedIncentive = true;
                        }
                    }

                }
            }
        }

        private void CalculateCommissionTLS(TRecPeriod recPeriod, IList<TLoan> listLoan)
        {
            MCommission comm = _mCommissionRepository.GetCommissionByDate(EnumDepartment.TLS, recPeriod.PeriodFrom, recPeriod.PeriodTo);
            if (comm == null)
                throw new Exception("Data komisi untuk TLS tidak tersedia.");

            //target mapped to commissionvalue
            //next to do, changes it to target
            decimal target = comm.CommissionValue.Value;

            IList<MCommissionDet> listDets = comm.CommissionDets;

            //get loan group by tls that status is OK
            var recapLoan = from loan in listLoan
                            where loan.LoanStatus == EnumLoanStatus.OK.ToString() && loan.TLSId != null
                            group loan by loan.TLSId into grouped
                            select new
                            {
                                TLSId = grouped.Key,
                                SumLoan = grouped.Sum(x => x.LoanBasicPrice)
                            };
            //save TLS commission
            MCommissionDet det;
            foreach (var recap in recapLoan)
            {
                decimal percentReal = Math.Ceiling(target != 0 ? recap.SumLoan.Value / target * 100 : 0);
                decimal percentCalc = percentReal > 100 ? 100 : percentReal;
                //get total to calculate,if realization more than 100, use target value
                //if not use sumloan
                decimal totalToCalc = percentReal > 100 ? target : recap.SumLoan.Value;

                //get incentive
                det = (from d in listDets
                       where d.DetailLowTarget <= percentCalc && d.DetailHighTarget >= percentCalc
                       select d).FirstOrDefault();

                //check if it has commission to calculate
                if (det != null)
                {
                    //normal commission
                    SaveCommission(recPeriod, EnumCommissionType.Commission, EnumDepartment.TLS, recap.TLSId, 1, totalToCalc, det.DetailValue.Value / 100);

                    //if real > 100, sisa calculate new commission
                    if (percentReal > 100)
                    {
                        det = (from d in listDets
                               where d.DetailLowTarget > 100
                               select d).FirstOrDefault();
                        //check if it has commission to calculate
                        if (det != null)
                        {
                            //above normal commission
                            SaveCommission(recPeriod, EnumCommissionType.Commission, EnumDepartment.TLS, recap.TLSId, 2, recap.SumLoan.Value - target, det.DetailValue.Value / 100);
                        }
                    }
                }


            }
        }

        private void CalculateCommissionSurveyor(TRecPeriod recPeriod, IList<TLoan> listLoan)
        {
            MCommission comm = _mCommissionRepository.GetCommissionByDate(EnumDepartment.SU, recPeriod.PeriodFrom, recPeriod.PeriodTo);
            if (comm == null)
                throw new Exception("Data komisi untuk surveyor tidak tersedia.");

            IList<MCommissionDet> listDets = comm.CommissionDets;
            decimal? incentiveSurvey = 0;
            decimal? incentiveApprove = 0;

            //get incentive
            if (listDets.Count > 0)
            {
                MCommissionDet det = listDets[0];
                incentiveSurvey = det.DetailIncentiveSurveyOnly;
                incentiveApprove = det.DetailIncentiveSurveyAcc;
            }

            //get survey group by surveyor
            IList<TLoanSurvey> listSurvey = _tLoanSurveyRepository.GetListBySurveyDate(recPeriod.PeriodFrom, recPeriod.PeriodTo);
            var recapSurvey = from survey in listSurvey
                              where survey.LoanId.SurveyorId != null
                              group survey by survey.LoanId.SurveyorId into grouped
                              select new
                              {
                                  SurveyorId = grouped.Key,
                                  CountSurvey = grouped.Count(x => x.LoanId.SurveyorId != null)
                              };
            //save survey commission
            foreach (var recap in recapSurvey)
            {
                SaveCommission(recPeriod, EnumCommissionType.IncentiveSurvey, EnumDepartment.SU, recap.SurveyorId, 1, recap.CountSurvey, incentiveSurvey);
            }

            //get loan group by surveyor that status is OK
            var recapLoan = from loan in listLoan
                            where loan.LoanStatus == EnumLoanStatus.OK.ToString() && loan.SurveyorId != null
                            group loan by loan.SurveyorId into grouped
                            select new
                            {
                                SurveyorId = grouped.Key,
                                CountLoan = grouped.Count(x => x.SurveyorId != null)
                            };
            //save loan commission
            foreach (var recap in recapLoan)
            {
                SaveCommission(recPeriod, EnumCommissionType.IncentiveApprove, EnumDepartment.SU, recap.SurveyorId, 1, recap.CountLoan, incentiveApprove);
            }
        }

        private void SaveCommission(TRecPeriod recPeriod, EnumCommissionType enumCommissionType, EnumDepartment department, MEmployee mEmployee, int level, decimal? commissionFactor, decimal? commissionValue)
        {
            TCommission comm = new TCommission();
            comm.SetAssignedIdTo(Guid.NewGuid().ToString());
            comm.CommissionLevel = level;
            comm.CommissionFactor = commissionFactor;
            comm.CommissionValue = commissionValue;
            comm.EmployeeId = mEmployee;
            comm.CommissionType = enumCommissionType.ToString();
            comm.CommissionStatus = department.ToString();
            comm.RecPeriodId = recPeriod;

            comm.CreatedBy = User.Identity.Name;
            comm.CreatedDate = DateTime.Now;
            comm.DataStatus = EnumDataStatus.Updated.ToString();
            _tCommissionRepository.Save(comm);
        }
    }
}
