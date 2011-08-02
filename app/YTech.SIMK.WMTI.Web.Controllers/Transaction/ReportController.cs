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
        public ReportController(ITLoanRepository loanRepository, ITInstallmentRepository installmentRepository)
        {
            Check.Require(loanRepository != null, "loanRepository may not be null");
            Check.Require(installmentRepository != null, "installmentRepository may not be null");

            this._loanRepository = loanRepository;
            this._installmentRepository = installmentRepository;
        }

        [Transaction]
        public ActionResult Report(EnumReports reports)
        {
            ReportParamViewModel viewModel = ReportParamViewModel.Create();
            string title = string.Empty;
            switch (reports)
            {
                case EnumReports.RptDueInstallment:
                    title = "Daftar Angsuran Jatuh Tempo";
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = false;
                    break;
            }
            ViewData["CurrentItem"] = title;


            ViewData["ExportFormat"] = new SelectList(Enum.GetValues(typeof(EnumExportFormat)));

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Report(EnumReports reports, ReportParamViewModel viewModel, FormCollection formCollection)
        {
            ReportDataSource[] repCol = new ReportDataSource[1];
            DateTime? dateFrom = Helper.CommonHelper.ConvertToDate(formCollection["DateFrom"]);
            DateTime? dateTo = Helper.CommonHelper.ConvertToDate(formCollection["DateTo"]);
            switch (reports)
            {
                case EnumReports.RptDueInstallment:
                    repCol[0] = GetDueInstallment(dateFrom, dateTo);
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
                           ins.InstallmentMustPaid
                       }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("InstallmentViewModel", list.ToList());
            return reportDataSource;
        }


    }
}
