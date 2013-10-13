using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class AcquittanceViewModel
    {
        public static AcquittanceViewModel Create(ITLoanRepository loanRepository, IMEmployeeRepository mEmployeeRepository, string loanCode)
        {
            AcquittanceViewModel viewModel = new AcquittanceViewModel();

            TLoan loan = loanRepository.GetLoanByLoanCode(loanCode);
            viewModel.BasicPrice = loan.LoanBasicPrice;
            viewModel.CreditPrice = loan.LoanCreditPrice;
            viewModel.Installment = loan.LoanBasicInstallment;
            viewModel.LoanTenor = loan.LoanTenor;
            viewModel.PaidInstallment =
                (loan.Installments).Where(x => x.InstallmentStatus == EnumInstallmentStatus.Paid.ToString()).Count();

            viewModel.RequiredInstallment = Math.Ceiling(loan.LoanTenor.Value * 0.75m);
            viewModel.RequiredInstallmentTotal = (viewModel.RequiredInstallment - viewModel.PaidInstallment) * viewModel.Installment;
            viewModel.LoseSort = viewModel.LoanTenor - viewModel.RequiredInstallment;
            viewModel.LoseSortTotal = viewModel.LoseSort * (viewModel.CreditPrice / viewModel.LoanTenor);
            viewModel.Provision = viewModel.LoseSortTotal.Value * 0.1m;
            viewModel.MustPaid = viewModel.RequiredInstallmentTotal + viewModel.LoseSortTotal + viewModel.Provision;


            var listEmployee = mEmployeeRepository.GetEmployeeByDept(EnumDepartment.COL.ToString());
            MEmployee employee = new MEmployee();
            listEmployee.Insert(0, employee);

            var collector = from emp in listEmployee
                            select new { Id = emp.Id, Name = emp.PersonId != null ? emp.PersonId.PersonName : "-Pilih Kolektor-" };
            viewModel.CollectorList = new SelectList(collector, "Id", "Id", string.Empty);
            return viewModel;
        }

        public decimal? BasicPrice { get; set; }
        public decimal? CreditPrice { get; set; }
        public decimal? Installment { get; set; }
        public decimal? LoanTenor { get; set; }
        public decimal? PaidInstallment { get; set; }
        public decimal? RequiredInstallment { get; set; }
        public decimal? RequiredInstallmentTotal { get; set; }
        public decimal? LoseSort { get; set; }
        public decimal? LoseSortTotal { get; set; }
        public decimal? Provision { get; set; }
        public decimal? MustPaid { get; set; }

        public SelectList CollectorList { get; internal set; }
        public string ReceiptNo { get; set; }
        public DateTime? InstallmentPaymentDate { get; set; }
    }
}
