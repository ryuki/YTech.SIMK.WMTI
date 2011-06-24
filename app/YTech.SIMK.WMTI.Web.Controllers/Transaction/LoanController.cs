using System;
using System.Linq;
using System.Web.Mvc;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
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

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Registration(TLoanSurvey loanSurvey, FormCollection formCollection)
        {
            _tLoanSurveyRepository.DbContext.BeginTransaction();

            TLoan loan = new TLoan();
            MCustomer customer = new MCustomer();
            RefPerson person = new RefPerson();
            RefAddress address = new RefAddress();

            try
            {
                _tLoanSurveyRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {
                _tLoanSurveyRepository.DbContext.RollbackTransaction();

                var result = new
                {
                    Success = false,
                    Message = e.Message
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var resultx = new
            {
                Success = true,
                Message = @" <div class='ui-state-highlight ui-corner-all' style='padding: 5pt; margin-bottom: 5pt;'>
                             <p>
                                <span class='ui-icon ui-icon-info' style='float: left; margin-right: 0.3em;'></span>
                                Data berhasil disimpan.</p>
                             </div>"
            };

            return Json(resultx, JsonRequestBehavior.AllowGet);
        }

        private void TransferFormValuesTo(RefPerson person, FormCollection formCollection)
        {
            person.PersonFirstName = formCollection["PersonFirstName"];
            person.PersonGender = formCollection["PersonGender"];
            person.PersonIdCardNo = formCollection["PersonIdCardNo"];

            if (!string.IsNullOrEmpty(formCollection["PersonDob"]))
                person.PersonDob = Convert.ToDateTime(formCollection["PersonDob"]);
            else
                person.PersonDob = null;

            person.PersonPob = formCollection["PersonPob"];
            person.PersonPhone = formCollection["PersonPhone"];
            person.PersonMobile = formCollection["PersonMobile"];
            person.PersonOccupation = formCollection["PersonOccupation"];
            person.PersonLastEducation = formCollection["PersonLastEducation"];
            person.PersonAge = Convert.ToDecimal(formCollection["PersonAge"]);
            person.PersonReligion = formCollection["PersonReligion"];
            person.PersonIncome = Convert.ToDecimal(formCollection["PersonIncome"]);
            person.PersonNoOfChildren = Convert.ToDecimal(formCollection["PersonNoOfChildren"]);
            person.PersonMarriedStatus = formCollection["PersonMarriedStatus"];
            person.PersonCoupleName = formCollection["PersonCoupleName"];
            person.PersonCoupleOccupation = formCollection["PersonCoupleOccupation"];
            person.PersonCoupleIncome = Convert.ToDecimal(formCollection["PersonCoupleIncome"]);
            person.PersonStaySince = Convert.ToDateTime(formCollection["PersonStaySince"]);
            person.PersonGuarantorName = formCollection["PersonGuarantorName"];
        }
    }
}
