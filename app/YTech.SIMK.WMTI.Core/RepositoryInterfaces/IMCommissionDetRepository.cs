using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface IMCommissionDetRepository : INHibernateRepositoryWithTypedId<MCommissionDet, string>
    {
    }
}
