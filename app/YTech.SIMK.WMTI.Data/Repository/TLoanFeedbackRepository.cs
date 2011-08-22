using System.Collections.Generic;
using System.Text;
using NHibernate;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TLoanFeedbackRepository : NHibernateRepositoryWithTypedId<TLoanFeedback, string>, ITLoanFeedbackRepository
    {
        public IList<TLoanFeedback> GetLoanFeedbackbyType(string type, string loanId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(@" from TLoanFeedback as lfb where lfb.LoanId = :loanId and lfb.LoanFeedbackType = :type ");

            string query = string.Format(" select lfb {0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetString("loanId", loanId);
            q.SetString("type", type);

            IList<TLoanFeedback> list = q.List<TLoanFeedback>();

            return list;
        }
    }
}
