using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
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
                          (ins.InstallmentPaid.Value - ins.InstallmentMustPaid).ToString(Helper.CommonHelper.NumberFormat),
                          ins.EmployeeId != null ? ins.EmployeeId.PersonId.PersonName : null
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
