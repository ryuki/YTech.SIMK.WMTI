using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class MZoneEmployeeRepository : NHibernateRepositoryWithTypedId<MZoneEmployee, string>, IMZoneEmployeeRepository
    {
        public IEnumerable<MZoneEmployee> GetPagedZoneEmployeeList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MZoneEmployee));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MZoneEmployee))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MZoneEmployee> list = criteria.List<MZoneEmployee>();
            return list;
        }
    }
}
