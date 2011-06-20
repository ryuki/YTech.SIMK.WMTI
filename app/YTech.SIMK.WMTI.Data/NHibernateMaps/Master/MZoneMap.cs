using FluentNHibernate.Automapping;
using YTech.SIMK.WMTI.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Master
{
    public class MZoneMap : IAutoMappingOverride<MZone>
    {
        #region Implementation of IAutoMappingOverride<MZone>

        public void Override(AutoMapping<MZone> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("M_ZONE");
            mapping.Id(x => x.Id, "ZONE_ID").GeneratedBy.Assigned();

            mapping.Map(x => x.ZoneName, "ZONE_NAME");
            mapping.Map(x => x.ZoneCity, "ZONE_CITY");
            mapping.Map(x => x.ZoneStatus, "ZONE_STATUS");
            mapping.Map(x => x.ZoneDesc, "ZONE_DESC");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Map(x => x.RowVersion, "ROW_VERSION").ReadOnly();
        }

        #endregion
    }
}
