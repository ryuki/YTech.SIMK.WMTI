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
    public class InstallmentController : Controller
    {
        private readonly ITLoanRepository _loanRepository;
        private readonly ITInstallmentRepository _installmentRepository;
        private readonly IMEmployeeRepository _mEmployeeRepository;
        public InstallmentController(ITLoanRepository loanRepository, ITInstallmentRepository installmentRepository, IMEmployeeRepository mEmployeeRepository)
        {
            Check.Require(loanRepository != null, "loanRepository may not be null");
            Check.Require(installmentRepository != null, "installmentRepository may not be null");
            Check.Require(mEmployeeRepository != null, "mEmployeeRepository may not be null");

            this._loanRepository = loanRepository;
            this._installmentRepository = installmentRepository;
            this._mEmployeeRepository = mEmployeeRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        [Transaction]
        public ActionResult Payment(string loanCode)
        {
            ViewData["CurrentItem"] = "Pembayaran Angsuran";
            InstallmentPaymentFormViewModel viewModel = InstallmentPaymentFormViewModel.Create(_installmentRepository, _mEmployeeRepository, loanCode);
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Payment(TInstallment viewModel, FormCollection formCollection)
        {
            string Message = string.Empty;
            bool Success = true;

            try
            {
                _installmentRepository.DbContext.BeginTransaction();
                TInstallment installment = _installmentRepository.Get(viewModel.Id);

                ////validate receipt no
                //if (installment.InstallmentReceiptNo != formCollection["ReceiptNo"])
                //{
                //    throw new Exception("No kwitansi salah, data angsuran tidak dapat disimpan.");
                //}

                installment.EmployeeId = viewModel.EmployeeId;
                installment.InstallmentPaymentDate = Helper.CommonHelper.ConvertToDate(formCollection["InstallmentPaymentDate"]);
                installment.InstallmentPaid = Helper.CommonHelper.ConvertToDecimal(formCollection["InstallmentPaid"]);
                installment.InstallmentFine = Helper.CommonHelper.ConvertToDecimal(formCollection["InstallmentFine"]);
                installment.InstallmentStatus = EnumInstallmentStatus.Paid.ToString();
                installment.InstallmentReceiptNo = formCollection["ReceiptNo"];

                installment.ModifiedBy = User.Identity.Name;
                installment.ModifiedDate = DateTime.Now;
                installment.DataStatus = EnumDataStatus.Updated.ToString();
                _installmentRepository.Update(installment);

                //update loan status to Paid when all installment have been paid
                _loanRepository.UpdateLoanToPaid(installment.LoanId.Id);

                _installmentRepository.DbContext.CommitTransaction();
                Success = true;
                Message = "Pembayaran Angsuran Berhasil Disimpan.";
            }
            catch (Exception ex)
            {
                Success = false;
                Message = "Error :\n" + ex.GetBaseException().Message;
                _installmentRepository.DbContext.RollbackTransaction();
            }
            var e = new
            {
                Success,
                Message
            };
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string loanCode)
        {
            int totalRecords = 0;
            var installmentList = _installmentRepository.GetPagedInstallmentList(sidx, sord, page, rows, ref totalRecords, loanCode);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            decimal sisa = 0;
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from ins in installmentList
                    select new
                    {
                        i = ins.Id.ToString(),
                        cell = new string[] {
                            ins.Id,  
                         ins.InstallmentNo.HasValue ? ins.InstallmentNo.Value.ToString() : null,  
                          Helper.CommonHelper.ConvertToString(ins.InstallmentMaturityDate),
                         Helper.CommonHelper.ConvertToString(ins.InstallmentTotal),
                         Helper.CommonHelper.ConvertToString(ins.InstallmentFine), 
                         Helper.CommonHelper.ConvertToString(ins.InstallmentMustPaid), 
                         Helper.CommonHelper.ConvertToString(ins.InstallmentPaid),
                          Helper.CommonHelper.ConvertToString(ins.InstallmentPaymentDate),
                          Helper.CommonHelper.ConvertToString(GetCummulative(ins.InstallmentSisa)),
                          ins.EmployeeId != null ? ins.EmployeeId.PersonId.PersonName : null
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private decimal tempSisa = 0;
        private decimal? GetCummulative(decimal? installmentSisa)
        {
            if (installmentSisa.HasValue)
            {
                tempSisa += installmentSisa.Value;
                return tempSisa;
            }
            return null;
        }

        public ActionResult PrintReceipt()
        {
            PrintReceiptViewModel viewModel = PrintReceiptViewModel.Create();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PrintReceipt(PrintReceiptViewModel viewModel, FormCollection formCollection)
        {
            string Message = string.Empty;
            bool Success = true;
            try
            {
                ReportDataSource[] repCol = new ReportDataSource[1];
                repCol[0] = GetInstallment(_installmentRepository, viewModel.LoanAcc, viewModel.InstallmentNo);

                HttpContext.Session["ReportData"] = repCol;
                Success = true;
                Message = "redirect";
            }
            catch (Exception ex)
            {
                Success = false;
                Message = "Error :\n" + ex.GetBaseException().Message;
            }

            var e = new
            {
                Success = Success,
                Message = Message,
                UrlReport = string.Format("{0}", EnumReports.RptPrintInstallment.ToString())
            };
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        public static ReportDataSource GetInstallment(ITInstallmentRepository installmentRepository, string loanAcc, int installmentNo)
        {
            IEnumerable<TInstallment> installments = installmentRepository.GetInstallment(loanAcc, installmentNo);

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
                           ins.InstallmentReceiptNo,
                           CustomerAddress = ins.LoanId.AddressId.AddressLine1,
                           ins.InstallmentNo,
                           ins.LoanId.LoanTenor,
                           ins.LoanId.LoanUnits[0].UnitName,
                           ins.LoanId.LoanUnits[0].UnitType
                       }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("InstallmentViewModel", list.ToList());
            return reportDataSource;
        }
    }
}
