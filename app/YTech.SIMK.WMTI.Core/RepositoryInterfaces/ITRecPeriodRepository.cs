using System;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface ITRecPeriodRepository : INHibernateRepositoryWithTypedId<TRecPeriod, string>
    {
        DateTime? GetLastDateClosing();

        void DeleteByRecPeriodId(string recPeriodId);
    }
}
