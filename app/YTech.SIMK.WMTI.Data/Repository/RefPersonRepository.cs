﻿using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class RefPersonRepository : NHibernateRepositoryWithTypedId<RefPerson, string>, IRefPersonRepository
    {
    }
}
