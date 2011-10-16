using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core;
using SharpArch.Core.DomainModel;

namespace YTech.SIMK.WMTI.Core.Master
{
    public class MMenu : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual string MenuName { get; set; }
        public virtual string MenuType { get; set; }
        public virtual string MenuIcon { get; set; }
        public virtual string MenuLink { get; set; }
        public virtual MMenu MenuParent { get; set; }
        public virtual string MenuStatus { get; set; }
        public virtual string MenuDesc { get; set; }

        public virtual IList<MMenu> MenuChildren { get; set; }

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
