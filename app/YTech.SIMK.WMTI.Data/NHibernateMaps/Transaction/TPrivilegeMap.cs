using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Transaction
{
    public class TPrivilegeMap : IAutoMappingOverride<TPrivilege>
    {
        #region Implementation of IAutoMappingOverride<TPrivilege>

        public void Override(AutoMapping<TPrivilege> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("T_PRIVILEGE");
            mapping.Id(x => x.Id, "PRIVILEGE_ID").GeneratedBy.Assigned();

            mapping.Map(x => x.PrivilegeType, "PRIVILEGE_TYPE");
            mapping.Map(x => x.UserName, "USER_NAME");
            mapping.References(x => x.MenuId, "MENU_ID");
            mapping.Map(x => x.PrivilegeStatus, "PRIVILEGE_STATUS");
            mapping.Map(x => x.PrivilegeDesc, "PRIVILEGE_DESC");

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
