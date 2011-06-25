using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Core.Transaction
{
    public class TLoan : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual MZone ZoneId { get; set; }
        public virtual MPartner PartnerId { get; set; }
        public virtual MCustomer CustomerId { get; set; }
        public virtual RefPerson PersonId { get; set; }
        public virtual RefAddress AddressId { get; set; }
        
        public virtual MEmployee TLSId { get; set; }
        public virtual MEmployee SalesmanId { get; set; }
        public virtual MEmployee SurveyorId { get; set; }
        public virtual MEmployee CollectorId { get; set; }
            
        public virtual string LoanNo { get; set; }
        public virtual string LoanCode { get; set; }
        public virtual DateTime? LoanSubmissionDate { get; set; }
        public virtual DateTime? LoanSurveyDate { get; set; }
        public virtual decimal? LoanUnitPriceTotal { get; set; }
        public virtual decimal? LoanDownPayment { get; set; }
        public virtual decimal? LoanBasicPrice { get; set; }
        public virtual decimal? LoanCreditPrice { get; set; }
        public virtual decimal? LoanFactorMultiple { get; set; }
        public virtual decimal? LoanFactorAdd { get; set; }
        public virtual decimal? LoanInterest { get; set; }
        public virtual int? LoanTenor { get; set; }
        public virtual decimal? LoanBasicInstallment { get; set; }
        public virtual decimal? LoanInterestInstallment { get; set; }
        public virtual decimal? LoanOtherInstallment { get; set; }
            
        public virtual MEmployee LoanAccBy { get; set; }
        public virtual DateTime? LoanAccDate { get; set; }
            
        public virtual decimal? LoanAdminFee { get; set; }
        public virtual decimal? LoanMateraiFee { get; set; }
        public virtual int? LoanMaturityDate { get; set; }
        public virtual string LoanIsSalesmanKnownCustomer { get; set; }

        public virtual string LoanDesc { get; set; }
        public virtual string LoanStatus { get; set; }

        public virtual IList<TLoanSurvey> Surveys { get; set; }

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
