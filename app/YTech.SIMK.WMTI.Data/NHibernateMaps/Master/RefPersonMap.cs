using FluentNHibernate.Automapping;
using YTech.SIMK.WMTI.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Master
{
    public class RefPersonMap : IAutoMappingOverride<RefPerson>
    {
        #region Implementation of IAutoMappingOverride<RefPerson>

        public void Override(AutoMapping<RefPerson> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("REF_PERSON");
            mapping.Id(x => x.Id, "PERSON_ID").GeneratedBy.Assigned();

            mapping.Map(x => x.PersonFirstName, "PERSON_FIRST_NAME");
            mapping.Map(x => x.PersonLastName, "PERSON_LAST_NAME");
            mapping.Map(x => x.PersonNickName, "PERSON_NICK_NAME");
            mapping.Map(x => x.PersonDob, "PERSON_DOB");
            mapping.Map(x => x.PersonPob, "PERSON_POB");
            mapping.Map(x => x.PersonGender, "PERSON_GENDER");
            mapping.Map(x => x.PersonPhone, "PERSON_PHONE");
            mapping.Map(x => x.PersonMobile, "PERSON_MOBILE");
            mapping.Map(x => x.PersonEmail, "PERSON_EMAIL");
            mapping.Map(x => x.PersonReligion, "PERSON_RELIGION");
            mapping.Map(x => x.PersonRace, "PERSON_RACE");
            mapping.Map(x => x.PersonIdCardType, "PERSON_ID_CARD_TYPE");
            mapping.Map(x => x.PersonIdCardNo, "PERSON_ID_CARD_NO");
            mapping.Map(x => x.PersonLastEducation, "PERSON_LAST_EDU");
            mapping.Map(x => x.PersonAge, "PERSON_AGE");
            mapping.Map(x => x.PersonOccupation, "PERSON_OCCUPATION");
            mapping.Map(x => x.PersonOccupationSector, "PERSON_OCCUPATION_SECTOR");
            mapping.Map(x => x.PersonOccupationPosition, "PERSON_OCCUPATION_POSITION");
            mapping.Map(x => x.PersonIncome, "PERSON_INCOME");
            mapping.Map(x => x.PersonMarriedStatus, "PERSON_MARRIED_STATUS");
            mapping.Map(x => x.PersonHobby, "PERSON_HOBBY");
            mapping.Map(x => x.PersonNationality, "PERSON_NATIONALITY");
            mapping.Map(x => x.PersonBloodType, "PERSON_BLOOD_TYPE");
            mapping.Map(x => x.PersonNoOfChildren, "PERSON_NO_OF_CHILDREN");
            mapping.Map(x => x.PersonCoupleName, "PERSON_COUPLE_NAME");
            mapping.Map(x => x.PersonCoupleOccupation, "PERSON_COUPLE_OCCUPATION");
            mapping.Map(x => x.PersonCoupleIncome, "PERSON_COUPLE_INCOME");
            mapping.Map(x => x.PersonStaySince, "PERSON_STAY_SINCE");
            mapping.Map(x => x.PersonGuarantorName, "PERSON_GUARANTOR_NAME");
            mapping.Map(x => x.PersonGuarantorRelationship, "PERSON_GUARANTOR_RELATION");
            mapping.Map(x => x.PersonGuarantorOccupation, "PERSON_GUARANTOR_OCCUPATION");
            mapping.Map(x => x.PersonGuarantorPhone, "PERSON_GUARANTOR_PHONE");
            mapping.Map(x => x.PersonGuarantorHouseOwnerStatus, "PERSON_GUARANTOR_HOUSE_OWNER_STATUS");
            mapping.Map(x => x.PersonGuarantorStaySince, "PERSON_GUARANTOR_STAY_SINCE");
            mapping.Map(x => x.PersonDesc, "PERSON_DESC");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Map(x => x.RowVersion, "ROW_VERSION").ReadOnly();
        }

        #endregion
    }
}
