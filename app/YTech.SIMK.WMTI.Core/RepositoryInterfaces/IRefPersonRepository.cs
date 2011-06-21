using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface IRefPersonRepository : INHibernateRepositoryWithTypedId<RefPerson, string>
    {
    }
}
