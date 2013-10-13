using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class HomeViewModel
    {
        public static HomeViewModel Create(ITNewsRepository tNewsRepository, ITLoanRepository tLoanRepository)
        {
            var viewModel = new HomeViewModel();
            TNews news = tNewsRepository.GetByType(EnumNewsType.Promo);
            if (news == null)
            {
                news = new TNews();
            }
            viewModel.PromoNews = news;

            news = tNewsRepository.GetByType(EnumNewsType.Announcement);
            if (news == null)
            {
                news = new TNews();
            }
            viewModel.AnnouncementNews = news;

            DateTime currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            viewModel.CurrentLoanSummary = LoanSummary.Create(tLoanRepository, currentMonth);

            viewModel.OneMonthAgoLoanSummary = LoanSummary.Create(tLoanRepository, currentMonth.AddMonths(-1));

            viewModel.OneMonthAgoAchievement = AchievementSummary.Create(tLoanRepository, currentMonth.AddMonths(-1));

            return viewModel;
        }

        public TNews PromoNews { get; set; }
        public TNews AnnouncementNews { get; set; }

        public LoanSummary CurrentLoanSummary { get; set; }
        public LoanSummary OneMonthAgoLoanSummary { get; set; }
        public AchievementSummary OneMonthAgoAchievement { get; set; }
    }

    public class LoanSummary
    {
        public static LoanSummary Create(ITLoanRepository tLoanRepository, DateTime month)
        {
            LoanSummary summary = new LoanSummary();
            summary.Month = month;

            //calculate loan total
            IList list = tLoanRepository.GetTotalByMonth(month);
            if (list != null)
            {
                if (list.Count> 0)
                {
                    object[] obj = (object[])list[0];
                    summary.TotalHD = (decimal) obj[0];
                    summary.TotalHT = (decimal)obj[1];
                    summary.TotalInstallment = (decimal)obj[2];
                }
            }

            //calculate loan count
            summary.TotalLoanApprove = tLoanRepository.GetCountByLoanStatus(month, EnumLoanStatus.Approve);
            summary.TotalLoanReject = tLoanRepository.GetCountByLoanStatus(month, EnumLoanStatus.Reject);
            summary.TotalLoanPostpone = tLoanRepository.GetCountByLoanStatus(month, EnumLoanStatus.Postpone);
            summary.TotalLoanCancel = tLoanRepository.GetCountByLoanStatus(month, EnumLoanStatus.Cancel);
            summary.TotalLoan = summary.TotalLoanApprove + summary.TotalLoanReject + summary.TotalLoanPostpone +
                                summary.TotalLoanCancel;

            //calculate installment
            summary.TotalMustPaidInstallment = tLoanRepository.GetTotalInstallmentByStatus(month, "%");
            summary.TotalPaidInstallment = tLoanRepository.GetTotalInstallmentByStatus(month, EnumInstallmentStatus.Paid.ToString());
            
            return summary;
        }

        public DateTime Month { get; set; }

        public decimal TotalHD { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TotalInstallment { get; set; }

        public decimal TotalMustPaidInstallment { get; set; }
        public decimal TotalPaidInstallment { get; set; }

        public decimal TotalLoan { get; set; }
        public decimal TotalLoanApprove { get; set; }
        public decimal TotalLoanReject { get; set; }
        public decimal TotalLoanPostpone { get; set; }
        public decimal TotalLoanCancel { get; set; }
    }

    public class AchievementSummary
    {
        public static AchievementSummary Create(ITLoanRepository tLoanRepository, DateTime month)
        {
            AchievementSummary summary = new AchievementSummary();
            summary.Month = month;

            //get collector 
            IList listCol = tLoanRepository.GetMaxCollector(month);
            if (listCol != null)
            {
                if (listCol.Count > 0)
                {
                    object[] obj = (object[])listCol[0];
                    summary.CollectorName = obj[0].ToString();
                    summary.CollectorTarget = (decimal)obj[1];
                    summary.CollectorAchievement = (decimal)obj[2];
                }
            }

            //get tls 
            IList listTLS = tLoanRepository.GetMaxTLS(month);
            if (listTLS != null)
            {
                if (listTLS.Count > 0)
                {
                    object[] obj = (object[])listTLS[0];
                    summary.LTSName = obj[0].ToString();
                    summary.LTSAchievement = (decimal)obj[1];
                    summary.LTSTarget = (decimal)obj[2];
                }
            }

            //get sales 
            IList listSalesman = tLoanRepository.GetMaxSalesman(month);
            if (listSalesman != null)
            {
                if (listSalesman.Count > 0)
                {
                    object[] obj = (object[])listSalesman[0];
                    summary.SalesmanName = obj[0].ToString();
                    summary.SalesmanAchievement = (decimal)obj[1];
                    summary.SalesmanTarget = (decimal)obj[2];
                }
            }

            return summary;
        }

        public DateTime Month { get; set; }

        public string CollectorName { get; set; }
        public decimal CollectorAchievement { get; set; }
        public decimal CollectorTarget { get; set; }

        public string LTSName { get; set; }
        public decimal LTSAchievement { get; set; }
        public decimal LTSTarget { get; set; }

        public string SalesmanName { get; set; }
        public decimal SalesmanAchievement { get; set; }
        public decimal SalesmanTarget { get; set; }

    }
}
