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
        public static InstallmentPaymentFormViewModel Create(ITInstallmentRepository installmentRepository, IMEmployeeRepository mEmployeeRepository, string loanCode)
        {
            installmentRepository.DbContext.BeginTransaction();
            InstallmentPaymentFormViewModel viewModel = new InstallmentPaymentFormViewModel();
            TInstallment ins = installmentRepository.GetLastInstallment(loanCode);
            if (ins == null)
            {
                ins = new TInstallment();
            }
            viewModel.installment = ins;
            viewModel.installment.InstallmentPaymentDate = DateTime.Today;
            installmentRepository.DbContext.RollbackTransaction();

            var listEmployee = mEmployeeRepository.GetEmployeeByDept(EnumDepartment.COL.ToString());
            MEmployee employee = new MEmployee();
            //mCustomer.SupplierName = "-Pilih Supplier-";
            listEmployee.Insert(0, employee);

            var collector = from emp in listEmployee
                            //where emp.DepartmentId.DepartmentName == "COLLECTOR"
                            select new { Id = emp.Id, Name = emp.PersonId != null ? emp.PersonId.PersonName : "-Pilih Kolektor-" };
            viewModel.CollectorList = new SelectList(collector, "Id", "Name", string.Empty);

            return viewModel;
        }

        public TInstallment installment { get; internal set; }
        public SelectList CollectorList { get; internal set; }
    }
}
