using FluentNHibernate.Automapping;
using YTech.SIMK.WMTI.Core.Transaction;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Transaction
{
    public class TLoanFeedbackMap : IAutoMappingOverride<TLoanFeedback>
    {
        #region Implementation of IAutoMappingOverride<TLoan>

        public void Override(AutoMapping<TLoanFeedback> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("T_LOAN_FEEDBACK");
            mapping.Id(x => x.Id, "LOAN_FEEDBACK_ID").GeneratedBy.Assigned();

            mapping.References(x => x.LoanId, "LOAN_ID");

            mapping.Map(x => x.LoanFeedbackType, "LOAN_FEEDBACK_TYPE");
            mapping.Map(x => x.LoanFeedbackDesc, "LOAN_FEEDBACK_DESC");
            mapping.Map(x => x.LoanFeedbackBy, "LOAN_FEEDBACK_BY");
            mapping.Map(x => x.LoanFeedbackStatus, "LOAN_FEEDBACK_STATUS");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Version(x => x.RowVersion)
                .Column("ROW_VERSION")
                .CustomSqlType("Timestamp")
                .Not.Nullable()
                .Generated.Always();
        }

        #endregion
    }
}
