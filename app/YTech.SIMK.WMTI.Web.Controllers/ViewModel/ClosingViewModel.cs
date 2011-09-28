using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class ClosingViewModel
    {
        public static ClosingViewModel Create(ITRecPeriodRepository tRecPeriodRepository)
        {
            var viewModel = new ClosingViewModel();
            //viewModel.StartDate = DateTime.Parse(string.Format("{0:yyyy-MM}-01", DateTime.Today));
            DateTime? dt = tRecPeriodRepository.GetLastDateClosing();
            if (dt.HasValue)
            {
                dt = dt.Value.AddDays(1);
            }
            else
            {
                dt = DateTime.Parse(string.Format("{0:yyyy-MM}-01", DateTime.Today));
            }
            viewModel.StartDate = dt;
            viewModel.EndDate = DateTime.Parse(string.Format("{0:yyyy-MM-dd}", viewModel.StartDate.Value.AddMonths(1).AddDays(-1)));
            return viewModel;
        }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
