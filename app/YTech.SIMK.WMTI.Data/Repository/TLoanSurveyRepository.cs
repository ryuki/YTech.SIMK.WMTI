using System;
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

        public IList<TLoanSurvey> GetListBySurveyDate(DateTime? startDate, DateTime? endDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TLoanSurvey as survey ");
            sql.AppendLine(@" where survey.SurveyDate >= :startDate ");
            sql.AppendLine(@"   and survey.SurveyDate <= :endDate ");

            string query = string.Format(" select survey {0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q = Session.CreateQuery(query);
            q.SetDateTime("startDate", startDate.Value);
            q.SetDateTime("endDate", endDate.Value);
            IList<TLoanSurvey> list = q.List<TLoanSurvey>();
            return list;
        }
    }
}
