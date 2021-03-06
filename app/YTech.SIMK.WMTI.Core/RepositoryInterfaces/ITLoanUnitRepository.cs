﻿using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface ITLoanUnitRepository : INHibernateRepositoryWithTypedId<TLoanUnit, string>
    {
    }
}
