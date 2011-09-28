using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
   public class ReportParamViewModel
   {
       public static ReportParamViewModel Create(ITRecPeriodRepository tRecPeriodRepository)
         {
             ReportParamViewModel viewModel = new ReportParamViewModel();
             viewModel.DateFrom = DateTime.Today.AddDays(-1);
             viewModel.DateTo = DateTime.Today;

             IList<TRecPeriod> listRecPeriod = tRecPeriodRepository.GetAll();
             TRecPeriod recPeriod = new TRecPeriod();
             recPeriod.PeriodDesc = "-Pilih Periode-";
             listRecPeriod.Insert(0, recPeriod);
             viewModel.RecPeriodList = new SelectList(listRecPeriod, "Id", "PeriodDesc");
             return viewModel;
         }

       public bool ShowReport { get; internal set; }
       public string ExportFormat { get; internal set; }
       public string Title { get; internal set; }


       public bool ShowDateFrom { get; internal set; }
       public bool ShowDateTo { get; internal set; }
       public bool ShowRecPeriod { get; internal set; }

       public DateTime? DateFrom { get; set; }
       public DateTime? DateTo { get; set; }
       public string RecPeriodId { get; set; }

       public SelectList RecPeriodList { get; internal set; }
    }
}
