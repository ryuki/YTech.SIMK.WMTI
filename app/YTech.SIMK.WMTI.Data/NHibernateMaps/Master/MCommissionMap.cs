using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Master
{
    public class MCommissionMap : IAutoMappingOverride<MCommission>
    {
        #region Implementation of IAutoMappingOverride<MZoneEmployee>

        public void Override(AutoMapping<MCommission> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("M_COMMISSION");
            mapping.Id(x => x.Id, "COMMISSION_ID").GeneratedBy.Assigned();

            mapping.Map(x => x.CommissionType, "COMMISSION_TYPE");
            mapping.Map(x => x.CommissionLevel, "COMMISSION_LEVEL");
            mapping.Map(x => x.CommissionLowTarget, "COMMISSION_LOW_TARGET");
            mapping.Map(x => x.CommissionHighTarget, "COMMISSION_HIGH_TARGET");
            mapping.Map(x => x.CommissionStartDate, "COMMISSION_START_DATE");
            mapping.Map(x => x.CommissionEndDate, "COMMISSION_END_DATE");
            mapping.Map(x => x.CommissionValue, "COMMISSION_VALUE");
            mapping.Map(x => x.CommissionStatus, "COMMISSION_STATUS");
            mapping.Map(x => x.CommissionDesc, "COMMISSION_DESC");

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
