﻿using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Master;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface IMCommissionRepository : INHibernateRepositoryWithTypedId<MCommission, string>
    {
    }
}