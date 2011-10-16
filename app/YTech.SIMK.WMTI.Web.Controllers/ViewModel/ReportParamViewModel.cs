using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class ReportParamViewModel
    {
        public static ReportParamViewModel Create(ITRecPeriodRepository tRecPeriodRepository, IMPartnerRepository mPartnerRepository)
        {
            ReportParamViewModel viewModel = new ReportParamViewModel();
            viewModel.DateFrom = DateTime.Today.AddDays(-1);
            viewModel.DateTo = DateTime.Today;

            IList<TRecPeriod> listRecPeriod = tRecPeriodRepository.GetAll();
            TRecPeriod recPeriod = new TRecPeriod();
            recPeriod.PeriodDesc = "-Pilih Periode-";
            listRecPeriod.Insert(0, recPeriod);
            viewModel.RecPeriodList = new SelectList(listRecPeriod, "Id", "PeriodDesc");


            IList<MPartner> listPartner = mPartnerRepository.GetAll();
            MPartner partner = new MPartner();
            partner.PartnerName = "-Pilih Toko-";
            listPartner.Insert(0, partner);
            viewModel.PartnerList = new SelectList(listPartner, "Id", "PartnerName");
            return viewModel;
        }

        public bool ShowReport { get; internal set; }
        public string ExportFormat { get; internal set; }
        public string Title { get; internal set; }


        public bool ShowDateFrom { get; internal set; }
        public bool ShowDateTo { get; internal set; }
        public bool ShowRecPeriod { get; internal set; }
        public bool ShowPartner { get; internal set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string RecPeriodId { get; set; }
        public string PartnerId { get; set; }

        public SelectList RecPeriodList { get; internal set; }
        public SelectList PartnerList { get; internal set; }
    }
}
