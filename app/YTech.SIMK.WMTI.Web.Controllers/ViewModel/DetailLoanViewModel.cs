using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class DetailLoanViewModel
    {
        public static DetailLoanViewModel Create(ITLoanRepository loanRepository, ITInstallmentRepository installmentRepository, string loanCode)
        {
            DetailLoanViewModel viewModel = new DetailLoanViewModel();
            viewModel.Loan = loanRepository.GetLoanByLoanCode(loanCode);

            var l = from t in viewModel.Loan.Installments
                    where
                        t.InstallmentStatus == EnumInstallmentStatus.Not_Paid.ToString() &&
                        t.InstallmentMaturityDate < DateTime.Today
                    select t;

            viewModel.InstallmentLate = l.Count();

            viewModel.InstallmentMore =
                (viewModel.Loan.Installments).Where(x => x.InstallmentStatus == EnumInstallmentStatus.Paid.ToString()).
                    Sum(x => x.InstallmentSisa).Value;

            viewModel.InstallmentMinus =
                (viewModel.Loan.Installments).Where(x => x.InstallmentStatus == EnumInstallmentStatus.Paid.ToString()).
                    Sum(x => x.InstallmentSisa).Value;

            viewModel.InstallmentFine =
                (viewModel.Loan.Installments).Where(x => x.InstallmentStatus == EnumInstallmentStatus.Paid.ToString()).
                    Sum(x => x.InstallmentFine).Value;

            string separator = "|";
            string[] photos = viewModel.Loan.LoanDesc.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            viewModel.Photo1 = "~/Content/Images/no_photo104_on.jpg";
            viewModel.Photo2 = "~/Content/Images/no_photo104_on.jpg";
            if (photos.Count() > 0)
            {
                viewModel.Photo1 = photos[0];
            }
            if (photos.Count() > 1)
            {
                viewModel.Photo2 = photos[1];
            }
            return viewModel;
        }

        public TLoan Loan { get; set; }
        public int InstallmentLate { get; set; }
        public decimal InstallmentMore { get; set; }
        public decimal InstallmentMinus { get; set; }
        public decimal InstallmentFine { get; set; }

        public string Photo1 { get; internal set; }
        public string Photo2 { get; internal set; }
    }
}
