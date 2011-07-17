using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TLoanSurveyRepository : NHibernateRepositoryWithTypedId<TLoanSurvey, string>, ITLoanSurveyRepository
    {
        public IEnumerable<TLoanSurvey> GetPagedLoanSurveyList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TLoanSurvey));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(TLoanSurvey))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<TLoanSurvey> list = criteria.List<TLoanSurvey>();
            return list;
        }
    }
}
