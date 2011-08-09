using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface IMZoneEmployeeRepository : INHibernateRepositoryWithTypedId<MZoneEmployee, string>
    {
        IEnumerable<MZoneEmployee> GetPagedZoneEmployeeList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows);
    }
}
