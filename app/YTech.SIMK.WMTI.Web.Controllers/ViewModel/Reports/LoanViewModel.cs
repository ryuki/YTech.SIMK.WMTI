using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel.Reports
{
    public class LoanViewModel : TLoan
    {
        public string PartnerName { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string SalesmanName { get; set; }
    }
}
