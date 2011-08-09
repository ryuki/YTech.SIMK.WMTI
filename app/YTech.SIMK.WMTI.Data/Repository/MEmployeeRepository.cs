using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class MEmployeeRepository : NHibernateRepositoryWithTypedId<MEmployee, string>, IMEmployeeRepository
    {
        public IEnumerable<MEmployee> GetPagedEmployeeList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MEmployee));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MEmployee))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MEmployee> list = criteria.List<MEmployee>();
            return list;
        }

        public IList<MEmployee> GetEmployeeByDept(string dept)
        {
            var sql = new StringBuilder();

            sql.AppendLine(@" from MEmployee as emp where emp.DepartmentId = :deptId ");

            string query = string.Format(" select emp {0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetString("deptId", dept);

            IList<MEmployee> list = q.List<MEmployee>();

            return list;
        }

        public IList<MEmployee> GetEmployeeBySuCol()
        {
            var sql = new StringBuilder();

            sql.AppendLine(@" from MEmployee as emp where emp.DepartmentId in ('SU','COL')");

            string query = string.Format(" select emp {0}", sql);
            IQuery q = Session.CreateQuery(query);

            IList<MEmployee> list = q.List<MEmployee>();

            return list;
        }
    }
}
