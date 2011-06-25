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
        private readonly ITInstallmentRepository _tInstallmentRepository;

        public LoanController(ITLoanRepository tLoanRepository, ITLoanSurveyRepository tLoanSurveyRepository, IMCustomerRepository mCustomerRepository, IRefAddressRepository refAddressRepository, IRefPersonRepository refPersonRepository, ITInstallmentRepository tInstallmentRepository)
        {
            Check.Require(tLoanRepository != null, "tLoanRepository may not be null");
            Check.Require(tLoanSurveyRepository != null, "tLoanSurveyRepository may not be null");
            Check.Require(mCustomerRepository != null, "mCustomerRepository may not be null");
            Check.Require(refAddressRepository != null, "refAddressRepository may not be null");
            Check.Require(refPersonRepository != null, "refPersonRepository may not be null");
            Check.Require(tInstallmentRepository != null, "tInstallmentRepository may not be null");

            _tLoanRepository = tLoanRepository;
            _tLoanSurveyRepository = tLoanSurveyRepository;
            _mCustomerRepository = mCustomerRepository;
            _refAddressRepository = refAddressRepository;
            _refPersonRepository = refPersonRepository;
            _tInstallmentRepository = tInstallmentRepository;
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
                        i = loan.Id,
                        cell = new string[]
                            {
                            string.Empty,
                           loan.Surveys.Count > 0 ? loan.Surveys[0].Id : null,
                           loan.Id,
                            loan.LoanNo,
                            loan.LoanCode,
                            loan.LoanSurveyDate.HasValue ? loan.LoanSurveyDate.Value.ToString(Helper.CommonHelper.DateFormat) : null,
                            loan.PersonId.PersonName,
                            loan.SurveyorId != null ?  loan.SurveyorId.PersonId.PersonName : null,
                            loan.ZoneId != null ? loan.ZoneId.ZoneName : null,
                            loan.LoanStatus
                            }
                    }
                ).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Survey(string loanSurveyId)
        {
            ViewData["CurrentItem"] = "Lembaran Survey";
            SurveyFormViewModel viewModel =
                SurveyFormViewModel.CreateSurveyFormViewModel(_tLoanSurveyRepository, loanSurveyId);

            return View(viewModel);
        }

        [Transaction]
        public ActionResult EditSurvey(string loanSurveyId)
        {
            ViewData["CurrentItem"] = "Lembaran Survey";
            SurveyFormViewModel viewModel =
                SurveyFormViewModel.CreateSurveyFormViewModel(_tLoanSurveyRepository, loanSurveyId);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Survey(TLoanSurvey surveyVM, TLoan loanVM, FormCollection formCollection, string loanSurveyId)
        {
            try
            {
                _tLoanSurveyRepository.DbContext.BeginTransaction();

                TLoan loan = new TLoan();
                TLoanSurvey survey = new TLoanSurvey();
                MCustomer customer = new MCustomer();
                RefPerson person = new RefPerson();
                RefAddress address = new RefAddress();
                bool isSave = true;
                if (!string.IsNullOrEmpty(loanSurveyId))
                {
                    survey = _tLoanSurveyRepository.Get(loanSurveyId);
                    if (survey != null)
                    {
                        isSave = false;
                        loan = survey.LoanId;
                        address = loan.AddressId;
                        person = loan.PersonId;
                        customer = loan.CustomerId;
                    }
                } 

                //save address
                TransferFormValuesTo(address, formCollection);
                if (isSave)
                {
                    address.SetAssignedIdTo(Guid.NewGuid().ToString());
                    address.CreatedDate = DateTime.Now;
                    address.CreatedBy = User.Identity.Name;
                    address.DataStatus = EnumDataStatus.New.ToString();
                    _refAddressRepository.Save(address);
                }
                else
                {
                    address.ModifiedDate = DateTime.Now;
                    address.ModifiedBy = User.Identity.Name;
                    address.DataStatus = EnumDataStatus.Updated.ToString();
                    _refAddressRepository.Update(address);
                }
                
                //save person
                TransferFormValuesTo(person, formCollection);
                if (isSave)
                {
                    person.SetAssignedIdTo(Guid.NewGuid().ToString());
                    person.CreatedDate = DateTime.Now;
                    person.CreatedBy = User.Identity.Name;
                    person.DataStatus = EnumDataStatus.New.ToString();
                    _refPersonRepository.Save(person);
                }
                else
                {
                    person.ModifiedDate = DateTime.Now;
                    person.ModifiedBy = User.Identity.Name;
                    person.DataStatus = EnumDataStatus.Updated.ToString();
                    _refPersonRepository.Update(person);
                }

                //save customer
                customer.AddressId = address;
                customer.PersonId = person;
                if (isSave)
                {
                    customer.SetAssignedIdTo(Guid.NewGuid().ToString());
                    customer.CreatedDate = DateTime.Now;
                    customer.CreatedBy = User.Identity.Name;
                    customer.DataStatus = EnumDataStatus.New.ToString();

                    _mCustomerRepository.Save(customer);
                }
                else
                {
                    customer.ModifiedDate = DateTime.Now;
                    customer.ModifiedBy = User.Identity.Name;
                    customer.DataStatus = EnumDataStatus.Updated.ToString();
                    _mCustomerRepository.Update(customer);
                }

                //save loan
                loan.AddressId = address;
                loan.PersonId = person;
                loan.CustomerId = customer;

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
                loan.LoanBasicInstallment = loanVM.LoanBasicInstallment;
                loan.LoanMaturityDate = loanVM.LoanMaturityDate;
                if (isSave)
                {
                    loan.SetAssignedIdTo(Guid.NewGuid().ToString());
                    loan.CreatedDate = DateTime.Now;
                    loan.CreatedBy = User.Identity.Name;
                    loan.DataStatus = EnumDataStatus.New.ToString();
                    _tLoanRepository.Save(loan);
                }
                else
                {
                    loan.ModifiedDate = DateTime.Now;
                    loan.ModifiedBy = User.Identity.Name;
                    loan.DataStatus = EnumDataStatus.Updated.ToString();
                    _tLoanRepository.Update(loan);
                }

                //save survey
                survey.LoanId = loan;
                survey.SurveyDate = surveyVM.SurveyDate;
                survey.SurveyDesc = surveyVM.SurveyDesc;
                survey.SurveyHouseType = surveyVM.SurveyHouseType;
                survey.SurveyNeighbor = surveyVM.SurveyNeighbor;
                survey.SurveyNeighborAsset = surveyVM.SurveyNeighborAsset;
                survey.SurveyNeighborCharacter = surveyVM.SurveyNeighborCharacter;
                survey.SurveyNeighborConclusion = surveyVM.SurveyNeighborConclusion;
                survey.SurveyStatus = EnumSurveyStatus.New.ToString();
                survey.SurveyUnitDeliverAddress = surveyVM.SurveyUnitDeliverAddress;
                survey.SurveyUnitDeliverDate = surveyVM.SurveyUnitDeliverDate;
                survey.SurveyReceivedBy = surveyVM.SurveyReceivedBy;
                survey.SurveyProcessBy = surveyVM.SurveyProcessBy;
                if (isSave)
                {
                    survey.SetAssignedIdTo(Guid.NewGuid().ToString());
                    survey.CreatedDate = DateTime.Now;
                    survey.CreatedBy = User.Identity.Name;
                    survey.DataStatus = EnumDataStatus.New.ToString();
                    _tLoanSurveyRepository.Save(survey);
                }
                else
                {
                    survey.ModifiedDate = DateTime.Now;
                    survey.ModifiedBy = User.Identity.Name;
                    survey.DataStatus = EnumDataStatus.Updated.ToString();
                    _tLoanSurveyRepository.Update(survey);
                }

                _tLoanSurveyRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {
                _tLoanSurveyRepository.DbContext.RollbackTransaction();

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

        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Approve(string loanId)
        {
            _tLoanSurveyRepository.DbContext.BeginTransaction();
            TLoan loan = _tLoanRepository.Get(loanId);
            if (loan != null)
            {
                //loan.LoanAccBy = User.Identity.Name;
                loan.LoanAccDate = DateTime.Now;
                loan.LoanStatus = EnumLoanStatus.Approve.ToString();

                loan.ModifiedBy = User.Identity.Name;
                loan.ModifiedDate = DateTime.Now;
                loan.DataStatus = EnumDataStatus.Updated.ToString();
                _tLoanRepository.Update(loan);

                //save installment
                SaveInstallment(loan);
            }

            string Message = string.Empty;
            bool Success = true;
            try
            {
                _tLoanSurveyRepository.DbContext.CommitTransaction();
                Success = true;
                Message = "Approve Kredit Berhasil.";
            }
            catch (Exception ex)
            {
                Success = false;
                Message = ex.GetBaseException().Message;
                _tLoanSurveyRepository.DbContext.RollbackTransaction();
            }
            var e = new
            {
                Success,
                Message
            };
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        private void SaveInstallment(TLoan loan)
        {
            if (loan.LoanTenor.HasValue)
            {
                TInstallment ins = null;
                DateTime startDate = Convert.ToDateTime(string.Format("{1:yyyy-MM}-{0}", loan.LoanMaturityDate.Value, loan.LoanAccDate.Value));
                for (int i = 0; i < loan.LoanTenor.Value; i++)
                {
                    ins = new TInstallment();
                    ins.SetAssignedIdTo(Guid.NewGuid().ToString());
                    ins.LoanId = loan;
                    ins.InstallmentBasic = loan.LoanBasicInstallment;
                    ins.InstallmentInterest = loan.LoanInterest;
                    ins.InstallmentOthers = loan.LoanOtherInstallment;
                    ins.InstallmentNo = i + 1;
                    ins.InstallmentStatus = EnumInstallmentStatus.Not_Paid.ToString();
                    ins.InstallmentMaturityDate = startDate.AddMonths(i + 1);
                    ins.DataStatus = EnumDataStatus.New.ToString();
                    ins.CreatedBy = User.Identity.Name;
                    ins.CreatedDate = DateTime.Now;
                    _tInstallmentRepository.Save(ins);
                }
            }
        }
    }
}
