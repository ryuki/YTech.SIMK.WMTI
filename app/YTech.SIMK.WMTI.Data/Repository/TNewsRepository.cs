using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TNewsRepository : NHibernateRepositoryWithTypedId<TNews, string>, ITNewsRepository
    {
        public TNews GetByType(EnumNewsType enumNewsType)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TNews));

            //get list results
            criteria.Add(Restrictions.Eq("NewsType", enumNewsType.ToString()));
            criteria.SetMaxResults(1);

            return criteria.UniqueResult<TNews>();
        }
    }
}
