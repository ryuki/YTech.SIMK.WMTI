﻿using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TSettleRepository : NHibernateRepositoryWithTypedId<TSettle, string>, ITSettleRepository
    {
    }
}
