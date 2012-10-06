using System;
using System.Collections;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface ITLoanRepository : INHibernateRepositoryWithTypedId<TLoan, string>
    {
        IEnumerable<TLoan> GetPagedLoanList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string loanStatus, string searchBy, string searchText, string zoneId, string collectorId, string tLSId, string salesmanId, int? month, int? year);

        IEnumerable<TLoan> GetPagedLoanListToday(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows);

        IList<TLoan> GetListByAccDate(System.DateTime? startDate, System.DateTime? endDate);

        IEnumerable<TLoan> GetListByAccDatePartner(System.DateTime? dateFrom, System.DateTime? dateTo, string partnerId);

        decimal? GetTotalInstallmentLoan(string loanStatus, string searchBy, string searchText, string zoneId, string collectorId, string tLSId, string salesmanId, int? month, int? year);
        IList GetTotalByMonth(DateTime month);
        Int64 GetCountByLoanStatus(DateTime month, EnumLoanStatus loanStatus);

        decimal GetTotalInstallmentByStatus(DateTime month, string installmentStatus);

        IList GetMaxCollector(DateTime month);

        IList GetMaxTLS(DateTime month);

        IList GetMaxSalesman(DateTime month);

        void UpdateLoanToPaid(string loanId);

        DateTime? GetLastMaturityDate(string loanId);
    }
}
