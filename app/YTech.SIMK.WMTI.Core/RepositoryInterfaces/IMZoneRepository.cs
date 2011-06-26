using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface IMZoneRepository : INHibernateRepositoryWithTypedId<MZone, string>
    {
        IEnumerable<MZone> GetPagedZoneList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows);
    }
}
