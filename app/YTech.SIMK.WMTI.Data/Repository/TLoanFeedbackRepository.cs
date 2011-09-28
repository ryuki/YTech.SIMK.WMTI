using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TLoanFeedbackRepository : NHibernateRepositoryWithTypedId<TLoanFeedback, string>, ITLoanFeedbackRepository
    {
        public TLoanFeedback GetLastFeedback(string loanId, EnumLoanFeedbackType feedbackType)
        {
            var sql = new StringBuilder();

            sql.AppendLine(@" from TLoanFeedback as lfb where lfb.LoanId.Id = :loanId and lfb.LoanFeedbackType = :feedbackType order by lfb.CreatedDate desc ");

            string query = string.Format(" select lfb {0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetString("loanId", loanId);
            q.SetString("feedbackType", feedbackType.ToString());

            IList<TLoanFeedback> list = q.List<TLoanFeedback>();
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }
    }
}
