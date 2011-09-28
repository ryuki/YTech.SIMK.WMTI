﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class OpeningViewModel
    {
        public static OpeningViewModel Create(ITRecPeriodRepository tRecPeriodRepository)
        {
            OpeningViewModel viewModel=new OpeningViewModel();
            IList<TRecPeriod> listRecPeriod = tRecPeriodRepository.GetAll();
            TRecPeriod recPeriod = new TRecPeriod();
            recPeriod.PeriodDesc = "-Pilih Period-";
            listRecPeriod.Insert(0, recPeriod);
            viewModel.RecPeriodList = new SelectList(listRecPeriod, "Id", "PeriodDesc");

            return viewModel;
        }

        public string RecPeriodId { get; set; }
        public SelectList RecPeriodList { get; internal set; }
    }
}
