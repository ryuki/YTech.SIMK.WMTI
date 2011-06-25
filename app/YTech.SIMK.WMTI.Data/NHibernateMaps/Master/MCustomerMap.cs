using FluentNHibernate.Automapping;
using YTech.SIMK.WMTI.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Master
{
    public class MCustomerMap : IAutoMappingOverride<MCustomer>
    {
        #region Implementation of IAutoMappingOverride<MCustomer>

        public void Override(AutoMapping<MCustomer> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("dbo.M_CUSTOMER");
            mapping.Id(x => x.Id, "CUSTOMER_ID").GeneratedBy.Assigned();

            mapping.Map(x => x.CustomerCode, "CUSTOMER_CODE");

            mapping.References(x => x.PersonId, "PERSON_ID").Fetch.Join();
            mapping.References(x => x.AddressId, "ADDRESS_ID").Fetch.Join();

            mapping.Map(x => x.CustomerStatus, "CUSTOMER_STATUS");
            mapping.Map(x => x.CustomerDesc, "CUSTOMER_DESC");

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
