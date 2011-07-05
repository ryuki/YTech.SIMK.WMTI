﻿using System;
using System.Linq;
using System.Web.Mvc;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class CRFormViewModel
    {
        public static CRFormViewModel CreateCRFormViewModel(IMEmployeeRepository mEmployeeRepository, string loanCustomerRequestId)
        {
            CRFormViewModel viewModel = new CRFormViewModel();
            viewModel.CanEditId = true;

            TLoan loan = new TLoan();
            TLoanUnit loanUnit = new TLoanUnit();
            MEmployee emp = new MEmployee();
            MCustomer cust = new MCustomer();
            RefPerson person = new RefPerson();
            RefAddress address = new RefAddress();

            loan.TLSId = emp;
            loan.CustomerId = cust;
            loan.CustomerId.PersonId = person;
            loan.CustomerId.AddressId = address;

            viewModel.Loan = loan;

            var listEmployee = mEmployeeRepository.GetAll();
            MEmployee employee = new MEmployee();
            listEmployee.Insert(0, employee);

            var tls = from emps in listEmployee
                      //where emp.DepartmentId.DepartmentName == "TEAM LEADER SALESMAN"
                      select new { Id = emps.Id, Name = emps.PersonId != null ? emps.PersonId.PersonName : "-Pilih Team Leader-" };
            viewModel.TLSList = new SelectList(tls, "Id", "Name", loan.TLSId != null ? loan.TLSId.Id : string.Empty);

            var salesman = from emps in listEmployee
                           //where emp.DepartmentId.DepartmentName == "SALESMAN"
                           select new { Id = emps.Id, Name = emps.PersonId != null ? emps.PersonId.PersonName : "-Pilih Salesman-" };
            viewModel.SalesmanList = new SelectList(salesman, "Id", "Name", loan.SalesmanId != null ? loan.SalesmanId.Id : string.Empty);

            var surveyor = from emps in listEmployee
                           //where emp.DepartmentId.DepartmentName == "SURVEYOR"
                           select new { Id = emps.Id, Name = emps.PersonId != null ? emps.PersonId.PersonName : "-Pilih Surveyor-" };
            viewModel.SurveyorList = new SelectList(surveyor, "Id", "Name", loan.SurveyorId != null ? loan.SurveyorId.Id : string.Empty);

            return viewModel;
        }
/*
        private static void SetFeeCheck(string fee, CRFormViewModel viewModel)
        {
            if(!string.IsNullOrEmpty(fee))
            {
                string[] feeArr = fee.Split((",").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (feeArr.Contains("ADM"))
                    viewModel.LoanAdminFee = true;
                if (feeArr.Contains("MTA"))
                    viewModel.LoanMateraiFee = true;
            }
        }
*/
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
