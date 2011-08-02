using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Master
{
    public class MCommissionDetMap : IAutoMappingOverride<MCommissionDet>
    {
        #region Implementation of IAutoMappingOverride<MCommissionDet>

        public void Override(AutoMapping<MCommissionDet> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("M_COMMISSION_DET");
            mapping.Id(x => x.Id, "COMMISSION_DET_ID").GeneratedBy.Assigned();

            mapping.References(x => x.CommissionId, "COMMISSION_ID").Fetch.Join();

            mapping.Map(x => x.DetailType, "DETAIL_TYPE");
            mapping.Map(x => x.DetailValue, "DETAIL_VALUE");
            mapping.Map(x => x.DetailStatus, "DETAIL_STATUS");
            mapping.Map(x => x.DetailDesc, "DETAIL_DESC");

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

            mapping.Map(x => x.DetailLowTarget, "DETAIL_LOW_TARGET");
            mapping.Map(x => x.DetailHighTarget, "DETAIL_HIGH_TARGET");
        }

        #endregion
    }
}
