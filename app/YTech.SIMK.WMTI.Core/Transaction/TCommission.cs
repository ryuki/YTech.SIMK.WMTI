using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Core.Transaction
{
    public class TCommission : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual MEmployee EmployeeId { get; set; }

        public virtual string CommissionType { get; set; }
        public virtual int? CommissionLevel { get; set; }
        public virtual decimal? CommissionValue { get; set; }
        public virtual DateTime? CommissionStartDate { get; set; }
        public virtual DateTime? CommissionEndDate { get; set; }
        public virtual string CommissionDesc { get; set; }
        public virtual string CommissionStatus { get; set; }
        public virtual TRecPeriod RecPeriodId { get; set; }
        public virtual decimal? CommissionFactor { get; set; }

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
