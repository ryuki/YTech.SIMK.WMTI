using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Transaction
{
    public class TLoanUnitMap : IAutoMappingOverride<TLoanUnit>
    {
        #region Implementation of IAutoMappingOverride<TLoanSurvey>
        
        public void Override(AutoMapping<TLoanUnit> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("T_LOAN_UNIT");
            mapping.Id(x => x.Id, "LOAN_UNIT_ID").GeneratedBy.Assigned();

            mapping.References(x => x.LoanId, "LOAN_ID");
            mapping.Map(x => x.UnitName, "UNIT_NAME");
            mapping.Map(x => x.UnitPrice, "UNIT_PRICE");
            mapping.Map(x => x.UnitType, "UNIT_TYPE");
            mapping.Map(x => x.UnitDesc, "UNIT_DESC");
            mapping.Map(x => x.UnitStatus, "UNIT_STATUS");

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
