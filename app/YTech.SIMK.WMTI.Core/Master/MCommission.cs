using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;

namespace YTech.SIMK.WMTI.Core.Master
{
    public class MCommission : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual string CommissionType { get; set; }
        public virtual int CommissionLevel { get; set; }
        public virtual decimal? CommissionLowTarget { get; set; }
        public virtual decimal? CommissionHighTarget { get; set; }
        public virtual DateTime? CommissionStartDate { get; set; }
        public virtual DateTime? CommissionEndDate { get; set; }
        public virtual decimal? CommissionValue { get; set; }
        public virtual string CommissionStatus { get; set; }
        public virtual string CommissionDesc { get; set; }

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
