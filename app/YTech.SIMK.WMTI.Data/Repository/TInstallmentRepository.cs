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
        public IEnumerable<TInstallment> GetPagedInstallmentList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string loanCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TInstallment as ins
                               left outer join ins.LoanId as loan ");

            sql.AppendLine(@" where loan.LoanCode = :loanCode");

            string queryCount = string.Format(" select count(ins.Id) {0}", sql);
            IQuery q = Session.CreateQuery(queryCount);
            q.SetString("loanCode", loanCode);
            q.SetString("loanCode", loanCode);

            totalRows = Convert.ToInt32(q.UniqueResult());


            string query = string.Format(" select ins {0}  order by ins.InstallmentNo", sql);
            q = Session.CreateQuery(query);
            q.SetString("loanCode", loanCode);
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

            string query = string.Format(" select ins {0}  order by ins.InstallmentNo ", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetString("loanCode", loanCode);
            q.SetString("status", EnumInstallmentStatus.Not_Paid.ToString());
            //q.SetString("status", loanId);
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

        public IEnumerable<TInstallment> GetInstallments(string loanCode)
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"from TInstallment as ins inner join ins.LoanId as loan");
            sql.AppendLine(@" where loan.LoanCode = :loanCode");

            string query = string.Format("select ins {0} order by ins.InstallmentNo", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetString("loanCode", loanCode);
            IEnumerable<TInstallment> list = q.List<TInstallment>();

            return list;
        }
    }
}
