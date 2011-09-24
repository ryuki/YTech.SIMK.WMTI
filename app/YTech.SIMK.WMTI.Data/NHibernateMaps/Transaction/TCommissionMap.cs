using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Transaction
{
    public class TCommissionMap : IAutoMappingOverride<TCommission>
    {
        #region Implementation of IAutoMappingOverride<TCommission>

        public void Override(AutoMapping<TCommission> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("T_COMMISSION");
            mapping.Id(x => x.Id, "COMMISSION_ID").GeneratedBy.Assigned();

            mapping.References(x => x.EmployeeId, "EMPLOYEE_ID");
            mapping.Map(x => x.CommissionType, "COMMISSION_TYPE");
            mapping.Map(x => x.CommissionLevel, "COMMISSION_LEVEL");
            mapping.Map(x => x.CommissionStartDate, "COMMISSION_START_DATE");
            mapping.Map(x => x.CommissionEndDate, "COMMISSION_END_DATE");
            mapping.Map(x => x.CommissionValue, "COMMISSION_VALUE");
            mapping.Map(x => x.CommissionStatus, "COMMISSION_STATUS");
            mapping.Map(x => x.CommissionDesc, "COMMISSION_DESC");
            mapping.References(x => x.RecPeriodId, "REC_PERIOD_ID");
            mapping.Map(x => x.CommissionFactor, "COMMISSION_FACTOR");

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
