using FluentNHibernate.Automapping;
using YTech.SIMK.WMTI.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Master
{
    public class RefAddressMap : IAutoMappingOverride<RefAddress>
    {
        #region Implementation of IAutoMappingOverride<RefAddress>

        public void Override(AutoMapping<RefAddress> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("REF_ADDRESS");
            mapping.Id(x => x.Id, "ADDRESS_ID").GeneratedBy.Assigned();

            mapping.Map(x => x.AddressLine1, "ADDRESS_LINE1");
            mapping.Map(x => x.AddressLine2, "ADDRESS_LINE2");
            mapping.Map(x => x.AddressLine3, "ADDRESS_LINE3");
            mapping.Map(x => x.AddressRt, "ADDRESS_RT");
            mapping.Map(x => x.AddressRw, "ADDRESS_RW");
            mapping.Map(x => x.AddressPostCode, "ADDRESS_POST_CODE");
            mapping.Map(x => x.AddressCity, "ADDRESS_CITY");
            mapping.Map(x => x.AddressStatusOwner, "ADDRESS_STATUS_OWNER");
            mapping.Map(x => x.AddressLongStay, "ADDRESS_LONG_STAY");
            mapping.Map(x => x.AddressOffice, "ADDRESS_OFFICE");
            mapping.Map(x => x.AddressOfficePhone, "ADDRESS_OFFICE_PHONE");
            mapping.Map(x => x.AddressStatus, "ADDRESS_STATUS");
            mapping.Map(x => x.AddressDesc, "ADDRESS_DESC");

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
