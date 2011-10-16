using System;
using NHibernate.Validator.Constraints;
using SharpArch.Core;
using SharpArch.Core.DomainModel;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Core.Transaction
{
    public class TPrivilege : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual string PrivilegeType { get; set; }
        public virtual string UserName { get; set; }
        public virtual MMenu MenuId { get; set; }
        public virtual bool? PrivilegeStatus { get; set; }
        public virtual string PrivilegeDesc { get; set; }

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
