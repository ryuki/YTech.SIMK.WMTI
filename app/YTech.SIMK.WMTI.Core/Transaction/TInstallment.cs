using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Core.Transaction
{
    public class TInstallment : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        public virtual TLoan LoanId { get; set; }
        public virtual MEmployee EmployeeId { get; set; }

        public virtual int? InstallmentNo { get; set; }
        public virtual decimal? InstallmentBasic { get; set; }
        public virtual decimal? InstallmentInterest { get; set; }
        public virtual decimal? InstallmentOthers { get; set; }
        public virtual DateTime? InstallmentPaymentDate { get; set; }
        public virtual DateTime? InstallmentMaturityDate { get; set; }
        public virtual decimal ? InstallmentFine { get; set; }
        public virtual string InstallmentDesc { get; set; }
        public virtual string InstallmentStatus { get; set; }

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
