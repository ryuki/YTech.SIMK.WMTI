using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Core.Transaction
{
    public class TInstallment : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual int? InstallmentNo { get; set; }
        public virtual TLoan LoanId { get; set; }
        public virtual MEmployee EmployeeId { get; set; }

        public virtual decimal? InstallmentBasic { get; set; }
        public virtual decimal? InstallmentInterest { get; set; }
        public virtual decimal? InstallmentOthers { get; set; }
        public virtual DateTime? InstallmentPaymentDate { get; set; }
        public virtual DateTime? InstallmentMaturityDate { get; set; }
        public virtual decimal? InstallmentFine { get; set; }
        public virtual string InstallmentDesc { get; set; }
        public virtual string InstallmentStatus { get; set; }
        public virtual decimal? InstallmentPaid { get; set; }
        public virtual string InstallmentReceiptNo { get; set; }

        public virtual decimal InstallmentTotal
        {
            get
            {
                decimal basic = InstallmentBasic.HasValue ? InstallmentBasic.Value : 0;
                decimal interest = InstallmentInterest.HasValue ? InstallmentInterest.Value : 0;
                decimal other = InstallmentOthers.HasValue ? InstallmentOthers.Value : 0;
                return basic + interest + other;
            }
        }

        public virtual decimal InstallmentMustPaid
        {
            get
            {
                decimal total = InstallmentTotal;
                decimal fine = InstallmentFine.HasValue ? InstallmentFine.Value : 0;
                return total + fine;
            }
        }

        public virtual decimal? InstallmentSisa
        {
            get
            {
                if (InstallmentStatus.Equals(EnumInstallmentStatus.Paid.ToString()))
                {
                    decimal mustpaid = InstallmentMustPaid;
                    decimal paid = InstallmentPaid.HasValue ? InstallmentPaid.Value : 0;
                    return paid - mustpaid;
                }
                else
                {
                    return null;
                }
            }
        }

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
