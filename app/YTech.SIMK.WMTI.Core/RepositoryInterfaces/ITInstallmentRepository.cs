using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface ITInstallmentRepository : INHibernateRepositoryWithTypedId<TInstallment, string>
    {
        IEnumerable<TInstallment> GetPagedInstallmentList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string loanCode, EnumLoanStatus loanStatus);

        TInstallment GetLastInstallment(string loanCode);

        IEnumerable<TInstallment> GetListDueByDate(System.DateTime? dateFrom);

        void UpdateInstallmentByLoan(string loanId, decimal loanBasicInstallment, decimal loanInterest, decimal loanOtherInstallment);

        IEnumerable<TInstallment> GetInstallment(string loanCode, int installmentNo);

        IEnumerable<TInstallment> GetLastInstallmentByLoanId(string loanId);

        IList<TInstallment> GetListByMaturityDate(System.DateTime? dateFrom, System.DateTime? dateTo);
    }
}
