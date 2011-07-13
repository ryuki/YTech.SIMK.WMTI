using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface IMPartnerRepository : INHibernateRepositoryWithTypedId<MPartner, string>
    {
        IEnumerable<MPartner> GetPagedPartnerList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows);
    }
}
