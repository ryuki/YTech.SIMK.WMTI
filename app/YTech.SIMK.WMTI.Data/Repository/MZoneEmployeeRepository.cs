using System;
using System.Collections.Generic;
using System.Text;
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

        public IList<MZoneEmployee> GetListByDate(DateTime? startDate, DateTime? endDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from MZoneEmployee as ze ");

            sql.AppendLine(@" where ze.StartDate <= :startDate ");
            sql.AppendLine(@"   and ze.EndDate >= :endDate ");

            string query = string.Format(" select ze {0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetDateTime("startDate", startDate.Value);
            q.SetDateTime("endDate", endDate.Value);

            IList<MZoneEmployee> list = q.List<MZoneEmployee>();
            return list;
        }
    }
}
