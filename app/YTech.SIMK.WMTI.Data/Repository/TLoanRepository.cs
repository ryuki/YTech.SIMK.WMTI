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

        #endregion
    }
}
