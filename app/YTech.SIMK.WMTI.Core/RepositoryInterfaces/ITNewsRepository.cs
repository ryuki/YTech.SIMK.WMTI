using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface ITNewsRepository : INHibernateRepositoryWithTypedId<TNews, string>
    {
        TNews GetByType(Enums.EnumNewsType enumNewsType);
    }
}
