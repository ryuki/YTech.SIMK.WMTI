using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using SharpArch.Core.DomainModel;
using System;
using SharpArch.Core;

namespace YTech.SIMK.WMTI.Core.Transaction
{
    public class TLoanSurvey : EntityWithTypedId<string>, IHasAssignedId<string>
    {
        [DomainSignature]
        [NotNull, NotEmpty]
        public virtual TLoan LoanId { get; set; }
        public virtual DateTime? SurveyDate { get; set; }
        public virtual string SurveyNeighbor { get; set; }
        public virtual string SurveyNeighborCharacter { get; set; }
        public virtual string SurveyNeighborAsset { get; set; }
        public virtual string SurveyNeighborConclusion { get; set; }
        public virtual string SurveyHouseType { get; set; }
        public virtual DateTime? SurveyUnitDeliverDate { get; set; }
        public virtual string SurveyUnitDeliverAddress { get; set; }
        public virtual string SurveyDesc { get; set; }
        public virtual string SurveyStatus { get; set; }

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
