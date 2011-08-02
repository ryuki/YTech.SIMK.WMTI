using System;
using SharpArch.Core;
using SharpArch.Core.DomainModel;
using NHibernate.Validator.Constraints;

namespace YTech.SIMK.WMTI.Core.Master
{
    public class MCommissionDet : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull,NotEmpty]
        public virtual MCommission CommissionId { get; set; }

        public virtual string DetailType { get; set; }
        public virtual decimal? DetailValue { get; set; }
        public virtual string DetailStatus { get; set; }
        public virtual string DetailDesc { get; set; }

        public virtual string DataStatus { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual byte[] RowVersion { get; set; }

        public virtual int DetailLowTarget { get; set; }
        public virtual int DetailHighTarget { get; set; }

        #region Implementation of IHasAssignedId<string>

        public virtual void SetAssignedIdTo(string assignedId)
        {
            Check.Require(!string.IsNullOrEmpty(assignedId), "Assigned Id may not be null or empty");
            Id = assignedId.Trim();
        }

        #endregion
    }
}
