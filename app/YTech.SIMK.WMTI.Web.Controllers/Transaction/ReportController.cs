using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Data.Repository;
using YTech.SIMK.WMTI.Enums;
using YTech.SIMK.WMTI.Web.Controllers.ViewModel;
using Microsoft.Reporting.WebForms;
//using YTech.SIMK.WMTI.Web.Controllers.ViewModel.Reports;
//using Newtonsoft.Json;
//using System.Web.Script.Serialization;

namespace YTech.SIMK.WMTI.Web.Controllers.Transaction
{
    [HandleError]
    public class ReportController : Controller
    {
        private readonly ITLoanRepository _loanRepository;
        private readonly ITInstallmentRepository _installmentRepository;
        private readonly ITCommissionRepository _tCommissionRepository;
        private readonly ITRecPeriodRepository _tRecPeriodRepository;
        public ReportController(ITLoanRepository loanRepository, ITInstallmentRepository installmentRepository, ITCommissionRepository tCommissionRepository, ITRecPeriodRepository tRecPeriodRepository)
        {
            Check.Require(loanRepository != null, "loanRepository may not be null");
            Check.Require(installmentRepository != null, "installmentRepository may not be null");
            Check.Require(tCommissionRepository != null, "tCommissionRepository may not be null");
            Check.Require(tRecPeriodRepository != null, "tRecPeriodRepository may not be null");

            this._loanRepository = loanRepository;
            this._installmentRepository = installmentRepository;
            this._tCommissionRepository = tCommissionRepository;
            this._tRecPeriodRepository = tRecPeriodRepository;
        }

        [Transaction]
        public ActionResult Report(EnumReports reports, EnumDepartment? dep = null)
        {
            ReportParamViewModel viewModel = ReportParamViewModel.Create(_tRecPeriodRepository);
            string title = string.Empty;
            switch (reports)
            {
                case EnumReports.RptDueInstallment:
                    title = "Daftar Angsuran Jatuh Tempo";
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = false;
                    break;
                case EnumReports.RptCommission:
                    title = "Lap. Komisi Karyawan";
                    viewModel.ShowRecPeriod = true;
                    break;
            }
            ViewData["CurrentItem"] = title;


            ViewData["ExportFormat"] = new SelectList(Enum.GetValues(typeof(EnumExportFormat)));

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Report(EnumReports reports, EnumDepartment? dep, ReportParamViewModel viewModel, FormCollection formCollection)
        {
            ReportDataSource[] repCol = new ReportDataSource[1];
            DateTime? dateFrom = Helper.CommonHelper.ConvertToDate(formCollection["DateFrom"]);
            DateTime? dateTo = Helper.CommonHelper.ConvertToDate(formCollection["DateTo"]);
            switch (reports)
            {
                case EnumReports.RptDueInstallment:
                    repCol[0] = GetDueInstallment(dateFrom, dateTo);
                    break;
                case EnumReports.RptCommission:
                    repCol[0] = GetCommission(viewModel.RecPeriodId, dep);
                    break;
            }
            HttpContext.Session["ReportData"] = repCol;

            var e = new
            {
                Success = true,
                Message = "redirect",
                UrlReport = string.Format("{0}", reports.ToString())
            };
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        private ReportDataSource GetCommission(string recPeriodId, EnumDepartment? dep)
        {
            IEnumerable<TCommission> comms = _tCommissionRepository.GetListByRecapId(recPeriodId, dep);

            var list = from com in comms
                       select new
                       {
                           com.Id,
                           RecPeriodId = com.RecPeriodId.Id,
                           EmployeeId = com.EmployeeId != null ? com.EmployeeId.Id : null,
                           EmployeeName = com.EmployeeId != null ? com.EmployeeId.PersonId.PersonName : null,
                           com.RecPeriodId.PeriodTo,
                           com.RecPeriodId.PeriodFrom,
                           com.CommissionLevel,
                           CommissionType = GetStringValue(com.CommissionType),
                           com.CommissionValue,
                           com.CommissionFactor,
                           com.CommissionDesc,
                           com.CommissionStatus
                       }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("CommissionViewModel", list.ToList());
            return reportDataSource;
        }

        private string GetStringValue(string commType)
        {
            EnumCommissionType t = (EnumCommissionType)Enum.Parse(typeof(EnumCommissionType), commType);
            //if (t != null)
            {
                return Helper.CommonHelper.GetStringValue(t);
            }
            return string.Empty;
        }

        private ReportDataSource GetDueInstallment(DateTime? dateFrom, DateTime? dateTo)
        {
            IEnumerable<TInstallment> installments = _installmentRepository.GetListDueByDate(dateFrom);

            var list = from ins in installments
                       select new
                       {
                           ins.Id,
                           ins.LoanId.LoanCode,
                           CustomerName = ins.LoanId.PersonId.PersonName,
                           ins.InstallmentMaturityDate,
                           ins.InstallmentTotal,
                           ins.InstallmentFine,
                           ins.InstallmentMustPaid,
                           ins.InstallmentReceiptNo
                       }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("InstallmentViewModel", list.ToList());
            return reportDataSource;
        }


    }
}
