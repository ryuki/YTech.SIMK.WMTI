using System;
using NHibernate.Validator.Constraints;
using SharpArch.Core;
using SharpArch.Core.DomainModel;

namespace YTech.SIMK.WMTI.Core.Transaction
{
    public class TNews : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual DateTime? NewsTitle { get; set; }
        public virtual decimal? NewsDesc { get; set; }
        public virtual decimal? NewsStatus { get; set; }
        public virtual decimal? NewsStartDate { get; set; }
        public virtual string NewsEndDate { get; set; }
        public virtual string NewsTo { get; set; }

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
