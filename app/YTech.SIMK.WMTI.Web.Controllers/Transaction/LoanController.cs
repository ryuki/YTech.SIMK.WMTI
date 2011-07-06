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

        public LoanController(ITLoanRepository tLoanRepository, ITLoanSurveyRepository tLoanSurveyRepository, IMCustomerRepository mCustomerRepository, IRefAddressRepository refAddressRepository, IRefPersonRepository refPersonRepository, ITInstallmentRepository tInstallmentRepository, IMEmployeeRepository mEmployeeRepository, ITLoanUnitRepository tLoanUnitRepository, IMZoneRepository mZoneRepository)
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

            _tLoanRepository = tLoanRepository;
            _tLoanSurveyRepository = tLoanSurveyRepository;
            _mCustomerRepository = mCustomerRepository;
            _refAddressRepository = refAddressRepository;
            _refPersonRepository = refPersonRepository;
            _tInstallmentRepository = tInstallmentRepository;
            _mEmployeeRepository = mEmployeeRepository;
            _tLoanUnitRepository = tLoanUnitRepository;
            _mZoneRepository = mZoneRepository;
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
                                                loan.LoanSubmissionDate.HasValue ? loan.LoanSubmissionDate.Value.ToString(Helper.CommonHelper.DateFormat) : null,
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
        public ActionResult CustomerRequest(TLoan loanVM, TLoanUnit loanUnitVM, FormCollection formCollection, string loanCustomerRequestId)
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
                    if (loan != null )
                    {
                        isSave = false;
                        address = loan.AddressId;
                        person = loan.PersonId;
                        customer = loan.CustomerId;
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

                loan.TLSId = loanVM.TLSId;
                loan.SalesmanId = loanVM.SalesmanId;
                loan.SurveyorId = loanVM.SurveyorId;

                loan.LoanNo = loanVM.LoanNo;
                loan.LoanCode = loanVM.LoanCode;
                loan.LoanBasicPrice = loanVM.LoanBasicPrice;
                loan.LoanCreditPrice = loanVM.LoanCreditPrice;
                loan.LoanSubmissionDate = loanVM.LoanSubmissionDate;
                loan.LoanAdminFee = loanVM.LoanAdminFee;
                loan.LoanMateraiFee = loanVM.LoanMateraiFee;
                loan.LoanTenor = loanVM.LoanTenor;

                if (!string.IsNullOrEmpty(formCollection["LoanDownPayment"]))
                    loan.LoanDownPayment = Convert.ToDecimal(formCollection["LoanDownPayment"].Replace(",", ""));
                else
                    loan.LoanDownPayment = null;

                if (!string.IsNullOrEmpty(formCollection["LoanBasicInstallment"]))
                    loan.LoanBasicInstallment = Convert.ToDecimal(formCollection["LoanBasicInstallment"].Replace(",", ""));
                else
                    loan.LoanBasicInstallment = null;

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
                loanSurvey.LoanId = loan;

                if (isSave)
                {
                    loanSurvey.SetAssignedIdTo(Guid.NewGuid().ToString());
                    loanSurvey.CreatedDate = DateTime.Now;
                    loanSurvey.CreatedBy = User.Identity.Name;
                    loanSurvey.DataStatus = EnumDataStatus.New.ToString();
                    _tLoanSurveyRepository.Save(loanSurvey);
                }
                else
                {
                    loanSurvey.ModifiedDate = DateTime.Now;
                    loanSurvey.ModifiedBy = User.Identity.Name;
                    loanSurvey.DataStatus = EnumDataStatus.Updated.ToString();
                    _tLoanSurveyRepository.Update(loanSurvey);
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
                SurveyFormViewModel.CreateSurveyFormViewModel(_tLoanSurveyRepository, _mEmployeeRepository, _mZoneRepository, loanSurveyId);

            return View(viewModel);
        }

        [Transaction]
        public ActionResult EditSurvey(string loanSurveyId)
        {
            ViewData["CurrentItem"] = "Lembar Survey";
            SurveyFormViewModel viewModel =
                SurveyFormViewModel.CreateSurveyFormViewModel(_tLoanSurveyRepository, _mEmployeeRepository, _mZoneRepository, loanSurveyId);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Survey(TLoanSurvey surveyVM, TLoan loanVM, TLoanUnit loanUnitVM,  FormCollection formCollection, string loanSurveyId)
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
                        address = loan.AddressId;
                        person = loan.PersonId;
                        customer = loan.CustomerId;
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
                loan.LoanSubmissionDate = loanVM.LoanSubmissionDate;

                if (!string.IsNullOrEmpty(formCollection["LoanDownPayment"]))
                    loan.LoanDownPayment = Convert.ToDecimal(formCollection["LoanDownPayment"].Replace(",", ""));
                else
                    loan.LoanDownPayment = null;

                //loan.LoanDownPayment = loanVM.LoanDownPayment;
                loan.LoanIsSalesmanKnownCustomer = loanVM.LoanIsSalesmanKnownCustomer;
                loan.LoanTenor = loanVM.LoanTenor;
                loan.LoanNo = loanVM.LoanNo;

                if (!string.IsNullOrEmpty(formCollection["LoanUnitPriceTotal"]))
                    loan.LoanUnitPriceTotal = Convert.ToDecimal(formCollection["LoanUnitPriceTotal"].Replace(",", ""));
                else
                    loan.LoanUnitPriceTotal = null;

                if (!string.IsNullOrEmpty(formCollection["LoanBasicInstallment"]))
                    loan.LoanBasicInstallment = Convert.ToDecimal(formCollection["LoanBasicInstallment"].Replace(",", ""));
                else
                    loan.LoanBasicInstallment = null;

                loan.LoanMaturityDate = loanVM.LoanMaturityDate;

                loan.LoanSurveyDate = surveyVM.SurveyDate;

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
                if (!string.IsNullOrEmpty(formCollection["UnitPrice"]))
                    unit.UnitPrice = Convert.ToDecimal(formCollection["UnitPrice"].Replace(",", ""));
                else
                    unit.UnitPrice = null;
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
                person.PersonIncome = Convert.ToDecimal(formCollection["PersonIncome"].Replace(",", ""));
            else
                person.PersonIncome = null;

            if (!string.IsNullOrEmpty(formCollection["PersonNoOfChildren"]))
                person.PersonNoOfChildren = Convert.ToDecimal(formCollection["PersonNoOfChildren"].Replace(",", ""));
            else
                person.PersonNoOfChildren = null;

            person.PersonMarriedStatus = formCollection["PersonMarriedStatus"];
            person.PersonCoupleName = formCollection["PersonCoupleName"];
            person.PersonCoupleOccupation = formCollection["PersonCoupleOccupation"];

            if (!string.IsNullOrEmpty(formCollection["PersonCoupleIncome"]))
                person.PersonCoupleIncome = Convert.ToDecimal(formCollection["PersonCoupleIncome"].Replace(",", ""));
            else
                person.PersonCoupleIncome = null;

            if (!string.IsNullOrEmpty(formCollection["PersonStaySince"]))
                person.PersonStaySince = Convert.ToDateTime(formCollection["PersonStaySince"]);
            else
                person.PersonStaySince = null;

            person.PersonGuarantorName = formCollection["PersonGuarantorName"];
            person.PersonGuarantorRelationship = formCollection["PersonGuarantorRelationship"];
            person.PersonGuarantorOccupation = formCollection["PersonGuarantorOccupation"];
            person.PersonGuarantorPhone = formCollection["PersonGuarantorPhone"];

            if (!string.IsNullOrEmpty(formCollection["PersonGuarantorStaySince"]))
                person.PersonGuarantorStaySince = Convert.ToDateTime(formCollection["PersonGuarantorStaySince"]);
            else
                person.PersonGuarantorStaySince = null;

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
            if (!string.IsNullOrEmpty(formCollection["PersonGuarantorStaySince"]))
                loanSurvey.SurveyUnitDeliverDate = Convert.ToDateTime(formCollection["SurveyUnitDeliverDate"]);
            else
                loanSurvey.SurveyUnitDeliverDate = null;
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
