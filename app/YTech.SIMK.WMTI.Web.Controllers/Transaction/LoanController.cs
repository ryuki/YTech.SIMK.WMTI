using System;
using System.Linq;
using System.Web.Mvc;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Web.Controllers.ViewModel;

namespace YTech.SIMK.WMTI.Web.Controllers.Transaction
{
    [HandleError]
    public class LoanController : Controller
    {
        private readonly ITLoanRepository _tLoanRepository;
        private readonly ITLoanSurveyRepository _tLoanSurveyRepository;
        private readonly IMCustomerRepository _mCustomerRepository;

        public LoanController(ITLoanRepository tLoanRepository, ITLoanSurveyRepository tLoanSurveyRepository, IMCustomerRepository mCustomerRepository)
        {
            Check.Require(tLoanRepository != null, "tLoanRepository may not be null");
            Check.Require(tLoanSurveyRepository != null, "tLoanSurveyRepository may not be null");
            Check.Require(mCustomerRepository != null, "mCustomerRepository may not be null");

            _tLoanRepository = tLoanRepository;
            _tLoanSurveyRepository = tLoanSurveyRepository;
            _mCustomerRepository = mCustomerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }
        
        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;   
            var loans = _tLoanRepository.GetPagedLoanList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            var jsonData = new
                               {
                                   total = totalPages,
                                   page = page,
                                   records = totalRecords,
                                   rows = (
                                        from loan in loans
                                        select new
                                                   {
                                                       i = loan.Id.ToString(),
                                                       cell = new string[]
                                                                  {
                                                                    string.Empty,
                                                                    loan.Id,
                                                                    loan.LoanSurveyDate.ToString(),
                                                                    loan.CustomerId.PersonId.PersonFirstName,
                                                                    loan.CustomerId.PersonId.PersonLastName,
                                                                    loan.SurveyorId.PersonId.PersonFirstName,
                                                                    loan.ZoneId.ZoneName,
                                                                    loan.LoanStatus
                                                                  }
                                                   } 
                                   ).ToArray()
                               };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Survey()
        {
            ViewData["CurrentItem"] = "Lembaran Survey";
            SurveyFormViewModel viewModel =
                SurveyFormViewModel.CreateSurveyFormViewModel(_tLoanSurveyRepository, null);
            //if (usePopup.HasValue)
            //{
            //    if (usePopup.Value)
            //        return View("Registration", "MasterPopup", viewModel);
            //}
            return View(viewModel);
        }
    }
}
