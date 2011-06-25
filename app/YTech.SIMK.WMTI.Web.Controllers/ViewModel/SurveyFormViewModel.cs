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
        public static SurveyFormViewModel CreateSurveyFormViewModel(ITLoanSurveyRepository tLoanSurveyRepository, string loanSurveyId)
        {
            SurveyFormViewModel viewModel = new SurveyFormViewModel();
            viewModel.CanEditId = true;

            TLoanSurvey loanSurvey = null;
            RefPerson person = new RefPerson();
            RefAddress address = new RefAddress();
            if (!string.IsNullOrEmpty(loanSurveyId))
            {
                loanSurvey = tLoanSurveyRepository.Get(loanSurveyId);
                person = loanSurvey.LoanId.CustomerId.PersonId;
                address = loanSurvey.LoanId.CustomerId.AddressId;
                viewModel.CanEditId = false;
            }
            if (loanSurvey == null)
            {
                TLoan loan = new TLoan();
                //MEmployee employee = new MEmployee();
                MCustomer customer = new MCustomer();
                loanSurvey = new TLoanSurvey();
                MZone zone = new MZone();

                loanSurvey.LoanId = loan;
                //loanSurvey.LoanId.SalesmanId = employee;
                loanSurvey.LoanId.CustomerId = customer;
                loanSurvey.LoanId.CustomerId.PersonId = person;
                loanSurvey.LoanId.CustomerId.AddressId = address;
                loanSurvey.LoanId.ZoneId = zone;
            }
            viewModel.LoanSurvey = loanSurvey;

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
            var houses = from EnumHouseOwner e in Enum.GetValues(typeof (EnumHouseOwner))
                         select new {ID = e, Name = CommonHelper.GetStringValue(e)};
            viewModel.HouseOwnerList = new SelectList(houses, "Id", "Name", address.AddressStatusOwner);

            //get neighbor character list
            var neighbors = from EnumNeighborChar e in Enum.GetValues(typeof (EnumNeighborChar))
                           select new {ID = e, Name = CommonHelper.GetStringValue(e)};
            viewModel.NeighborCharList = new SelectList(neighbors, "Id", "Name", loanSurvey.SurveyNeighbor);

            //get house type list
            var housetypes = from EnumHouseType e in Enum.GetValues(typeof (EnumHouseType))
                             select new {ID = e, Name = CommonHelper.GetStringValue(e)};
            viewModel.HouseTypeList = new SelectList(housetypes, "Id", "Name", loanSurvey.SurveyHouseType);

            //get known customer list
            var knowcustomers = from EnumKnowCustomer e in Enum.GetValues(typeof (EnumKnowCustomer))
                                select new {ID = e, Name = CommonHelper.GetStringValue(e)};
            viewModel.KnowCustomerList = new SelectList(knowcustomers, "Id", "Name", loanSurvey.LoanId.LoanIsSalesmanKnownCustomer);

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
        public SelectList NeighborCharList { get; internal set; }
        public SelectList HouseTypeList { get; internal set; }
        public SelectList KnowCustomerList { get; internal set; }
        public bool CanEditId { get; internal set; }

    }
}
