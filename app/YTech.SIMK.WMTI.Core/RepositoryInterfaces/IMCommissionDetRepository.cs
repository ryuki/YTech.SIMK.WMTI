using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface IMCommissionDetRepository : INHibernateRepositoryWithTypedId<MCommissionDet, string>
    {
        //IEnumerable<MCommissionDet> GetPagedCommissionDetList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows);

        IList<MCommissionDet> GetCommissionDetListById(string commissionId);
    }
}
