using System;
using System.Linq;
using System.Web.Mvc;
using SharpArch.Core;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Data.Repository;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.Master
{
    [HandleError]
    public class CommissionController : Controller
    {
        //public CommissionController()
        //    : this(new MCommissionRepository())
        //{ }

        private readonly IMCommissionRepository _mCommissionRepository;
        private readonly IMCommissionDetRepository _mCommissionDetRepository;

        public CommissionController(IMCommissionRepository mCommissionRepository, IMCommissionDetRepository mCommissionDetRepository)
        {
            Check.Require(mCommissionRepository != null, "mCommissionRepository may not be null");
            Check.Require(mCommissionDetRepository != null, "mCommissionDetRepository may not be null");

            this._mCommissionRepository = mCommissionRepository;
            this._mCommissionDetRepository = mCommissionDetRepository;
        }

        public ActionResult Index(string department)
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string department)
        {
            int totalRecords = 0;
            var commissions = _mCommissionRepository.GetPagedCommissionList(sidx, sord, page, rows, ref totalRecords, department);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from commission in commissions
                    select new
                    {
                        id = commission.Id.ToString(),
                        cell = new string[] {
                            Helper.CommonHelper.ConvertToString(commission.CommissionValue),
                            Helper.CommonHelper.ConvertToString(commission.CommissionStartDate),
                            Helper.CommonHelper.ConvertToString(commission.CommissionEndDate)
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Insert(MCommission viewModel, FormCollection formCollection, string department)
        {
            var commission = new MCommission();

            TransferFormValuesTo(commission, formCollection);
            commission.SetAssignedIdTo(Guid.NewGuid().ToString());
            commission.CommissionStatus = department;
            commission.CreatedDate = DateTime.Now;
            commission.CreatedBy = User.Identity.Name;
            commission.DataStatus = EnumDataStatus.New.ToString();
            _mCommissionRepository.Save(commission);

            try
            {
                _mCommissionRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCommissionRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Komisi berhasil dimasukkan");
        }

        [Transaction]
        public ActionResult Delete(MCommission viewModel, FormCollection formCollection)
        {
            MCommission commission = _mCommissionRepository.Get(viewModel.Id);

            if (commission != null)
            {
                _mCommissionRepository.Delete(commission);
            }

            try
            {
                _mCommissionRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCommissionRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Komisi Berhasil Dihapus");
        }

         [Transaction]
        public ActionResult Update(MCommission viewModel, FormCollection formCollection)
        {

            MCommission commission = _mCommissionRepository.Get(viewModel.Id);

            TransferFormValuesTo(commission, formCollection);
            commission.ModifiedDate = DateTime.Now;
            commission.ModifiedBy = User.Identity.Name;
            commission.DataStatus = EnumDataStatus.Updated.ToString();

            _mCommissionRepository.Update(commission);

            try
            {
                _mCommissionRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCommissionRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Komisi Berhasil Diupdate");
        }

        private static void TransferFormValuesTo(MCommission commission, FormCollection formCollection)
        {
            commission.CommissionStartDate = Helper.CommonHelper.ConvertToDate(formCollection["CommissionStartDate"]);
            commission.CommissionEndDate = Helper.CommonHelper.ConvertToDate(formCollection["CommissionEndDate"]);
            commission.CommissionValue = Helper.CommonHelper.ConvertToDecimal(formCollection["CommissionValue"]);
        }

        [Transaction]
        public ActionResult ListSub(string commissionId)
        {
        var commissionDets = _mCommissionDetRepository.GetCommissionDetListById(commissionId);
            
            var jsonData = new
            {
                rows = (
                    from commissionDet in commissionDets
                    select new
                    {
                        i = commissionDet.Id.ToString(),
                        cell = new string[]
                        {
                            commissionDet.Id,
                            commissionDet.DetailType,
                            Helper.CommonHelper.ConvertToString(commissionDet.DetailLowTarget),
                            Helper.CommonHelper.ConvertToString(commissionDet.DetailHighTarget),
                            Helper.CommonHelper.ConvertToString(commissionDet.DetailValue),
                            Helper.CommonHelper.ConvertToString(commissionDet.DetailCustomerNumber),
                            Helper.CommonHelper.ConvertToString(commissionDet.DetailTransportAllowance),
                            Helper.CommonHelper.ConvertToString(commissionDet.DetailIncentive),
                            Helper.CommonHelper.ConvertToString(commissionDet.DetailIncentiveSurveyAcc),
                            Helper.CommonHelper.ConvertToString(commissionDet.DetailIncentiveSurveyOnly)
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InsertSub(MCommissionDet viewModel, FormCollection formCollection, string commissionId)
        {
            var commissionDet = new MCommissionDet();
            commissionDet.CommissionId = _mCommissionRepository.Get(commissionId);
            TransferFormValuesTo(commissionDet, formCollection);
            commissionDet.SetAssignedIdTo(Guid.NewGuid().ToString());
            commissionDet.CreatedDate = DateTime.Now;
            commissionDet.CreatedBy = User.Identity.Name;
            commissionDet.DataStatus = EnumDataStatus.New.ToString();

            _mCommissionDetRepository.Save(commissionDet);

            try
            {
                _mCommissionDetRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCommissionDetRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Detail berhasil dimasukkan");
        }

        [Transaction]
        public ActionResult UpdateSub(MCommissionDet viewModel, FormCollection formCollection)
        {

            MCommissionDet commissionDet = _mCommissionDetRepository.Get(viewModel.Id);

            TransferFormValuesTo(commissionDet, formCollection);
            commissionDet.ModifiedDate = DateTime.Now;
            commissionDet.ModifiedBy = User.Identity.Name;
            commissionDet.DataStatus = EnumDataStatus.Updated.ToString();

            _mCommissionDetRepository.Update(commissionDet);

            try
            {
                _mCommissionDetRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCommissionDetRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Detail Berhasil Diupdate");
        }

        [Transaction]
        public ActionResult DeleteSub(MCommissionDet viewModel, FormCollection formCollection)
        {
            MCommissionDet commissionDet = _mCommissionDetRepository.Get(viewModel.Id);

            if (commissionDet != null)
            {
                _mCommissionDetRepository.Delete(commissionDet);
            }

            try
            {
                _mCommissionDetRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCommissionDetRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Detail Berhasil Dihapus");
        }

        private static void TransferFormValuesTo(MCommissionDet commissionDet, FormCollection formCollection)
        {
            commissionDet.DetailType = formCollection["DetailType"];
            commissionDet.DetailLowTarget = Helper.CommonHelper.ConvertToDecimal(formCollection["DetailLowTarget"]);
            commissionDet.DetailHighTarget = Helper.CommonHelper.ConvertToDecimal(formCollection["DetailHighTarget"]);
            commissionDet.DetailValue = Helper.CommonHelper.ConvertToDecimal(formCollection["DetailValue"]);
            commissionDet.DetailCustomerNumber = Helper.CommonHelper.ConvertToInteger(formCollection["DetailCustomerNumber"]);
            commissionDet.DetailTransportAllowance = Helper.CommonHelper.ConvertToDecimal(formCollection["DetailTransportAllowance"]);
            commissionDet.DetailIncentive = Helper.CommonHelper.ConvertToDecimal(formCollection["DetailIncentive"]);
            commissionDet.DetailIncentiveSurveyAcc = Helper.CommonHelper.ConvertToDecimal(formCollection["DetailIncentiveSurveyAcc"]);
            commissionDet.DetailIncentiveSurveyOnly = Helper.CommonHelper.ConvertToDecimal(formCollection["DetailIncentiveSurveyOnly"]);
        }
    }
}
