using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
   public class ReportParamViewModel
   {
         public static ReportParamViewModel Create()
         {
             ReportParamViewModel viewModel = new ReportParamViewModel();
             viewModel.DateFrom = DateTime.Today.AddDays(-1);
             viewModel.DateTo = DateTime.Today;
             return viewModel;
         }

       public bool ShowReport { get; internal set; }
       public string ExportFormat { get; internal set; }
       public string Title { get; internal set; }


       public bool ShowDateFrom { get; internal set; }
       public bool ShowDateTo { get; internal set; }

       public DateTime? DateFrom { get; set; }
       public DateTime? DateTo { get; set; }
    }
}
