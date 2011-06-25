using FluentNHibernate.Automapping;
using YTech.SIMK.WMTI.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Master
{
    public class MPartnerMap : IAutoMappingOverride<MPartner>
    {
        #region Implementation of IAutoMappingOverride<MPartner>
        
        public void Override(AutoMapping<MPartner> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("M_PARTNER");
            mapping.Id(x => x.Id, "PARTNER_ID").GeneratedBy.Assigned();

            mapping.References(x => x.AddressId, "ADDRESS_ID").Fetch.Join();

            mapping.Map(x => x.PartnerName, "PARTNER_NAME");
            mapping.Map(x => x.PartnerDesc, "PARTNER_DESC");
            mapping.Map(x => x.PartnerStatus, "PARTNER_STATUS");

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
