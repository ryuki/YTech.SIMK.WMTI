using System;
using System.Linq;
using System.Text;
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
        private readonly IMEmployeeRepository _mEmployeeRepository;
        private readonly ITLoanUnitRepository _tLoanUnitRepository;
        private readonly IMZoneRepository _mZoneRepository;
        private readonly IMPartnerRepository _mPartnerRepository;
        private readonly IMDepartmentRepository _mDepartmentRepository;

        public LoanController(ITLoanRepository tLoanRepository, ITLoanSurveyRepository tLoanSurveyRepository, IMCustomerRepository mCustomerRepository, IRefAddressRepository refAddressRepository, IRefPersonRepository refPersonRepository, ITInstallmentRepository tInstallmentRepository, IMEmployeeRepository mEmployeeRepository, ITLoanUnitRepository tLoanUnitRepository, IMZoneRepository mZoneRepository, IMPartnerRepository mPartnerRepository, IMDepartmentRepository mDepartmentRepository)
        {
            Check.Require(tLoanRepository != null, "tLoanRepository may not be null");
            Check.Require(tLoanSurveyRepository != null, "tLoanSurveyRepository may not be null");
            Check.Require(mCustomerRepository != null, "mCustomerRepository may not be null");
            Check.Require(refAddressRepository != null, "refAddressRepository may not be null");
            Check.Require(refPersonRepository != null, "refPersonRepository may not be null");
            Check.Require(tInstallmentRepository != null, "tInstallmentRepository may not be null");
            Check.Require(mEmployeeRepository != null, "mEmployeeRepository may not be null");
            Check.Require(tLoanUnitRepository != null, "tLoanUnitRepository may not be null");
            Check.Require(mZoneRepository != null, "mZoneRepository may not be null");
            Check.Require(mPartnerRepository != null, "mPartnerRepository may not be null");
            Check.Require(mDepartmentRepository != null, "mDepartmentRepository may not be null");

            _tLoanRepository = tLoanRepository;
            _tLoanSurveyRepository = tLoanSurveyRepository;
            _mCustomerRepository = mCustomerRepository;
            _refAddressRepository = refAddressRepository;
            _refPersonRepository = refPersonRepository;
            _tInstallmentRepository = tInstallmentRepository;
            _mEmployeeRepository = mEmployeeRepository;
            _tLoanUnitRepository = tLoanUnitRepository;
            _mZoneRepository = mZoneRepository;
            _mPartnerRepository = mPartnerRepository;
            _mDepartmentRepository = mDepartmentRepository;
        }

        public ActionResult Index(string loanStatus)
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string loanStatus, string searchBy, string searchText)
        {

            int totalRecords = 0;
            var loans = _tLoanRepository.GetPagedLoanList(sidx, sord, page, rows, ref totalRecords, loanStatus, searchBy, searchText);

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
                                                //loan.LoanSubmissionDate.HasValue ? loan.LoanSubmissionDate.Value.ToString(Helper.CommonHelper.DateFormat) : null,
                                                Helper.CommonHelper.ConvertToString(loan.LoanSubmissionDate),
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
        public ActionResult CustomerRequest(string loanCustomerRequestId)
        {
            ViewData["CurrentItem"] = "Lembar Permohonan Konsumen";

            CRFormViewModel viewModel = CRFormViewModel.CreateCRFormViewModel(_tLoanRepository, _mEmployeeRepository, loanCustomerRequestId);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CustomerRequest(TLoan loanVM, TLoanUnit loanUnitVM, RefPerson personVM, RefAddress addressVM, FormCollection formCollection, string loanCustomerRequestId)
        {
            try
            {
                _tLoanRepository.DbContext.BeginTransaction();

                TLoan loan = new TLoan();
                TLoanSurvey loanSurvey = new TLoanSurvey();
                TLoanUnit unit = new TLoanUnit();
                MCustomer customer = new MCustomer();
                RefPerson person = new RefPerson();
                RefAddress address = new RefAddress();

                bool isSave = true;

                if (!string.IsNullOrEmpty(loanCustomerRequestId))
                {
                    loan = _tLoanRepository.Get(loanCustomerRequestId);
                    if (loan != null)
                    {
                        isSave = false;

                        customer = loan.CustomerId;
                        address = loan.CustomerId.AddressId;
                        person = loan.CustomerId.PersonId;

                        if (loan.LoanUnits.Count > 0)
                            unit = loan.LoanUnits[0];
                        else
                            unit = new TLoanUnit();

                    }
                }

                //save address
                TransferFormValuesTo(address, formCollection);
                address.AddressOffice = addressVM.AddressOffice;
                address.AddressOfficePhone = addressVM.AddressOfficePhone;

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
                person.PersonOccupationSector = personVM.PersonOccupationSector;
                person.PersonOccupationPosition = personVM.PersonOccupationPosition;

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

                loan.TLSId = loanVM.TLSId;
                loan.SalesmanId = loanVM.SalesmanId;
                loan.SurveyorId = loanVM.SurveyorId;
                loan.PartnerId = loanVM.PartnerId;

                loan.LoanNo = loanVM.LoanNo;
                loan.LoanBasicPrice = Helper.CommonHelper.ConvertToDecimal(formCollection["LoanBasicPrice"]);

                loan.LoanCreditPrice = Helper.CommonHelper.ConvertToDecimal(formCollection["LoanCreditPrice"]);

                loan.LoanSubmissionDate = Helper.CommonHelper.ConvertToDate(formCollection["LoanSubmissionDate"]); //loanVM.LoanSubmissionDate;

                if (formCollection["LoanAdminFee"].Contains("true"))
                    loan.LoanAdminFee = 14000;

                if (formCollection["LoanMateraiFee"].Contains("true"))
                    loan.LoanMateraiFee = 6000;

                loan.LoanTenor = loanVM.LoanTenor;

                loan.LoanDownPayment = Helper.CommonHelper.ConvertToDecimal(formCollection["LoanDownPayment"]);

                loan.LoanBasicInstallment = Helper.CommonHelper.ConvertToDecimal(formCollection["LoanBasicInstallment"]);

                if (isSave)
                {
                    loan.SetAssignedIdTo(Guid.NewGuid().ToString());
                    loan.CreatedDate = DateTime.Now;
                    loan.CreatedBy = User.Identity.Name;
                    loan.DataStatus = EnumDataStatus.New.ToString();
                    loan.LoanStatus = EnumLoanStatus.Survey.ToString();
                    _tLoanRepository.Save(loan);
                }
                else
                {
                    loan.ModifiedDate = DateTime.Now;
                    loan.ModifiedBy = User.Identity.Name;
                    loan.DataStatus = EnumDataStatus.Updated.ToString();
                    _tLoanRepository.Update(loan);
                }

                //save unit
                unit.LoanId = loan;
                unit.UnitType = loanUnitVM.UnitType;
                unit.UnitName = loanUnitVM.UnitName;

                if (isSave)
                {
                    unit.SetAssignedIdTo(Guid.NewGuid().ToString());
                    unit.CreatedDate = DateTime.Now;
                    unit.CreatedBy = User.Identity.Name;
                    unit.DataStatus = EnumDataStatus.New.ToString();
                    _tLoanUnitRepository.Save(unit);
                }
                else
                {
                    unit.ModifiedDate = DateTime.Now;
                    unit.ModifiedBy = User.Identity.Name;
                    unit.DataStatus = EnumDataStatus.Updated.ToString();
                    _tLoanUnitRepository.Update(unit);
                }

                //save loanSurvey
                if (isSave)
                {
                    loanSurvey.LoanId = loan;
                    loanSurvey.SetAssignedIdTo(Guid.NewGuid().ToString());
                    loanSurvey.CreatedDate = DateTime.Now;
                    loanSurvey.CreatedBy = User.Identity.Name;
                    loanSurvey.DataStatus = EnumDataStatus.New.ToString();
                    _tLoanSurveyRepository.Save(loanSurvey);
                }

                _tLoanRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {
                _tLoanRepository.DbContext.RollbackTransaction();

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

        [Transaction]
        public ActionResult Survey(string loanSurveyId)
        {
            ViewData["CurrentItem"] = "Lembar Survey";
            SurveyFormViewModel viewModel =
                SurveyFormViewModel.CreateSurveyFormViewModel(_tLoanSurveyRepository, _mEmployeeRepository, _mZoneRepository, _mPartnerRepository, _mDepartmentRepository, loanSurveyId);

            return View(viewModel);
        }

        [Transaction]
        public ActionResult EditSurvey(string loanSurveyId)
        {
            ViewData["CurrentItem"] = "Lembar Survey";
            SurveyFormViewModel viewModel =
                SurveyFormViewModel.CreateSurveyFormViewModel(_tLoanSurveyRepository, _mEmployeeRepository, _mZoneRepository, _mPartnerRepository, _mDepartmentRepository, loanSurveyId);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Survey(TLoanSurvey surveyVM, TLoan loanVM, TLoanUnit loanUnitVM, FormCollection formCollection, string loanSurveyId)
        {
            try
            {
                _tLoanSurveyRepository.DbContext.BeginTransaction();

                TLoan loan = new TLoan();
                TLoanSurvey survey = new TLoanSurvey();
                TLoanUnit unit = new TLoanUnit();
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

                        if (loan.AddressId == null)
                        {
                            address.SetAssignedIdTo(Guid.NewGuid().ToString());
                            address.CreatedDate = DateTime.Now;
                            address.CreatedBy = User.Identity.Name;
                            address.DataStatus = EnumDataStatus.New.ToString();

                            _refAddressRepository.Save(address);
                        }
                        else
                        {
                            address = loan.AddressId;
                        }

                        if (loan.PersonId == null)
                        {
                            person.SetAssignedIdTo(Guid.NewGuid().ToString());
                            person.CreatedDate = DateTime.Now;
                            person.CreatedBy = User.Identity.Name;
                            person.DataStatus = EnumDataStatus.New.ToString();

                            _refPersonRepository.Save(person);
                        }
                        else
                        {
                            person = loan.PersonId;
                        }

                        if (loan.CustomerId == null)
                        {
                            customer.SetAssignedIdTo(Guid.NewGuid().ToString());
                            customer.CreatedDate = DateTime.Now;
                            customer.CreatedBy = User.Identity.Name;
                            customer.DataStatus = EnumDataStatus.New.ToString();

                            _mCustomerRepository.Save(customer);
                        }
                        else
                        {
                            customer = loan.CustomerId;
                        }
                        if (loan.LoanUnits.Count > 0)
                            unit = loan.LoanUnits[0];
                        else
                            unit = new TLoanUnit();

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
                loan.SalesmanId = loanVM.SalesmanId;
                loan.SurveyorId = loanVM.SurveyorId;
                loan.TLSId = loanVM.TLSId;
                loan.LoanCode = loanVM.LoanCode;
                loan.LoanCreditPrice = loanVM.LoanCreditPrice;
                loan.LoanDesc = loanVM.LoanDesc;
                loan.ZoneId = loanVM.ZoneId;
                loan.LoanSubmissionDate = Helper.CommonHelper.ConvertToDate(formCollection["LoanSubmissionDate"]);// loanVM.LoanSubmissionDate;
                loan.PartnerId = loanVM.PartnerId;

                loan.LoanDownPayment = Helper.CommonHelper.ConvertToDecimal(formCollection["LoanDownPayment"]);

                //loan.LoanDownPayment = loanVM.LoanDownPayment;
                loan.LoanIsSalesmanKnownCustomer = loanVM.LoanIsSalesmanKnownCustomer;
                loan.LoanTenor = loanVM.LoanTenor;
                loan.LoanNo = loanVM.LoanNo;

                loan.LoanUnitPriceTotal = Helper.CommonHelper.ConvertToDecimal(formCollection["LoanUnitPriceTotal"]);

                loan.LoanBasicInstallment = Helper.CommonHelper.ConvertToDecimal(formCollection["LoanBasicInstallment"]);

                loan.LoanMaturityDate = loanVM.LoanMaturityDate;

                loan.LoanSurveyDate = Helper.CommonHelper.ConvertToDate(formCollection["SurveyDate"]);// surveyVM.SurveyDate;

                if (isSave)
                {
                    loan.SetAssignedIdTo(Guid.NewGuid().ToString());
                    loan.CreatedDate = DateTime.Now;
                    loan.CreatedBy = User.Identity.Name;
                    loan.DataStatus = EnumDataStatus.New.ToString();
                    loan.LoanStatus = EnumLoanStatus.Survey.ToString();
                    _tLoanRepository.Save(loan);
                }
                else
                {
                    //update status to survey if edit did in loan status is new, 
                    //because survey has been save when insert new PK
                    if (loan.LoanStatus.Equals(EnumLoanStatus.Request.ToString()))
                        loan.LoanStatus = EnumLoanStatus.Survey.ToString();

                    loan.ModifiedDate = DateTime.Now;
                    loan.ModifiedBy = User.Identity.Name;
                    loan.DataStatus = EnumDataStatus.Updated.ToString();
                    _tLoanRepository.Update(loan);
                }

                //update installment list 
                _tInstallmentRepository.UpdateInstallmentByLoan(loan.Id, loan.LoanBasicInstallment.HasValue ? loan.LoanBasicInstallment.Value : 0, loan.LoanInterest.HasValue ? loan.LoanInterest.Value : 0, loan.LoanOtherInstallment.HasValue ? loan.LoanOtherInstallment.Value : 0);

                //save unit
                unit.LoanId = loan;
                unit.UnitType = loanUnitVM.UnitType;
                unit.UnitName = loanUnitVM.UnitName;
                unit.UnitPrice = Helper.CommonHelper.ConvertToDecimal(formCollection["UnitPrice"]);

                if (isSave)
                {
                    unit.SetAssignedIdTo(Guid.NewGuid().ToString());
                    unit.CreatedDate = DateTime.Now;
                    unit.CreatedBy = User.Identity.Name;
                    unit.DataStatus = EnumDataStatus.New.ToString();
                    _tLoanUnitRepository.Save(unit);
                }
                else
                {
                    unit.ModifiedDate = DateTime.Now;
                    unit.ModifiedBy = User.Identity.Name;
                    unit.DataStatus = EnumDataStatus.Updated.ToString();
                    _tLoanUnitRepository.Update(unit);
                }

                //save survey
                survey.LoanId = loan;
                survey.SurveyDate = Helper.CommonHelper.ConvertToDate(formCollection["SurveyDate"]);// surveyVM.SurveyDate;
                survey.SurveyDesc = surveyVM.SurveyDesc;
                survey.SurveyHouseType = surveyVM.SurveyHouseType;
                survey.SurveyNeighbor = surveyVM.SurveyNeighbor;
                survey.SurveyNeighborAsset = surveyVM.SurveyNeighborAsset;
                survey.SurveyNeighborCharacter = surveyVM.SurveyNeighborCharacter;
                survey.SurveyNeighborConclusion = surveyVM.SurveyNeighborConclusion;
                survey.SurveyStatus = EnumSurveyStatus.New.ToString();
                survey.SurveyUnitDeliverAddress = surveyVM.SurveyUnitDeliverAddress;
                survey.SurveyUnitDeliverDate = Helper.CommonHelper.ConvertToDate(formCollection["SurveyUnitDeliverDate"]);// surveyVM.SurveyUnitDeliverDate;
                survey.SurveyReceivedBy = surveyVM.SurveyReceivedBy;
                survey.SurveyProcessBy = surveyVM.SurveyProcessBy;
                survey.SurveyNeighborAsset = GetAsset(formCollection);
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

                //save installment
                if (loan.LoanStatus == "Approve")
                    SaveInstallment(loan);

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

        private string GetAsset(FormCollection formCollection)
        {
            StringBuilder result = new StringBuilder();

            //checkbox return string "true,false" if checked and return "false" if not checked

            //if (viewModel.SurveyNeighborAsset_CTV)
            if (formCollection["SurveyNeighborAsset_CTV"].Contains("true"))
                result.Append("CTV,");
            //if (viewModel.SurveyNeighborAsset_SF)
            if (formCollection["SurveyNeighborAsset_SF"].Contains("true"))
                result.Append("SF,");
            //if (viewModel.SurveyNeighborAsset_KG)
            if (formCollection["SurveyNeighborAsset_KG"].Contains("true"))
                result.Append("KG,");
            //if (viewModel.SurveyNeighborAsset_MC)
            if (formCollection["SurveyNeighborAsset_MC"].Contains("true"))
                result.Append("MC,");
            // if (viewModel.SurveyNeighborAsset_LH)
            if (formCollection["SurveyNeighborAsset_LH"].Contains("true"))
                result.Append("LH,");
            // if (viewModel.SurveyNeighborAsset_PC)
            if (formCollection["SurveyNeighborAsset_PC"].Contains("true"))
                result.Append("PC,");
            //if (viewModel.SurveyNeighborAsset_MTR)
            if (formCollection["SurveyNeighborAsset_MTR"].Contains("true"))
                result.Append("MTR,");
            //if (viewModel.SurveyNeighborAsset_MBL)
            if (formCollection["SurveyNeighborAsset_MBL"].Contains("true"))
                result.Append("MBL,");
            // if (viewModel.SurveyNeighborAsset_AC)
            if (formCollection["SurveyNeighborAsset_AC"].Contains("true"))
                result.Append("AC,");

            return result.ToString();
        }

        private void TransferFormValuesTo(RefPerson person, FormCollection formCollection)
        {
            person.PersonFirstName = formCollection["PersonFirstName"];
            person.PersonGender = formCollection["PersonGender"];
            person.PersonIdCardNo = formCollection["PersonIdCardNo"];

            person.PersonDob = Helper.CommonHelper.ConvertToDate(formCollection["PersonDob"]);

            person.PersonPob = formCollection["PersonPob"];
            person.PersonPhone = formCollection["PersonPhone"];
            person.PersonMobile = formCollection["PersonMobile"];
            person.PersonOccupation = formCollection["PersonOccupation"];
            person.PersonLastEducation = formCollection["PersonLastEducation"];
            person.PersonAge = Helper.CommonHelper.ConvertToDecimal(formCollection["PersonAge"]);
            person.PersonReligion = formCollection["PersonReligion"];

            person.PersonIncome = Helper.CommonHelper.ConvertToDecimal(formCollection["PersonIncome"]);

            person.PersonNoOfChildren = Helper.CommonHelper.ConvertToDecimal(formCollection["PersonNoOfChildren"]);

            person.PersonMarriedStatus = formCollection["PersonMarriedStatus"];
            person.PersonCoupleName = formCollection["PersonCoupleName"];
            person.PersonCoupleOccupation = formCollection["PersonCoupleOccupation"];

            person.PersonCoupleIncome = Helper.CommonHelper.ConvertToDecimal(formCollection["PersonCoupleIncome"]);

            person.PersonStaySince = Helper.CommonHelper.ConvertToDate(formCollection["PersonStaySince"]);

            person.PersonGuarantorName = formCollection["PersonGuarantorName"];
            person.PersonGuarantorRelationship = formCollection["PersonGuarantorRelationship"];
            person.PersonGuarantorOccupation = formCollection["PersonGuarantorOccupation"];
            person.PersonGuarantorPhone = formCollection["PersonGuarantorPhone"];

            person.PersonGuarantorStaySince = Helper.CommonHelper.ConvertToDate(formCollection["PersonGuarantorStaySince"]);

            person.PersonGuarantorHouseOwnerStatus = formCollection["PersonGuarantorHouseOwnerStatus"];
        }

        private void TransferFormValuesTo(RefAddress address, FormCollection formCollection)
        {
            address.AddressLine1 = formCollection["AddressLine1"];
            address.AddressLine2 = formCollection["AddressLine2"];
            address.AddressPostCode = formCollection["AddressPostCode"];
            address.AddressStatusOwner = formCollection["AddressStatusOwner"];

        }

        private void TransferFormValuesTo(TLoanSurvey loanSurvey, FormCollection formCollection)
        {
            loanSurvey.SurveyNeighbor = formCollection["SurveyNeighbor"];
            loanSurvey.SurveyNeighborCharacter = formCollection["SurveyNeighborCharacter"];
            loanSurvey.SurveyNeighborConclusion = formCollection["SurveyNeighborConclusion"];
            loanSurvey.SurveyNeighborAsset = formCollection["SurveyNeighborAsset"];
            loanSurvey.SurveyHouseType = formCollection["SurveyHouseType"];
            loanSurvey.SurveyUnitDeliverDate = Helper.CommonHelper.ConvertToDate(formCollection["SurveyUnitDeliverDate"]);
            loanSurvey.SurveyUnitDeliverAddress = formCollection["SurveyUnitDeliverAddress"];
        }

        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Approve(string loanId)
        {
            return ChangeLoanStatus(EnumLoanStatus.Approve, loanId, "Kredit Berhasil Disetujui");
        }

        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Oke(string loanId)
        {
            return ChangeLoanStatus(EnumLoanStatus.OK, loanId, "Kredit Ok");
        }

        private ActionResult ChangeLoanStatus(EnumLoanStatus enumLoanStatus, string loanId, string successMsg)
        {
            string Message = string.Empty;
            bool Success = true;
            try
            {
                _tLoanSurveyRepository.DbContext.BeginTransaction();
                TLoan loan = _tLoanRepository.Get(loanId);
                if (loan != null)
                {
                    //loan.LoanAccBy = User.Identity.Name;
                    loan.LoanAccDate = DateTime.Now;
                    loan.LoanStatus = enumLoanStatus.ToString();

                    loan.ModifiedBy = User.Identity.Name;
                    loan.ModifiedDate = DateTime.Now;
                    loan.DataStatus = EnumDataStatus.Updated.ToString();
                    _tLoanRepository.Update(loan);

                    if (enumLoanStatus == EnumLoanStatus.OK)
                    {
                        //save installment
                        SaveInstallment(loan);
                    }
                }

                _tLoanSurveyRepository.DbContext.CommitTransaction();
                Success = true;
                Message = successMsg;
            }
            catch (Exception ex)
            {
                Success = false;
                Message = "Error :\n" + ex.GetBaseException().Message;
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
                DateTime? firstDate = null;
                //if use DP, first installment's maturity date is when item is received
                if (loan.LoanDownPayment.HasValue)
                {
                    if (loan.LoanDownPayment.Value > 0)
                    {
                        if (loan.Surveys.Count > 0)
                        {
                            if (loan.Surveys[0].SurveyUnitDeliverDate.HasValue)
                            {
                                firstDate = loan.Surveys[0].SurveyUnitDeliverDate.Value;
                            }
                            else
                            {
                                throw new Exception("Tanggal Pengiriman Barang kosong, \nuntuk Kredit dengan DP, jatuh tempo angsuran pertama adalah tanggal pengiriman barang.");
                            }

                        }
                    }
                }

                if (!loan.LoanMaturityDate.HasValue)
                {
                    throw new Exception("Tanggal Jatuh tempo kredit kosong.\nTanggal jatuh tempo angsuran tidak bisa diinput.");
                }
                if (!loan.LoanAccDate.HasValue)
                {
                    throw new Exception("Tanggal kredit disetujui kosong.\nTanggal jatuh tempo angsuran tidak bisa diinput.");
                }

                //looping for each month
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
                    //if use DP, first installment's maturity date is when item is received
                    if (i == 0 && firstDate.HasValue)
                    {
                        ins.InstallmentMaturityDate = firstDate;
                    }
                    else
                        ins.InstallmentMaturityDate = startDate.AddMonths(i + 1);

                    ins.DataStatus = EnumDataStatus.New.ToString();
                    ins.CreatedBy = User.Identity.Name;
                    ins.CreatedDate = DateTime.Now;
                    _tInstallmentRepository.Save(ins);
                }
            }
        }

        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Cancel(string loanId)
        {
            return ChangeLoanStatus(EnumLoanStatus.Cancel, loanId, "Kredit Berhasil Dibatalkan");
        }

        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Postpone(string loanId)
        {
            return ChangeLoanStatus(EnumLoanStatus.Postpone, loanId, "Kredit Berhasil Ditunda");
        }

        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Reject(string loanId)
        {
            return ChangeLoanStatus(EnumLoanStatus.Reject, loanId, "Kredit Berhasil Ditolak");
        }

        [Transaction]
        public virtual ActionResult ListToday(string sidx, string sord, int page, int rows)
        {

            int totalRecords = 0;
            var loans = _tLoanRepository.GetPagedLoanListToday(sidx, sord, page, rows, ref totalRecords);

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
                                                loan.Id,
                                                loan.LoanNo,
                                                loan.PersonId.PersonName,
                                                loan.AddressId.AddressLine1,
                                                loan.LoanUnits[0].UnitType,
                                                Helper.CommonHelper.ConvertToString(loan.LoanBasicPrice),
                                                Helper.CommonHelper.ConvertToString(loan.LoanCreditPrice),
                                                Helper.CommonHelper.ConvertToString(loan.LoanTenor),
                                                loan.SalesmanId != null ? loan.SalesmanId.Id : null,
                                                loan.TLSId != null ? loan.TLSId.Id : null,
                                                loan.LoanStatus
                                                }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
