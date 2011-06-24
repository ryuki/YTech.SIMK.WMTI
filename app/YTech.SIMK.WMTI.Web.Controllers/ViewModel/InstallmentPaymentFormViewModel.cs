using System;
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
using YTech.SIMK.WMTI.Web.Controllers.Helper;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class InstallmentPaymentFormViewModel
    {
        public static InstallmentPaymentFormViewModel Create(ITInstallmentRepository installmentRepository, string loanCode)
        {
            InstallmentPaymentFormViewModel viewModel = new InstallmentPaymentFormViewModel();
            TInstallment ins = installmentRepository.GetLastInstallment(loanCode);
            if (ins == null)
            {
                ins = new TInstallment();
            }
            viewModel.installment = ins;
            viewModel.installment.InstallmentPaymentDate = DateTime.Today;
            return viewModel;
        }

        public TInstallment installment { get; internal set; }
    }
}
