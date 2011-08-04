using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class MCommissionRepository : NHibernateRepositoryWithTypedId<MCommission, string>, IMCommissionRepository
    {
        public IEnumerable<MCommission> GetPagedCommissionList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string department)
        {
            ICriteria criteria = CreateNewCriteria(department);

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MCommission))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MCommission> list = criteria.List<MCommission>();
            return list;
        }

        private ICriteria CreateNewCriteria(string department)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MCommission), "commission");

            criteria.Add(Restrictions.Eq("commission.CommissionStatus", department));

            return criteria;
        }
    }
}
