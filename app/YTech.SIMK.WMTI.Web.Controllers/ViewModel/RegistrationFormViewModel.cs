using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;
using YTech.SIMK.WMTI.Web.Controllers.Helper;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class RegistrationFormViewModel
    {
        public static RegistrationFormViewModel CreateRegistrationFormViewModel(IMCustomerRepository mCustomerRepository, string customerId)
        {
            RegistrationFormViewModel viewModel = new RegistrationFormViewModel();
            viewModel.CanEditId = true;


            MCustomer customer = null;
            RefPerson person = new RefPerson();
            RefAddress address = new RefAddress();
            if (!string.IsNullOrEmpty(customerId))
            {
                customer = mCustomerRepository.Get(customerId);
                person = customer.PersonId;
                address = customer.AddressId;
                viewModel.CanEditId = false;
            }
            if (customer == null)
            {
                customer = new MCustomer();

                customer.PersonId = person;

                customer.AddressId = address;
            }
            viewModel.Customer = customer;

            //fill gender
            var values = from EnumGender e in Enum.GetValues(typeof(EnumGender))
                         select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.GenderList = new SelectList(values, "Id", "Name", person.PersonGender);

            ////get letters to list
            //var letters = from EnumLetterTo e in Enum.GetValues(typeof(EnumLetterTo))
            //              select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            //viewModel.LetterList = new SelectList(letters, "Id", "Name");

            //get education list
            var edus = from EnumEducation e in Enum.GetValues(typeof(EnumEducation))
                       select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.EducationList = new SelectList(edus, "Id", "Name", person.PersonLastEducation);

            //get married status list
            var merrieds = from EnumMarriedStatus e in Enum.GetValues(typeof(EnumMarriedStatus))
                           select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.MarriedStatusList = new SelectList(merrieds, "Id", "Name", person.PersonMarriedStatus);

            //get card type list
            var idcards = from EnumIdCardType e in Enum.GetValues(typeof(EnumIdCardType))
                          select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.IdCardTypeList = new SelectList(idcards, "Id", "Name", person.PersonIdCardType);

            //get blood type list
            //var bloods = from EnumBloodType e in Enum.GetValues(typeof(EnumBloodType))
            //             select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            //viewModel.BloodTypeList = new SelectList(bloods, "Id", "Name", person.PersonBloodType);

            //get religions list
            var religions = from EnumReligion e in Enum.GetValues(typeof(EnumReligion))
                            select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            viewModel.ReligionList = new SelectList(religions, "Id", "Name", person.PersonReligion);

            //get occupations list
            //var occupations = from EnumOccupation e in Enum.GetValues(typeof(EnumOccupation))
            //                  select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            //viewModel.OccupationList = new SelectList(occupations, "Id", "Name", person.PersonOccupation);

            //get hobbys list
            //var hobbys = from EnumHobby e in Enum.GetValues(typeof(EnumHobby))
            //             select new { ID = e, Name = CommonHelper.GetStringValue(e) };
            //viewModel.HobbyList = new SelectList(hobbys, "Id", "Name", person.PersonHobby);
            return viewModel;
        }

        public MCustomer Customer { get; internal set; }
        public TLoanSurvey LoanSurvey { get; internal set; }
        public SelectList GenderList { get; internal set; }
        public SelectList LetterList { get; internal set; }
        public SelectList EducationList { get; internal set; }
        public SelectList MarriedStatusList { get; internal set; }
        public SelectList IdCardTypeList { get; internal set; }
        //public SelectList BloodTypeList { get; internal set; }
        public SelectList ReligionList { get; internal set; }
        //public SelectList OccupationList { get; internal set; }
        //public SelectList HobbyList { get; internal set; }
        public bool CanEditId { get; internal set; }


    }
}
