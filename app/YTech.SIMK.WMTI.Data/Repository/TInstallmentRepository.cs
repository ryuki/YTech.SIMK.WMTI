using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Data;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TInstallmentRepository : NHibernateRepositoryWithTypedId<TInstallment, string>, ITInstallmentRepository
    {
        public IEnumerable<TInstallment> GetPagedInstallmentList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string loanCode, EnumLoanStatus loanStatus)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TInstallment as ins
                               left outer join ins.LoanId as loan ");

            sql.AppendLine(@" where loan.LoanCode = :loanCode");
            if (loanStatus != EnumLoanStatus.Nothing)
                sql.AppendLine(@" and loan.LoanStatus = :loanStatus ");

            string queryCount = string.Format(" select count(ins.Id) {0}", sql);
            IQuery q = Session.CreateQuery(queryCount);
            q.SetString("loanCode", loanCode);
            if (loanStatus != EnumLoanStatus.Nothing)
                q.SetString("loanStatus", loanStatus.ToString());

            totalRows = Convert.ToInt32(q.UniqueResult());


            string query = string.Format(" select ins {0}  order by ins.InstallmentNo", sql);
            q = Session.CreateQuery(query);
            q.SetString("loanCode", loanCode);
            if (loanStatus != EnumLoanStatus.Nothing)
                q.SetString("loanStatus", loanStatus.ToString());
            q.SetMaxResults(maxRows);
            q.SetFirstResult((pageIndex - 1) * maxRows);
            IEnumerable<TInstallment> list = q.List<TInstallment>();
            return list;
        }

        public TInstallment GetLastInstallment(string loanCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TInstallment as ins
                                               inner join ins.LoanId as loan ");

            sql.AppendLine(@" where loan.LoanCode = :loanCode and ins.InstallmentStatus = :status  ");
            sql.AppendLine(@" and loan.LoanStatus = :loanStatus ");

            string query = string.Format(" select ins {0}  order by ins.InstallmentNo ", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetString("loanCode", loanCode);
            q.SetString("status", EnumInstallmentStatus.Not_Paid.ToString());
            q.SetString("loanStatus", EnumLoanStatus.OK.ToString());
            q.SetMaxResults(1);

            return q.UniqueResult<TInstallment>();
        }

        IEnumerable<TInstallment> ITInstallmentRepository.GetListDueByDate(DateTime? dateFrom)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TInstallment as ins
                                               inner join ins.LoanId as loan ");

            sql.AppendLine(@" where ins.InstallmentStatus = :status and ins.InstallmentMaturityDate <= :dateFrom  ");

            string query = string.Format(" select ins {0}  order by loan.LoanCode ", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetDateTime("dateFrom", dateFrom.Value);
            q.SetString("status", EnumInstallmentStatus.Not_Paid.ToString());

            return q.List<TInstallment>();
        }

        public void UpdateInstallmentByLoan(string loanId, decimal loanBasicInstallment, decimal loanInterest, decimal loanOtherInstallment)
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"update TInstallment ins set ");
            sql.AppendLine(@"  ins.InstallmentBasic = :loanBasicInstallment");
            sql.AppendLine(@"  , ins.InstallmentInterest = :loanInterest");
            sql.AppendLine(@"  , ins.InstallmentOthers = :loanOtherInstallment");
            sql.AppendLine(@" where ins.LoanId.Id = :loanId");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("loanId", loanId);
            q.SetDecimal("loanBasicInstallment", loanBasicInstallment);
            q.SetDecimal("loanInterest", loanInterest);
            q.SetDecimal("loanOtherInstallment", loanOtherInstallment);
            q.ExecuteUpdate();
        }

        public IEnumerable<TInstallment> GetInstallment(string loanCode, int installmentNo)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TInstallment as ins
                                               inner join ins.LoanId as loan ");

            sql.AppendLine(@" where loan.LoanCode = :loanCode and ins.InstallmentNo = :installmentNo  ");

            string query = string.Format(" select ins {0}  order by loan.LoanCode ", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetString("loanCode", loanCode);
            q.SetInt32("installmentNo", installmentNo);

            return q.List<TInstallment>();
        }

        public IEnumerable<TInstallment> GetLastInstallmentByLoanId(string loanId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TInstallment as ins
                                               inner join ins.LoanId as loan ");

            sql.AppendLine(@" where loan.Id = :loanId and ins.InstallmentStatus = :status  ");

            string query = string.Format(" select ins {0}  order by ins.InstallmentNo ", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetString("loanId", loanId);
            q.SetString("status", EnumInstallmentStatus.Not_Paid.ToString());
            q.SetMaxResults(1);
            return q.List<TInstallment>();
        }

        public IList<TInstallment> GetListByMaturityDate(DateTime? dateFrom, DateTime? dateTo)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TInstallment as ins ");

            sql.AppendLine(@" where ins.InstallmentMaturityDate >= :dateFrom  ");
            sql.AppendLine(@"  and ins.InstallmentMaturityDate <= :dateTo  ");

            string query = string.Format(" select ins {0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetDateTime("dateFrom", dateFrom.Value);
            q.SetDateTime("dateTo", dateTo.Value);

            return q.List<TInstallment>();
        }
    }
}
