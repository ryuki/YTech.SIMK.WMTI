using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface IMCommissionRepository : INHibernateRepositoryWithTypedId<MCommission, string>
    {
        IEnumerable<MCommission> GetPagedCommissionList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows);
    }
}
