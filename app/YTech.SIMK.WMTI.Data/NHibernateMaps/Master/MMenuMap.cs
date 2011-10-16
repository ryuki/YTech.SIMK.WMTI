using FluentNHibernate.Automapping;
using YTech.SIMK.WMTI.Core.Master;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Master
{
    public class MMenuMap : IAutoMappingOverride<MMenu>
    {
        #region Implementation of IAutoMappingOverride<MMenu>

        public void Override(AutoMapping<MMenu> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("M_MENU");
            mapping.Id(x => x.Id, "MENU_ID").GeneratedBy.Assigned();

            mapping.References(x => x.MenuParent, "MENU_PARENT_ID");
            mapping.Map(x => x.MenuType, "MENU_TYPE");
            mapping.Map(x => x.MenuIcon, "MENU_ICON");
            mapping.Map(x => x.MenuName, "MENU_NAME");
            mapping.Map(x => x.MenuLink, "MENU_LINK");
            mapping.Map(x => x.MenuStatus, "MENU_STATUS");
            mapping.Map(x => x.MenuDesc, "MENU_DESC");

            mapping.HasMany(x => x.MenuChildren)
                .AsBag()
                .Inverse()
                .KeyColumn("MENU_PARENT_ID");

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
