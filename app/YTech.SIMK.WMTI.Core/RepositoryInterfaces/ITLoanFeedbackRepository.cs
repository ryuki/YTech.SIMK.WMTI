using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface ITLoanFeedbackRepository : INHibernateRepositoryWithTypedId<TLoanFeedback, string>
    {
        TLoanFeedback GetLastFeedback(string loanId, Enums.EnumLoanFeedbackType feedbackType);
    }
}
