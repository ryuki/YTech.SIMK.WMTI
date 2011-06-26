using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Transaction
{
    public class TSettleMap : IAutoMappingOverride<TSettle>
    {
        #region Implementation of IAutoMappingOverride<TSettle>

        public void Override(AutoMapping<TSettle> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("T_SETTLE");
            mapping.Id(x => x.Id, "SETTLE_ID").GeneratedBy.Assigned();
            mapping.References(x => x.LoanId, "LOAN_ID").Fetch.Join();

            mapping.Map(x => x.SettleDate, "SETTLE_DATE");
            mapping.Map(x => x.SettleRemainInstallment, "SETTLE_REMAIN_INSTALLMENT");
            mapping.Map(x => x.SettlePenalty, "SETTLE_PENALTY");
            mapping.Map(x => x.SettleRemainMustPaid, "SETTLE_REMAIN_MUST_PAID");
            mapping.Map(x => x.SettleStatus, "SETTLE_STATUS");
            mapping.Map(x => x.SettleDesc, "SETTLE_DESC");

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
