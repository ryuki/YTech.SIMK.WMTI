using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Enums;

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

        public MCommission GetCommissionByDate(EnumDepartment department, DateTime? startDate, DateTime? endDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from MCommission as comm ");

            sql.AppendLine(@" where comm.CommissionStatus = :department ");
            sql.AppendLine(@"   and comm.CommissionStartDate <= :startDate ");
            sql.AppendLine(@"   and comm.CommissionEndDate >= :endDate ");

            string query = string.Format(" select comm {0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetString("department", department.ToString());
            q.SetDateTime("startDate", startDate.Value);
            q.SetDateTime("endDate", endDate.Value);
            q.SetMaxResults(1);
            MCommission comm = q.UniqueResult<MCommission>();
            return comm;
        }

        private ICriteria CreateNewCriteria(string department)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MCommission), "commission");

            criteria.Add(Restrictions.Eq("commission.CommissionStatus", department));

            return criteria;
        }
    }
}
