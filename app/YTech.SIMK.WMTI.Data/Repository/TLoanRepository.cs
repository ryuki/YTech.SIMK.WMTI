using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TLoanRepository : NHibernateRepositoryWithTypedId<TLoan, string>, ITLoanRepository
    {
        #region Implementation of ITLoanRepository

        public IEnumerable<TLoan> GetPagedLoanList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TLoan));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(TLoan))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<TLoan> list = criteria.List<TLoan>();
            return list;
        }
        #endregion
    }
}
