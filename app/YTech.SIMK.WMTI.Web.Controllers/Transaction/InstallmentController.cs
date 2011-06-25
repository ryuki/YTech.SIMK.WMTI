using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
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
        public InstallmentController(ITLoanRepository loanRepository, ITInstallmentRepository installmentRepository)
        {
            Check.Require(loanRepository != null, "loanRepository may not be null");
            Check.Require(installmentRepository != null, "installmentRepository may not be null");

            this._loanRepository = loanRepository;
            this._installmentRepository = installmentRepository;
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
            InstallmentPaymentFormViewModel viewModel = InstallmentPaymentFormViewModel.Create(_installmentRepository, loanCode);
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Payment(TInstallment viewModel, FormCollection formCollection)
        {
            _installmentRepository.DbContext.BeginTransaction();
            TInstallment installment = _installmentRepository.Get(viewModel.Id);
            installment.InstallmentPaymentDate = viewModel.InstallmentPaymentDate;
            if (!string.IsNullOrEmpty(formCollection["InstallmentPaid"]))
                installment.InstallmentPaid = Convert.ToDecimal(formCollection["InstallmentPaid"].Replace(",", ""));
            installment.InstallmentStatus = EnumInstallmentStatus.Paid.ToString();
            installment.ModifiedBy = User.Identity.Name;
            installment.ModifiedDate = DateTime.Now;
            installment.DataStatus = EnumDataStatus.Updated.ToString();
            _installmentRepository.Update(installment);

            string Message = string.Empty;
            bool Success = true;
            try
            {
                _installmentRepository.DbContext.CommitTransaction();
                Success = true;
                Message = "Pembayaran Angsuran Berhasil Disimpan.";
            }
            catch (Exception ex)
            {
                Success = false;
                Message = ex.Message;
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
                          ins.InstallmentMaturityDate.HasValue ? ins.InstallmentMaturityDate.Value.ToString(Helper.CommonHelper.DateFormat) : null,
                         ins.InstallmentTotal.ToString(Helper.CommonHelper.NumberFormat),  
                         ins.InstallmentFine.HasValue ? ins.InstallmentFine.Value.ToString(Helper.CommonHelper.NumberFormat) : null,
                         ins.InstallmentMustPaid.ToString(Helper.CommonHelper.NumberFormat),  
                         ins.InstallmentPaid.HasValue ? ins.InstallmentPaid.Value.ToString(Helper.CommonHelper.NumberFormat) : null,  
                          ins.InstallmentPaymentDate.HasValue ? ins.InstallmentPaymentDate.Value.ToString(Helper.CommonHelper.DateFormat) : null,
                         ins.InstallmentSisa.ToString(Helper.CommonHelper.NumberFormat),
                          ins.EmployeeId != null ? ins.EmployeeId.PersonId.PersonName : null
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
