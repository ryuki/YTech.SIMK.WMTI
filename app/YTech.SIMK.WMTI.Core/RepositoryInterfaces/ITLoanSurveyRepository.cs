using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface ITLoanSurveyRepository : INHibernateRepositoryWithTypedId<TLoanSurvey, string>
    {
        IEnumerable<TLoanSurvey> GetPagedLoanSurveyList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows);
    }
}
