using System;
using System.Linq;
using System.Web.Mvc;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class CRFormViewModel
    {
        public static CRFormViewModel CreateCRFormViewModel(ITLoanRepository tLoanRepository, IMEmployeeRepository mEmployeeRepository, string loanCustomerRequestId)
        {
            CRFormViewModel viewModel = new CRFormViewModel();
            viewModel.CanEditId = true;

            TLoan loan = null;
            TLoanUnit loanUnit = null;
            RefPerson person = new RefPerson();
            RefAddress address = new RefAddress();

            if (!string.IsNullOrEmpty(loanCustomerRequestId))
            {
                loan = tLoanRepository.Get(loanCustomerRequestId);
                person = loan.PersonId;
                address = loan.AddressId;
                if (loan.LoanUnits.Count > 0)
                    loanUnit = loan.LoanUnits[0];
                else
                    loanUnit = new TLoanUnit();

                viewModel.CanEditId = false;
            }

            if (loan == null)
            {
                MEmployee emp = new MEmployee();
                MCustomer cust = new MCustomer();

                loan = new TLoan();
                loanUnit = new TLoanUnit();

                loan.TLSId = emp;
                loan.CustomerId = cust;
                loan.CustomerId.PersonId = person;
                loan.CustomerId.AddressId = address;
            }

            viewModel.Loan = loan;
            viewModel.LoanUnit = loanUnit;

            if (loan.LoanAdminFee != null)
                viewModel.LoanAdminFee = true;

            if (loan.LoanMateraiFee != null)
                viewModel.LoanMateraiFee = true;

            MEmployee employee = new MEmployee();

            var listTlsEmployee = mEmployeeRepository.GetEmployeeByDept(EnumDepartment.TLS.ToString());
            listTlsEmployee.Insert(0, employee);

            var tls = from emps in listTlsEmployee
                      select new { Id = emps.Id, Name = emps.PersonId != null ? emps.PersonId.PersonName : "-Pilih Team Leader-" };
            viewModel.TLSList = new SelectList(tls, "Id", "Name", loan.TLSId != null ? loan.TLSId.Id : string.Empty);

            var listSaEmployee = mEmployeeRepository.GetEmployeeByDept(EnumDepartment.SA.ToString());
            listSaEmployee.Insert(0, employee);

            var salesman = from emps in listSaEmployee
                           select new { Id = emps.Id, Name = emps.PersonId != null ? emps.PersonId.PersonName : "-Pilih Salesman-" };
            viewModel.SalesmanList = new SelectList(salesman, "Id", "Name", loan.SalesmanId != null ? loan.SalesmanId.Id : string.Empty);

            var listSuEmployee = mEmployeeRepository.GetEmployeeByDept(EnumDepartment.SU.ToString());
            listSuEmployee.Insert(0, employee);

            var surveyor = from emps in listSuEmployee
                           select new { Id = emps.Id, Name = emps.PersonId != null ? emps.PersonId.PersonName : "-Pilih Surveyor-" };
            viewModel.SurveyorList = new SelectList(surveyor, "Id", "Name", loan.SurveyorId != null ? loan.SurveyorId.Id : string.Empty);

            return viewModel;
        }

        public TLoan Loan { get; internal set; }
        public TLoanUnit LoanUnit { get; internal set; }

        public SelectList TLSList { get; internal set; }
        public SelectList SalesmanList { get; internal set; }
        public SelectList SurveyorList { get; internal set; }

        public bool CanEditId { get; internal set; }

        public bool LoanAdminFee { get; internal set; }
        public bool LoanMateraiFee { get; internal set; }
    }
}
