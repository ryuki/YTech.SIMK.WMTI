using System;
using System.Linq;
using System.Web.Mvc;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;
using YTech.SIMK.WMTI.Web.Controllers.Helper;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class SurveyFormViewModel
    {
        public static SurveyFormViewModel CreateSurveyFormViewModel(ITLoanSurveyRepository tLoanSurveyRepository, IMEmployeeRepository mEmployeeRepository,IMZoneRepository mZoneRepository, string loanSurveyId)
        {
            SurveyFormViewModel viewModel = new SurveyFormViewModel();
            viewModel.CanEditId = true;

            TLoanSurvey loanSurvey = null;
            TLoanUnit loanUnit = null;
            RefPerson person = new RefPerson();
            RefAddress address = new RefAddress();
            if (!string.IsNullOrEmpty(loanSurveyId))
            {
                loanSurvey = tLoanSurveyRepository.Get(loanSurveyId);
                person = loanSurvey.LoanId.PersonId;
                address = loanSurvey.LoanId.AddressId;
                if (loanSurvey.LoanId.LoanUnits.Count > 0)
                    loanUnit = loanSurvey.LoanId.LoanUnits[0];
                else
                    loanUnit = new TLoanUnit();
                viewModel.CanEditId = false;
            }
            if (loanSurvey == null)
            {
                TLoan loan = new TLoan();
                loanUnit = new TLoanUnit();
                MCustomer customer = new MCustomer();
                loanSurvey = new TLoanSurvey();
                MZone zone = new MZone();

                loanSurvey.LoanId = loan;
                loanSurvey.LoanId.CustomerId = customer;
                loanSurvey.LoanId.CustomerId.PersonId = person;
                loanSurvey.LoanId.CustomerId.AddressId = address;
                loanSurvey.LoanId.ZoneId = zone;

                MEmployee emp = new MEmployee();
                loan.SalesmanId = emp;
                loan.SurveyorId = emp;
                loan.CollectorId = emp;
                loan.TLSId = emp;
            }
            viewModel.LoanSurvey = loanSurvey;
            viewModel.LoanUnit = loanUnit;

            //fill gender
            var values = from EnumGender e in Enum.GetValues(typeof(EnumGender))
                         select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.GenderList = new SelectList(values, "Id", "Name", person.PersonGender);

            //get education list
            var edus = from EnumEducation e in Enum.GetValues(typeof(EnumEducation))
                       select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.EducationList = new SelectList(edus, "Id", "Name", person.PersonLastEducation);

            //get married status list
            var merrieds = from EnumMarriedStatus e in Enum.GetValues(typeof(EnumMarriedStatus))
                           select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.MarriedStatusList = new SelectList(merrieds, "Id", "Name", person.PersonMarriedStatus);

            //get religions list
            var religions = from EnumReligion e in Enum.GetValues(typeof(EnumReligion))
                            select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.ReligionList = new SelectList(religions, "Id", "Name", person.PersonReligion);

            //get house owner status list
            var houses = from EnumHouseOwner e in Enum.GetValues(typeof(EnumHouseOwner))
                         select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.HouseOwnerList = new SelectList(houses, "Id", "Name", address.AddressStatusOwner);
            viewModel.GuarantorHouseOwnerList = new SelectList(houses, "Id", "Name", person.PersonGuarantorHouseOwnerStatus);

            //get neighbor character list
            var neighbors = from EnumNeighborChar e in Enum.GetValues(typeof(EnumNeighborChar))
                            select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.NeighborCharList = new SelectList(neighbors, "Id", "Name", loanSurvey.SurveyNeighbor);

            //get house type list
            var housetypes = from EnumHouseType e in Enum.GetValues(typeof(EnumHouseType))
                             select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.HouseTypeList = new SelectList(housetypes, "Id", "Name", loanSurvey.SurveyHouseType);

            //get known customer list
            var knowcustomers = from EnumKnowCustomer e in Enum.GetValues(typeof(EnumKnowCustomer))
                                select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.KnowCustomerList = new SelectList(knowcustomers, "Id", "Name", loanSurvey.LoanId.LoanIsSalesmanKnownCustomer);

            var listEmployee = mEmployeeRepository.GetAll();
            MEmployee employee = new MEmployee();
            //mCustomer.SupplierName = "-Pilih Supplier-";
            listEmployee.Insert(0, employee);
            var salesman = from emp in listEmployee
                           //where emp.DepartmentId.DepartmentName == "SALESMAN"
                           select new { Id = emp.Id, Name = emp.PersonId != null ? emp.PersonId.PersonName : "-Pilih Salesman-" };
            viewModel.SalesmanList = new SelectList(salesman, "Id", "Name", loanSurvey.LoanId.SalesmanId != null ? loanSurvey.LoanId.SalesmanId.Id : string.Empty);

            var surveyor = from emp in listEmployee
                           //where emp.DepartmentId.DepartmentName == "SURVEYOR"
                           select new { Id = emp.Id, Name = emp.PersonId != null ? emp.PersonId.PersonName : "-Pilih Surveyor-" };
            viewModel.SurveyorList = new SelectList(surveyor, "Id", "Name", loanSurvey.LoanId.SurveyorId != null ? loanSurvey.LoanId.SurveyorId.Id : string.Empty);

            var collector = from emp in listEmployee
                            //where emp.DepartmentId.DepartmentName == "COLLECTOR"
                            select new { Id = emp.Id, Name = emp.PersonId != null ? emp.PersonId.PersonName : "-Pilih Kolektor-" };
            viewModel.CollectorList = new SelectList(collector, "Id", "Name", loanSurvey.LoanId.CollectorId != null ? loanSurvey.LoanId.CollectorId.Id : string.Empty);

            var tls = from emp in listEmployee
                      //where emp.DepartmentId.DepartmentName == "TEAM LEADER SALESMAN"
                      select new { Id = emp.Id, Name = emp.PersonId != null ? emp.PersonId.PersonName : "-Pilih Team Leader-" };
            viewModel.TLSList = new SelectList(tls, "Id", "Name", loanSurvey.LoanId.TLSId != null ? loanSurvey.LoanId.TLSId.Id : string.Empty);

            var listZone = mZoneRepository.GetAll();
            MZone z = new MZone();
            z.ZoneName = "-Pilih Wilayah-";
            listZone.Insert(0, z);
            var zones = from zo in listZone
                        select new { Id = zo.Id, Name = zo.ZoneName };
            viewModel.ZoneList = new SelectList(zones, "Id", "Name", loanSurvey.LoanId.ZoneId != null ? loanSurvey.LoanId.ZoneId.Id : string.Empty);

            return viewModel;
        }

        public TLoanSurvey LoanSurvey { get; internal set; }
        public TLoanUnit LoanUnit { get; internal set; }
        public SelectList GenderList { get; internal set; }
        public SelectList LetterList { get; internal set; }
        public SelectList EducationList { get; internal set; }
        public SelectList MarriedStatusList { get; internal set; }
        public SelectList ReligionList { get; internal set; }
        public SelectList HouseOwnerList { get; internal set; }
        public SelectList GuarantorHouseOwnerList { get; internal set; }
        public SelectList NeighborCharList { get; internal set; }
        public SelectList HouseTypeList { get; internal set; }
        public SelectList KnowCustomerList { get; internal set; }
        public SelectList ZoneList { get; internal set; }
        public SelectList SalesmanList { get; internal set; }
        public SelectList TLSList { get; internal set; }
        public SelectList SurveyorList { get; internal set; }
        public SelectList CollectorList { get; internal set; }
        public bool CanEditId { get; internal set; }

    }
}
