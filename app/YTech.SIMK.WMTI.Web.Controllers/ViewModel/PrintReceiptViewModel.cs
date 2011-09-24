using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
   public class PrintReceiptViewModel
    {
       public static PrintReceiptViewModel Create()
        {
            var viewModel = new PrintReceiptViewModel();
            return viewModel;
        }

       public string LoanAcc { get; set; }
       public int InstallmentNo { get; set; }
    }
}
