using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;

namespace YTech.SIMK.WMTI.Core.Master
{
    public class RefPerson : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual string PersonFirstName { get; set; }
        public virtual string PersonLastName { get; set; }
        public virtual string PersonNickName { get; set; }
        public virtual DateTime? PersonDob { get; set; }
        public virtual string PersonPob { get; set; }
        public virtual string PersonGender { get; set; }
        public virtual string PersonPhone { get; set; }
        public virtual string PersonMobile { get; set; }
        public virtual string PersonEmail { get; set; }
        public virtual string PersonReligion { get; set; }
        public virtual string PersonRace { get; set; }
        public virtual string PersonIdCardType { get; set; }
        public virtual string PersonIdCardNo { get; set; }
        public virtual string PersonLastEducation { get; set; }
        public virtual decimal? PersonAge { get; set; }
        public virtual string PersonOccupation { get; set; }
        public virtual string PersonOccupationSector { get; set; }
        public virtual string PersonOccupationPosition { get; set; }
        public virtual decimal? PersonIncome { get; set; }
        public virtual string PersonMarriedStatus { get; set; }
        public virtual string PersonHobby { get; set; }
        public virtual string PersonNationality { get; set; }
        public virtual string PersonBloodType { get; set; }
        public virtual decimal? PersonNoOfChildren { get; set; }
        public virtual string PersonCoupleName { get; set; }
        public virtual string PersonCoupleOccupation { get; set; }
        public virtual decimal? PersonCoupleIncome { get; set; }
        public virtual DateTime? PersonStaySince { get; set; }
        public virtual string PersonGuarantorName { get; set; }
        public virtual string PersonGuarantorOccupation { get; set; }
        public virtual string PersonGuarantorPhone { get; set; }
        public virtual string PersonGuarantorHouseOwnerStatus { get; set; }
        public virtual DateTime? PersonGuarantorStaySince { get; set; }
        public virtual string PersonDesc { get; set; }

        public virtual string DataStatus { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual byte[] RowVersion { get; set; }

        #region Implementation of IHasAssignedId<string>

        public virtual void SetAssignedIdTo(string assignedId)
        {
            Check.Require(!string.IsNullOrEmpty(assignedId), "Assigned Id may not be null or empty");
            Id = assignedId.Trim();
        }

        #endregion
    }
}
