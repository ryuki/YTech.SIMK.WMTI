using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TRecPeriodRepository : NHibernateRepositoryWithTypedId<TRecPeriod, string>, ITRecPeriodRepository
    {
        public DateTime? GetLastDateClosing()
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TRecPeriod))
            .SetProjection(Projections.Max("PeriodTo"));
          object obj =  criteria.UniqueResult();
            if (obj != null)
            {
                return Convert.ToDateTime(obj);
            }
            else
            {
                return null;
            }

            DateTime dt = criteria.FutureValue<DateTime>().Value;
            return dt;
            try
            {
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
