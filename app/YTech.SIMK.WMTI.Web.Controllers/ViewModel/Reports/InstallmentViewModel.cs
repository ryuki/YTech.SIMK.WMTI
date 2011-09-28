using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel.Reports
{
    public class InstallmentViewModel : TInstallment
    {
        public string LoanNo { get; internal set; }
        public string LoanCode { get; internal set; }
        public string CustomerName { get; internal set; }
        public string CustomerAddress { get; internal set; }
        public string LoanAcc { get; internal set; }
        public string LoanTenor { get; internal set; }
        public string UnitName { get; internal set; }
        public string UnitType { get; internal set; }
    }
}
