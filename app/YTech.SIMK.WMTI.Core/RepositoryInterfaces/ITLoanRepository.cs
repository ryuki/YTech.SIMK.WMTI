using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface ITLoanRepository : INHibernateRepositoryWithTypedId<TLoan, string>
    {
        IEnumerable<TLoan> GetPagedLoanList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string loanStatus, string searchBy, string searchText);

        IEnumerable<TLoan> GetPagedLoanListToday(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows);

        IList<TLoan> GetListByAccDate(System.DateTime? startDate, System.DateTime? endDate);

        IEnumerable<TLoan> GetListByAccDatePartner(System.DateTime? dateFrom, System.DateTime? dateTo, string partnerId);
    }
}
