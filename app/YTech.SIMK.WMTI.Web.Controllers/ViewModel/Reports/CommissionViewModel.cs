using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel.Reports
{
    public class CommissionViewModel : TCommission
    {
        public DateTime? PeriodFrom { get; internal set; }
        public DateTime? PeriodTo { get; internal set; }
        public string EmployeeName { get; set; }
    }
}
