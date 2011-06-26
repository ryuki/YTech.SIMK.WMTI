using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Master
{
    public class MZoneEmployeeMap : IAutoMappingOverride<MZoneEmployee>
    {
        #region Implementation of IAutoMappingOverride<MZoneEmployee>

        public void Override(AutoMapping<MZoneEmployee> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("M_ZONE_EMPLOYEE");
            mapping.Id(x => x.Id, "ZONE_EMPLOYEE_ID").GeneratedBy.Assigned();
            mapping.References(x => x.ZoneId, "ZONE_ID").Fetch.Join();
            mapping.References(x => x.EmployeeId, "EMPLOYEE_ID").Fetch.Join();

            mapping.Map(x => x.StartDate, "START_DATE");
            mapping.Map(x => x.EndDate, "END_DATE");
            mapping.Map(x => x.ZoneEmployeeStatus, "ZONE_EMPLOYEE_STATUS");
            mapping.Map(x => x.ZoneEmployeeDesc, "ZONE_EMPLOYEE_DESC");

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
