using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Data;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TCommissionRepository : NHibernateRepositoryWithTypedId<TCommission, string>, ITCommissionRepository
    {
    }
}
