using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Transaction
{
    public class TNewsMap : IAutoMappingOverride<TNews>
    {
        #region Implementation of IAutoMappingOverride<TPrivilege>

        public void Override(AutoMapping<TNews> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("T_NEWS");
            mapping.Id(x => x.Id, "NEWS_ID").GeneratedBy.Assigned();

            mapping.Map(x => x.NewsTitle, "NEWS_TITLE");
            mapping.Map(x => x.NewsDesc, "NEWS_DESC");
            mapping.Map(x => x.NewsStartDate, "NEWS_START_DATE");
            mapping.Map(x => x.NewsEndDate, "NEWS_END_DATE");
            mapping.Map(x => x.NewsStatus, "NEWS_STATUS");
            mapping.Map(x => x.NewsTo, "NEWS_TO");
            mapping.Map(x => x.NewsType, "NEWS_TYPE");

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
