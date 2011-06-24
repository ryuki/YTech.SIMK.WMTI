using System;
using System.Linq;
using System.Web.Mvc;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;
using YTech.SIMK.WMTI.Web.Controllers.ViewModel;

namespace YTech.SIMK.WMTI.Web.Controllers.Transaction
{
    [HandleError]
    public class LoanController : Controller
    {
        private readonly ITLoanRepository _tLoanRepository;
        private readonly ITLoanSurveyRepository _tLoanSurveyRepository;
        private readonly IMCustomerRepository _mCustomerRepository;
        private readonly IRefAddressRepository _refAddressRepository;
        private readonly IRefPersonRepository _refPersonRepository;

        public LoanController(ITLoanRepository tLoanRepository, ITLoanSurveyRepository tLoanSurveyRepository, IMCustomerRepository mCustomerRepository, IRefAddressRepository refAddressRepository, IRefPersonRepository refPersonRepository)
        {
            Check.Require(tLoanRepository != null, "tLoanRepository may not be null");
            Check.Require(tLoanSurveyRepository != null, "tLoanSurveyRepository may not be null");
            Check.Require(mCustomerRepository != null, "mCustomerRepository may not be null");
            Check.Require(refAddressRepository != null, "refAddressRepository may not be null");
            Check.Require(refPersonRepository != null, "refPersonRepository may not be null");

            _tLoanRepository = tLoanRepository;
            _tLoanSurveyRepository = tLoanSurveyRepository;
            _mCustomerRepository = mCustomerRepository;
            _refAddressRepository = refAddressRepository;
            _refPersonRepository = refPersonRepository;
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
        public ActionResult Survey(TLoanSurvey surveyVM, TLoan  loanVM, FormCollection formCollection)
        {
            _tLoanSurveyRepository.DbContext.BeginTransaction();

            TLoan loan = new TLoan();
            TLoanSurvey survey = new TLoanSurvey(); 
            MCustomer customer = new MCustomer();
            RefPerson person = new RefPerson();
            RefAddress address = new RefAddress();

            //save address
            TransferFormValuesTo(address, formCollection);
            address.SetAssignedIdTo(Guid.NewGuid().ToString());
            address.CreatedDate = DateTime.Now;
            address.CreatedBy = User.Identity.Name;
            address.DataStatus = EnumDataStatus.New.ToString();
            _refAddressRepository.Save(address);

            //save person
            TransferFormValuesTo(person, formCollection);
            person.SetAssignedIdTo(Guid.NewGuid().ToString());
            person.CreatedDate = DateTime.Now;
            person.CreatedBy = User.Identity.Name;
            person.DataStatus = EnumDataStatus.New.ToString();
            _refPersonRepository.Save(person);

            //save customer
            customer.SetAssignedIdTo(Guid.NewGuid().ToString());
            customer.CreatedDate = DateTime.Now;
            customer.CreatedBy = User.Identity.Name;
            customer.DataStatus = EnumDataStatus.New.ToString();
			
            customer.AddressId = address;
            customer.PersonId = person;
            _mCustomerRepository.Save(customer);

            //save loan
            loan.SetAssignedIdTo(Guid.NewGuid().ToString());
            loan.CreatedDate = DateTime.Now;
            loan.CreatedBy = User.Identity.Name;
            loan.DataStatus = EnumDataStatus.New.ToString();

            loan.AddressId = address;
            loan.PersonId = person;
            loan.CustomerId = customer;
            loan.CollectorId = loanVM.CollectorId;
            loan.LoanDownPayment = loanVM.LoanDownPayment;
            loan.CollectorId = loanVM.CollectorId;
            loan.LoanCode = loanVM.LoanCode;
            loan.LoanCreditPrice = loanVM.LoanCreditPrice;
            loan.LoanDesc = loanVM.LoanDesc;
            loan.LoanDownPayment = loanVM.LoanDownPayment;
            loan.LoanIsSalesmanKnownCustomer = loanVM.LoanIsSalesmanKnownCustomer;
            loan.LoanTenor = loanVM.LoanTenor;
            loan.LoanNo = loanVM.LoanNo;
            loan.LoanStatus = loanVM.LoanStatus;
            loan.LoanUnitPriceTotal = loanVM.LoanUnitPriceTotal;
            _tLoanRepository.Save(loan);

            survey.SetAssignedIdTo(Guid.NewGuid().ToString());
            survey.CreatedDate = DateTime.Now;
            survey.CreatedBy = User.Identity.Name;
            survey.DataStatus = EnumDataStatus.New.ToString();
            survey.LoanId = loan;
            survey.SurveyDate = surveyVM.SurveyDate;
            survey.SurveyDesc = surveyVM.SurveyDesc;
            survey.SurveyHouseType = surveyVM.SurveyHouseType;
            survey.SurveyNeighbor = surveyVM.SurveyNeighbor;
            survey.SurveyNeighborAsset = surveyVM.SurveyNeighborAsset;
            survey.SurveyNeighborCharacter = surveyVM.SurveyNeighborCharacter;
            survey.SurveyNeighborConclusion = surveyVM.SurveyNeighborConclusion;
            survey.SurveyStatus = surveyVM.SurveyStatus;
            survey.SurveyUnitDeliverAddress = surveyVM.SurveyUnitDeliverAddress;
            survey.SurveyUnitDeliverDate = surveyVM.SurveyUnitDeliverDate;
            _tLoanSurveyRepository.Save(survey);

            try
            {
                _tLoanSurveyRepository.DbContext.CommitChanges();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
            }
            catch (Exception e)
            {
                _tLoanSurveyRepository.DbContext.RollbackTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
                TempData[EnumCommonViewData.ErrorMessage.ToString()] = e.Message;

                var result = new
                {
                    Success = false,
                    Message = e.GetBaseException().Message
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

            person.PersonPob = formCollection["PersonPob"];
            person.PersonPhone = formCollection["PersonPhone"];
            person.PersonMobile = formCollection["PersonMobile"];
            person.PersonOccupation = formCollection["PersonOccupation"];
            person.PersonLastEducation = formCollection["PersonLastEducation"];
            if (!string.IsNullOrEmpty(formCollection["PersonAge"]))
                person.PersonAge = Convert.ToDecimal(formCollection["PersonAge"]);
            person.PersonReligion = formCollection["PersonReligion"];
            if (!string.IsNullOrEmpty(formCollection["PersonIncome"]))
                person.PersonIncome = Convert.ToDecimal(formCollection["PersonIncome"]);
            if (!string.IsNullOrEmpty(formCollection["PersonNoOfChildren"]))
                person.PersonNoOfChildren = Convert.ToDecimal(formCollection["PersonNoOfChildren"]);
            person.PersonMarriedStatus = formCollection["PersonMarriedStatus"];
            person.PersonCoupleName = formCollection["PersonCoupleName"];
            person.PersonCoupleOccupation = formCollection["PersonCoupleOccupation"];
            if (!string.IsNullOrEmpty(formCollection["PersonCoupleIncome"]))
                person.PersonCoupleIncome = Convert.ToDecimal(formCollection["PersonCoupleIncome"]);
            if (!string.IsNullOrEmpty(formCollection["PersonStaySince"]))
                person.PersonStaySince = Convert.ToDateTime(formCollection["PersonStaySince"]);
            person.PersonGuarantorName = formCollection["PersonGuarantorName"];
            person.PersonGuarantorRelationship = formCollection["PersonGuarantorRelationship"];
            person.PersonGuarantorOccupation = formCollection["PersonGuarantorOccupation"];
            person.PersonGuarantorPhone = formCollection["PersonGuarantorPhone"];
            if (!string.IsNullOrEmpty(formCollection["PersonGuarantorStaySince"]))
                person.PersonGuarantorStaySince = Convert.ToDateTime(formCollection["PersonGuarantorStaySince"]);
            person.PersonGuarantorHouseOwnerStatus = formCollection["PersonGuarantorHouseOwnerStatus"];
        }

        private void TransferFormValuesTo(RefAddress address, FormCollection formCollection)
        {
            address.AddressLine1 = formCollection["AddressLine1"];
            address.AddressLine2 = formCollection["AddressLine2"];
            address.AddressPostCode = formCollection["AddressPostCode"];
        }

        private void TransferFormValuesTo(TLoanSurvey loanSurvey, FormCollection formCollection)
        {
            loanSurvey.SurveyNeighbor = formCollection["SurveyNeighbor"];
            loanSurvey.SurveyNeighborCharacter = formCollection["SurveyNeighborCharacter"];
            loanSurvey.SurveyNeighborConclusion = formCollection["SurveyNeighborConclusion"];
            loanSurvey.SurveyNeighborAsset = formCollection["SurveyNeighborAsset"];
            loanSurvey.SurveyHouseType = formCollection["SurveyHouseType"];
            loanSurvey.SurveyUnitDeliverDate = Convert.ToDateTime(formCollection["SurveyUnitDeliverDate"]);
            loanSurvey.SurveyUnitDeliverAddress = formCollection["SurveyUnitDeliverAddress"];
        }
    }
}
