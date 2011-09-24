using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class LoanViewModel
    {
        public static LoanViewModel Create(EnumLoanStatus? loanStatus)
        {
            var viewModel = new LoanViewModel();

            viewModel.ShowInstallmentSubGrid = false;
            viewModel.ShowLegend = true;
            if (loanStatus != null)
            {
                switch (loanStatus)
                {
                    case EnumLoanStatus.OK:
                        viewModel.ShowInstallmentSubGrid = true;
                        break;
                    case EnumLoanStatus.Cancel:
                        viewModel.ShowLegend = false;
                        break;
                    case EnumLoanStatus.Reject:
                        viewModel.ShowLegend = false;
                        break;
                    case EnumLoanStatus.LatePay:
                        viewModel.ShowLegend = false;
                        viewModel.ShowInstallmentSubGrid = true;
                        break;
                    case EnumLoanStatus.Paid:
                        viewModel.ShowLegend = false;
                        viewModel.ShowInstallmentSubGrid = true;
                        break;
                }
            }

            return viewModel;
        }

        public bool ShowInstallmentSubGrid { get; internal set; }
        public bool ShowLegend { get; internal set; }
    }
}
