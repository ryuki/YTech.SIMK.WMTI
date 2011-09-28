using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TLoanRepository : NHibernateRepositoryWithTypedId<TLoan, string>, ITLoanRepository
    {
        #region Implementation of ITLoanRepository

        public IEnumerable<TLoan> GetPagedLoanList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string loanStatus, string searchBy, string searchText)
        {
            StringBuilder sql = new StringBuilder();
            //query for late pay status
            if (loanStatus == Enums.EnumLoanStatus.LatePay.ToString())
            {
                sql.AppendLine(@"  from TInstallment as ins
                               right outer join ins.LoanId as loan ");

                sql.AppendLine(@" where ins.InstallmentStatus = :notPaidStatus ");
                sql.AppendLine(@"   and ins.InstallmentMaturityDate <= :today ");
                if (!string.IsNullOrEmpty(searchText))
                    sql.AppendFormat(@" and {0} like :searchText", searchBy);

                string queryCount = string.Format(" select count(distinct loan.Id) {0}", sql);
                IQuery q = Session.CreateQuery(queryCount);
                q.SetString("notPaidStatus", Enums.EnumInstallmentStatus.Not_Paid.ToString());
                q.SetDateTime("today", DateTime.Today);
                if (!string.IsNullOrEmpty(searchText))
                    q.SetString("searchText", string.Format("%{0}%", searchText));

                totalRows = Convert.ToInt32(q.UniqueResult());


                string query = string.Format(" select distinct loan {0}  order by loan.LoanCode", sql);
                q = Session.CreateQuery(query);
                q.SetString("notPaidStatus", Enums.EnumInstallmentStatus.Not_Paid.ToString());
                q.SetDateTime("today", DateTime.Today);
                if (!string.IsNullOrEmpty(searchText))
                    q.SetString("searchText", string.Format("%{0}%", searchText));
                q.SetMaxResults(maxRows);
                q.SetFirstResult((pageIndex - 1) * maxRows);
                IEnumerable<TLoan> list = q.List<TLoan>();
                return list;
            }
            //query for paid status
            else if (loanStatus == Enums.EnumLoanStatus.Paid.ToString())
            {
                sql.AppendLine(@"  from TInstallment as ins
                               right outer join ins.LoanId as loan ");

                sql.AppendLine(@" where ins.InstallmentStatus = :paidStatus ");
                sql.AppendLine(@"   and ins.InstallmentNo = loan.LoanTenor ");
                if (!string.IsNullOrEmpty(searchText))
                    sql.AppendFormat(@" and {0} like :searchText", searchBy);

                string queryCount = string.Format(" select count(distinct loan.Id) {0}", sql);
                IQuery q = Session.CreateQuery(queryCount);
                q.SetString("paidStatus", Enums.EnumInstallmentStatus.Paid.ToString());
                if (!string.IsNullOrEmpty(searchText))
                    q.SetString("searchText", string.Format("%{0}%", searchText));

                totalRows = Convert.ToInt32(q.UniqueResult());


                string query = string.Format(" select distinct loan {0}  order by loan.LoanCode", sql);
                q = Session.CreateQuery(query);
                q.SetString("paidStatus", Enums.EnumInstallmentStatus.Paid.ToString());
                if (!string.IsNullOrEmpty(searchText))
                    q.SetString("searchText", string.Format("%{0}%", searchText));
                q.SetMaxResults(maxRows);
                q.SetFirstResult((pageIndex - 1) * maxRows);
                IEnumerable<TLoan> list = q.List<TLoan>();
                return list;
            }
            else
            {
                ICriteria criteria = CreateNewCriteria(loanStatus, searchBy, searchText);

                //calculate total rows
                totalRows = criteria
                    .SetProjection(Projections.RowCount())
                    .FutureValue<int>().Value;

                //recreate criteria for remove last projection
                criteria = CreateNewCriteria(loanStatus, searchBy, searchText);
                //get list results
                criteria.SetMaxResults(maxRows)
                    .SetFirstResult((pageIndex - 1) * maxRows);

                criteria.AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false));

                IEnumerable<TLoan> list = criteria.List<TLoan>();
                return list;
            }
            return null;
        }

        private ICriteria CreateNewCriteria(string loanStatus, string searchBy, string searchText)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TLoan), "loan");
            //join table person
            criteria.CreateCriteria("PersonId", "person", JoinType.LeftOuterJoin);
            if (!string.IsNullOrEmpty(loanStatus))
                criteria.Add(Restrictions.Eq("loan.LoanStatus", loanStatus));
            if (!string.IsNullOrEmpty(searchText))
                criteria.Add(Restrictions.Like(searchBy, searchText, MatchMode.Anywhere));

            return criteria;
        }

        public IEnumerable<TLoan> GetPagedLoanListToday(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TLoan));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(TLoan))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.Add(Restrictions.Eq("LoanSubmissionDate", DateTime.Today));
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<TLoan> list = criteria.List<TLoan>();

            return list;
        }

        public IList<TLoan> GetListByAccDate(DateTime? startDate, DateTime? endDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TLoan as loan ");
            sql.AppendLine(@" where loan.LoanAccDate >= :startDate ");
            sql.AppendLine(@"   and loan.LoanAccDate <= :endDate ");
            
            string query = string.Format(" select loan {0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q = Session.CreateQuery(query);
            q.SetDateTime("startDate", startDate.Value);
            q.SetDateTime("endDate", endDate.Value);
            IList<TLoan> list = q.List<TLoan>();
            return list;
        }

        #endregion
    }
}
