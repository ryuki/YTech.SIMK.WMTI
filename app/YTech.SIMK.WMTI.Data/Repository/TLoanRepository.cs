using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;
using System.Linq;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TLoanRepository : NHibernateRepositoryWithTypedId<TLoan, string>, ITLoanRepository
    {
        #region Implementation of ITLoanRepository

        public IEnumerable<TLoan> GetPagedLoanList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string loanStatus, string searchBy, string searchText, string zoneId, string collectorId, string tLSId, string salesmanId, int? month, int? year)
        {
            StringBuilder sql = new StringBuilder();
            //query for late pay status
            if (loanStatus == Enums.EnumLoanStatus.LatePay.ToString())
            {
                sql.AppendLine(@"  from TInstallment as ins
                               right outer join ins.LoanId as loan ");

                sql.AppendLine(@" where ins.InstallmentStatus = :notPaidStatus ");
                sql.AppendLine(@"   and ins.InstallmentMaturityDate <= :today ");
                sql.AppendLine(@"   and loan.LoanStatus = :loanStatus ");
                if (!string.IsNullOrEmpty(zoneId))
                    sql.AppendLine(@" and loan.ZoneId.Id = :zoneId ");
                if (!string.IsNullOrEmpty(collectorId))
                    sql.AppendLine(@" and loan.CollectorId.Id = :collectorId ");
                if (!string.IsNullOrEmpty(searchText))
                    sql.AppendFormat(@" and {0} like :searchText", searchBy);
                if (!string.IsNullOrEmpty(tLSId))
                    sql.AppendLine(@" and loan.TLSId.Id = :tLSId ");
                if (!string.IsNullOrEmpty(salesmanId))
                    sql.AppendLine(@" and loan.SalesmanId.Id = :salesmanId ");
                if (month.HasValue)
                    sql.AppendLine(@" and month(loan.LoanSubmissionDate) = :month ");
                if (year.HasValue)
                    sql.AppendLine(@" and year(loan.LoanSubmissionDate) = :year ");

                string queryCount = string.Format(" select count(distinct loan.Id) {0}", sql);
                IQuery q = Session.CreateQuery(queryCount);
                q.SetString("notPaidStatus", Enums.EnumInstallmentStatus.Not_Paid.ToString());
                q.SetString("loanStatus", Enums.EnumLoanStatus.OK.ToString());
                q.SetDateTime("today", DateTime.Today);
                CreateLatePayStringCriteria(q, loanStatus, searchBy, searchText, zoneId, collectorId, tLSId, salesmanId, month, year);

                totalRows = Convert.ToInt32(q.UniqueResult());


                string query = string.Format(" select distinct loan {0}  order by loan.LoanCode", sql);
                q = Session.CreateQuery(query);
                q.SetString("notPaidStatus", Enums.EnumInstallmentStatus.Not_Paid.ToString());
                q.SetString("loanStatus", Enums.EnumLoanStatus.OK.ToString());
                q.SetDateTime("today", DateTime.Today);
                CreateLatePayStringCriteria(q, loanStatus, searchBy, searchText, zoneId, collectorId, tLSId, salesmanId, month, year);
                q.SetMaxResults(maxRows);
                q.SetFirstResult((pageIndex - 1) * maxRows);
                IEnumerable<TLoan> list = q.List<TLoan>();
                return list;
            }
            //            //query for paid status
            //            else if (loanStatus == Enums.EnumLoanStatus.Paid.ToString())
            //            {
            //                sql.AppendLine(@"  from TInstallment as ins
            //                               right outer join ins.LoanId as loan ");

//                sql.AppendLine(@" where ins.InstallmentStatus = :paidStatus ");
            //                sql.AppendLine(@"   and ins.LoanStatus = :loanStatus ");
            //                sql.AppendLine(@"   and ins.InstallmentNo = loan.LoanTenor ");
            //                if (!string.IsNullOrEmpty(zoneId))
            //                    sql.AppendLine(@" and loan.ZoneId.Id = :zoneId ");
            //                if (!string.IsNullOrEmpty(collectorId))
            //                    sql.AppendLine(@" and loan.CollectorId.Id = :collectorId ");
            //                if (!string.IsNullOrEmpty(searchText))
            //                    sql.AppendFormat(@" and {0} like :searchText", searchBy);
            //                sql.AppendLine(@"   order by ins.InstallmentNo desc ");

//                string queryCount = string.Format(" select count(distinct loan.Id) {0}", sql);
            //                IQuery q = Session.CreateQuery(queryCount);
            //                q.SetString("paidStatus", Enums.EnumInstallmentStatus.Paid.ToString());
            //                q.SetString("loanStatus", Enums.EnumLoanStatus.OK.ToString());
            //                if (!string.IsNullOrEmpty(searchText))
            //                    q.SetString("searchText", string.Format("%{0}%", searchText));

//                totalRows = Convert.ToInt32(q.UniqueResult());


//                string query = string.Format(" select distinct loan {0}  order by loan.LoanCode", sql);
            //                q = Session.CreateQuery(query);
            //                q.SetString("paidStatus", Enums.EnumInstallmentStatus.Paid.ToString());
            //                q.SetString("loanStatus", Enums.EnumLoanStatus.OK.ToString());
            //                if (!string.IsNullOrEmpty(zoneId))
            //                    q.SetString("zoneId", zoneId);
            //                if (!string.IsNullOrEmpty(collectorId))
            //                    q.SetString("collectorId", collectorId);
            //                if (!string.IsNullOrEmpty(searchText))
            //                    q.SetString("searchText", string.Format("%{0}%", searchText));
            //                q.SetMaxResults(maxRows);
            //                q.SetFirstResult((pageIndex - 1) * maxRows);
            //                IEnumerable<TLoan> list = q.List<TLoan>();
            //                return list;
            //            }
            else
            {
                ICriteria criteria = CreateNewCriteria(loanStatus, searchBy, searchText, zoneId, collectorId, tLSId, salesmanId, month, year);

                //calculate total rows
                totalRows = criteria
                    .SetProjection(Projections.RowCount())
                    .FutureValue<int>().Value;

                //recreate criteria for remove last projection
                criteria = CreateNewCriteria(loanStatus, searchBy, searchText, zoneId, collectorId, tLSId, salesmanId, month, year);
                //get list results
                criteria.SetMaxResults(maxRows)
                    .SetFirstResult((pageIndex - 1) * maxRows);

                criteria.AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false));

                IEnumerable<TLoan> list = criteria.List<TLoan>();
                return list;
            }
            return null;
        }

        private void CreateLatePayStringCriteria(IQuery q,string loanStatus, string searchBy, string searchText, string zoneId, string collectorId, string tLSId, string salesmanId, int? month, int? year)
        {
            if (!string.IsNullOrEmpty(zoneId))
                q.SetString("zoneId", zoneId);
            if (!string.IsNullOrEmpty(collectorId))
                q.SetString("collectorId", collectorId);
            if (!string.IsNullOrEmpty(searchText))
                q.SetString("searchText", string.Format("%{0}%", searchText));
            if (!string.IsNullOrEmpty(tLSId))
                q.SetString("tLSId", tLSId);
            if (!string.IsNullOrEmpty(salesmanId))
                q.SetString("salesmanId", salesmanId);
            if (month.HasValue)
                q.SetInt32("month", month.Value);
            if (year.HasValue)
                q.SetInt32("year", year.Value);
        }

        private ICriteria CreateNewCriteria(string loanStatus, string searchBy, string searchText, string zoneId, string collectorId, string tLSId, string salesmanId, int? month, int? year)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TLoan), "loan");
            //join table person
            criteria.CreateCriteria("PersonId", "person", JoinType.LeftOuterJoin);
            if (!string.IsNullOrEmpty(loanStatus))
                criteria.Add(Restrictions.Eq("loan.LoanStatus", loanStatus));
            if (!string.IsNullOrEmpty(searchText))
                criteria.Add(Restrictions.Like(searchBy, searchText, MatchMode.Anywhere));
            if (!string.IsNullOrEmpty(zoneId))
                criteria.Add(Restrictions.Eq("loan.ZoneId.Id", zoneId));
            if (!string.IsNullOrEmpty(collectorId))
                criteria.Add(Restrictions.Eq("loan.CollectorId.Id", collectorId));
            if (!string.IsNullOrEmpty(tLSId))
                criteria.Add(Restrictions.Eq("loan.TLSId.Id", tLSId));
            if (!string.IsNullOrEmpty(salesmanId))
                criteria.Add(Restrictions.Eq("loan.SalesmanId.Id", salesmanId));
            if (month.HasValue)
            {
                var dateMonth = Projections.SqlFunction("month", NHibernateUtil.Int32, Projections.Property("loan.LoanSubmissionDate"));
                criteria.Add(Restrictions.Eq(dateMonth, month));
                //var dateYear = Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("loan.LoanAccDate"));
                //int checkYear = year.HasValue ? year.Value : DateTime.Today.Year;
                //criteria.Add(Restrictions.Eq(dateYear, checkYear));
            }
            if (year.HasValue)
            {
                var dateYear = Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("loan.LoanSubmissionDate"));
                criteria.Add(Restrictions.Eq(dateYear, year));
            }

            return criteria;
        }

        public IEnumerable<TLoan> GetPagedLoanListToday(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TLoan));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(TLoan))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.Add(Restrictions.Eq("LoanSubmissionDate", DateTime.Today));
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<TLoan> list = criteria.List<TLoan>();

            return list;
        }

        public IList<TLoan> GetListByAccDate(DateTime? startDate, DateTime? endDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TLoan as loan ");
            sql.AppendLine(@" where loan.LoanAccDate >= :startDate ");
            sql.AppendLine(@"   and loan.LoanAccDate <= :endDate ");

            string query = string.Format(" select loan {0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q = Session.CreateQuery(query);
            q.SetDateTime("startDate", startDate.Value);
            q.SetDateTime("endDate", endDate.Value);
            IList<TLoan> list = q.List<TLoan>();
            return list;
        }

        public IEnumerable<TLoan> GetListByAccDatePartner(DateTime? dateFrom, DateTime? dateTo, string partnerId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TLoan as loan ");
            sql.AppendLine(@" where loan.LoanAccDate >= :dateFrom ");
            sql.AppendLine(@"   and loan.LoanAccDate <= :dateTo ");
            sql.AppendLine(@"   and loan.LoanStatus = :loanStatus ");
            if (!string.IsNullOrEmpty(partnerId))
                sql.AppendLine(@"   and loan.PartnerId.Id = :partnerId ");

            string query = string.Format(" select loan {0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q = Session.CreateQuery(query);
            q.SetDateTime("dateFrom", dateFrom.Value);
            q.SetDateTime("dateTo", dateTo.Value);
            q.SetString("loanStatus", Enums.EnumLoanStatus.OK.ToString());
            if (!string.IsNullOrEmpty(partnerId))
                q.SetString("partnerId", partnerId);
            IList<TLoan> list = q.List<TLoan>();
            return list;
        }

        public decimal? GetTotalInstallmentLoan(string loanStatus, string searchBy, string searchText, string zoneId, string collectorId, string tLSId, string salesmanId, int? month, int? year)
        {
            StringBuilder sql = new StringBuilder();
            //query for late pay status
            if (loanStatus == EnumLoanStatus.LatePay.ToString())
            {
                sql.AppendLine(@"  select distinct loan  ");
                sql.AppendLine(@"  from TInstallment as ins
                               right outer join ins.LoanId as loan ");

                sql.AppendLine(@" where ins.InstallmentStatus = :notPaidStatus ");
                sql.AppendLine(@"   and ins.InstallmentMaturityDate <= :today ");
                sql.AppendLine(@"   and loan.LoanStatus = :loanStatus ");
                if (!string.IsNullOrEmpty(zoneId))
                    sql.AppendLine(@" and loan.ZoneId.Id = :zoneId ");
                if (!string.IsNullOrEmpty(collectorId))
                    sql.AppendLine(@" and loan.CollectorId.Id = :collectorId ");
                if (!string.IsNullOrEmpty(searchText))
                    sql.AppendFormat(@" and {0} like :searchText", searchBy);
                if (!string.IsNullOrEmpty(tLSId))
                    sql.AppendLine(@" and loan.TLSId.Id = :tLSId ");
                if (!string.IsNullOrEmpty(salesmanId))
                    sql.AppendLine(@" and loan.SalesmanId.Id = :salesmanId ");
                if (month.HasValue)
                    sql.AppendLine(@" and month(loan.LoanSubmissionDate) = :month ");
                if (year.HasValue)
                    sql.AppendLine(@" and year(loan.LoanSubmissionDate) = :year ");
                sql.AppendLine(@" ) ");

                string querySum = string.Format("{0}", sql);
                IQuery q = Session.CreateQuery(querySum);
                q.SetString("notPaidStatus", Enums.EnumInstallmentStatus.Not_Paid.ToString());
                q.SetString("loanStatus", Enums.EnumLoanStatus.OK.ToString());
                q.SetDateTime("today", DateTime.Today);
                CreateLatePayStringCriteria(q, loanStatus, searchBy, searchText, zoneId, collectorId, tLSId, salesmanId, month, year);

                IList<TLoan> listLoan = q.List<TLoan>();


                decimal? totalInstallment = listLoan.Sum(x => x.LoanBasicInstallment);
                return totalInstallment;
            }
            else
            {
                ICriteria criteria = CreateNewCriteria(loanStatus, searchBy, searchText, zoneId, collectorId, tLSId, salesmanId, month, year);

                //calculate total
                object total = criteria
                    .SetProjection(Projections.Sum("loan.LoanBasicInstallment"))
                    .UniqueResult();

                return total == null ? 0 : (decimal?)total;
            }
        }

        public IList GetTotalByMonth(DateTime month)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  select coalesce(sum(loan.LoanBasicPrice),0), coalesce(sum(loan.LoanCreditPrice),0), coalesce(sum(loan.LoanBasicInstallment),0) ");
            sql.AppendLine(@"  from TLoan as loan ");
            sql.AppendLine(@" where loan.LoanSubmissionDate >= :startDate ");
            sql.AppendLine(@"   and loan.LoanSubmissionDate <= :endDate ");
            sql.AppendLine(@"   and loan.LoanStatus = :loanStatus ");

            string query = string.Format("{0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q = Session.CreateQuery(query);
            q.SetDateTime("startDate", month);
            q.SetDateTime("endDate", month.AddMonths(1).AddDays(-1));
            q.SetString("loanStatus", EnumLoanStatus.OK.ToString());
            IList list = q.List();
            return list;
        }

        public Int64 GetCountByLoanStatus(DateTime month, EnumLoanStatus loanStatus)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  select coalesce(count(loan.Id),0) ");
            sql.AppendLine(@"  from TLoan as loan ");
            sql.AppendLine(@" where loan.LoanSubmissionDate >= :startDate ");
            sql.AppendLine(@"   and loan.LoanSubmissionDate <= :endDate ");
            sql.AppendLine(@"   and loan.LoanStatus = :loanStatus ");

            string query = string.Format("{0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q = Session.CreateQuery(query);
            q.SetDateTime("startDate", month);
            q.SetDateTime("endDate", month.AddMonths(1).AddDays(-1));
            q.SetString("loanStatus", loanStatus.ToString());
            return q.UniqueResult<Int64>();
        }

        public decimal GetTotalInstallmentByStatus(DateTime month, string installmentStatus)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  select coalesce(sum(ins.InstallmentBasic),0) ");
            sql.AppendLine(@"  from TInstallment as ins ");
            sql.AppendLine(@" where ins.InstallmentMaturityDate >= :startDate ");
            sql.AppendLine(@"   and ins.InstallmentMaturityDate <= :endDate ");
            sql.AppendLine(@"   and ins.InstallmentStatus like :installmentStatus ");

            string query = string.Format("{0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q = Session.CreateQuery(query);
            q.SetDateTime("startDate", month);
            q.SetDateTime("endDate", month.AddMonths(1).AddDays(-1));
            q.SetString("installmentStatus", installmentStatus);
            return q.UniqueResult<decimal>();
        }

        public IList GetMaxCollector(DateTime month)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"            
                select top 1 isnull(a.COLLECTOR_ID,'') COLLECTOR_ID,isnull(a.LOAN_BASIC_INSTALLMENT,0) LOAN_BASIC_INSTALLMENT, isnull(b.INSTALLMENT_BASIC,0) INSTALLMENT_BASIC
                from (
                select loan.COLLECTOR_ID, sum(loan.LOAN_BASIC_INSTALLMENT) LOAN_BASIC_INSTALLMENT
                from dbo.T_LOAN loan
                where loan.LOAN_STATUS = 'OK'
                and loan.COLLECTOR_ID is not null
                group by loan.COLLECTOR_ID
                having sum(loan.LOAN_BASIC_INSTALLMENT) > 0 ) a
                left join
                (
                select ins.EMPLOYEE_ID, sum(ins.INSTALLMENT_BASIC) INSTALLMENT_BASIC
                from dbo.T_INSTALLMENT ins
                where ins.INSTALLMENT_MATURITY_DATE >= :startDate and ins.INSTALLMENT_MATURITY_DATE <= :endDate
                and ins.INSTALLMENT_STATUS = 'Paid' and ins.EMPLOYEE_ID is not null
                group by ins.EMPLOYEE_ID ) b
                on a.COLLECTOR_ID = b.EMPLOYEE_ID
                order by b.INSTALLMENT_BASIC desc, a.LOAN_BASIC_INSTALLMENT desc;
            ");

            ISQLQuery q = Session.CreateSQLQuery(sql.ToString());
            q.SetDateTime("startDate", month);
            q.SetDateTime("endDate", month.AddMonths(1).AddDays(-1));
            q.AddScalar("COLLECTOR_ID", NHibernateUtil.String);
            q.AddScalar("LOAN_BASIC_INSTALLMENT", NHibernateUtil.Decimal);
            q.AddScalar("INSTALLMENT_BASIC", NHibernateUtil.Decimal);
            IList list = q.List();
            return list;
        }

        public IList GetMaxTLS(DateTime month)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"            
                select top 1 isnull(a.TLS_ID,'') TLS_ID ,isnull(a.LOAN_BASIC_PRICE,0) LOAN_BASIC_PRICE ,isnull(b.COMMISSION_VALUE,0)  COMMISSION_VALUE
                from (
                select loan.TLS_ID, sum(loan.LOAN_BASIC_PRICE) LOAN_BASIC_PRICE
                from dbo.T_LOAN loan
                where loan.LOAN_STATUS = 'OK'
                and loan.TLS_ID is not null
                and loan.LOAN_SUBMISSION_DATE >= :startDate and loan.LOAN_SUBMISSION_DATE <= :endDate
                group by loan.TLS_ID
                having sum(loan.LOAN_BASIC_PRICE) > 0 ) a
                left join
                (
                select com.COMMISSION_VALUE
                from dbo.M_COMMISSION com
                where com.COMMISSION_STATUS = 'TLS'
                and (com.COMMISSION_START_DATE <= :startDate
                or com.COMMISSION_END_DATE >= :endDate) ) b
                on 1=1
                order by a.LOAN_BASIC_PRICE desc;
            ");

            ISQLQuery q = Session.CreateSQLQuery(sql.ToString());
            q.SetDateTime("startDate", month);
            q.SetDateTime("endDate", month.AddMonths(1).AddDays(-1));
            q.AddScalar("TLS_ID", NHibernateUtil.String);
            q.AddScalar("LOAN_BASIC_PRICE", NHibernateUtil.Decimal);
            q.AddScalar("COMMISSION_VALUE", NHibernateUtil.Decimal);
            IList list = q.List();
            return list;
        }

        public IList GetMaxSalesman(DateTime month)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"            
                select top 1 isnull(a.SALESMAN_ID,'') SALESMAN_ID ,isnull(a.LOAN_CREDIT_PRICE,0) LOAN_CREDIT_PRICE ,isnull(b.COMMISSION_VALUE,0)  COMMISSION_VALUE
                from (
                select loan.SALESMAN_ID, sum(loan.LOAN_CREDIT_PRICE) LOAN_CREDIT_PRICE
                from dbo.T_LOAN loan
                where loan.LOAN_STATUS = 'OK'
                and loan.SALESMAN_ID is not null
                and loan.LOAN_SUBMISSION_DATE >= :startDate and loan.LOAN_SUBMISSION_DATE <= :endDate
                group by loan.SALESMAN_ID
                having sum(loan.LOAN_CREDIT_PRICE) > 0 ) a
                left join
                (
                select com.COMMISSION_VALUE
                from dbo.M_COMMISSION com
                where com.COMMISSION_STATUS = 'SA'
                and (com.COMMISSION_START_DATE <= :startDate
                or com.COMMISSION_END_DATE >= :endDate ) ) b
                on 1=1
                order by a.LOAN_CREDIT_PRICE desc;
            ");

            ISQLQuery q = Session.CreateSQLQuery(sql.ToString());
            q.SetDateTime("startDate", month);
            q.SetDateTime("endDate", month.AddMonths(1).AddDays(-1));
            q.AddScalar("SALESMAN_ID", NHibernateUtil.String);
            q.AddScalar("LOAN_CREDIT_PRICE", NHibernateUtil.Decimal);
            q.AddScalar("COMMISSION_VALUE", NHibernateUtil.Decimal);
            IList list = q.List();
            return list;
        }

        //update loan status to Paid when all installment have been paid
        public void UpdateLoanToPaid(string loanId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"            
                update l
                set loan_status = :loanStatus
                from t_loan l, 
                (
                select l.loan_id, count(i.installment_id) count_ins
                from dbo.T_LOAN l
                left join dbo.T_INSTALLMENT i
                on l.loan_id = i.loan_id and i.installment_status = :insStatus
                where l.loan_id = :loanId
                group by l.loan_id
                having count(i.installment_id) = 0
                ) i
                where l.loan_id = i.loan_id and i.count_ins = 0  and l.loan_status = :currentLoanStatus and l.loan_id = :loanId;
            ");

            ISQLQuery q = Session.CreateSQLQuery(sql.ToString());
            q.SetString("loanId", loanId);
            q.SetString("loanStatus", EnumLoanStatus.Paid.ToString());
            q.SetString("insStatus", EnumInstallmentStatus.Not_Paid.ToString());
            q.SetString("currentLoanStatus", EnumLoanStatus.OK.ToString());
            q.ExecuteUpdate();
        }

        public DateTime? GetLastMaturityDate(string loanId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  select coalesce(min(ins.InstallmentMaturityDate),getdate()) ");
            sql.AppendLine(@"  from TInstallment as ins ");
            sql.AppendLine(@" where ins.LoanId.Id = :loanId ");
            sql.AppendLine(@" and ins.InstallmentStatus = :installmentStatus ");

            string query = string.Format("{0} ", sql);
            IQuery q = Session.CreateQuery(query);
            q = Session.CreateQuery(query);
            q.SetString("loanId", loanId);
            q.SetString("installmentStatus", EnumInstallmentStatus.Not_Paid.ToString());
            return q.UniqueResult<DateTime>();
        }

        public TLoan GetLoanByLoanCode(string loanCode)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(TLoan));

            criteria.Add(Restrictions.Eq("LoanCode", loanCode));

            criteria.SetMaxResults(1);
            return criteria.UniqueResult<TLoan>();
        }

        #endregion
    }
}
