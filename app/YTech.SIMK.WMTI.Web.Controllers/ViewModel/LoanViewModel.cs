using System;
using System.Linq;
using System.Web.Mvc;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;
using YTech.SIMK.WMTI.Web.Controllers.Helper;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class LoanViewModel
    {
        public static LoanViewModel Create(EnumLoanStatus? loanStatus, IMEmployeeRepository mEmployeeRepository, IMZoneRepository mZoneRepository)
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

            MEmployee employee = new MEmployee();

            var listSaEmployee = mEmployeeRepository.GetEmployeeByDept(EnumDepartment.SA.ToString());
            listSaEmployee.Insert(0, employee);

            var salesman = from emp in listSaEmployee
                           select new { Id = emp.Id, Name = emp.PersonId != null ? emp.PersonId.PersonName : "-Semua Salesman-" };
            viewModel.SalesmanList = new SelectList(salesman, "Id", "Id", string.Empty);

            var listSuEmployee = mEmployeeRepository.GetEmployeeByDept(EnumDepartment.SU.ToString());
            listSuEmployee.Insert(0, employee);

            var surveyor = from emp in listSuEmployee
                           select new { Id = emp.Id, Name = emp.PersonId != null ? emp.PersonId.PersonName : "-Semua Surveyor-" };
            viewModel.SurveyorList = new SelectList(surveyor, "Id", "Id",string.Empty);

            var listColEmployee = mEmployeeRepository.GetEmployeeByDept(EnumDepartment.COL.ToString());
            listColEmployee.Insert(0, employee);

            var collector = from emp in listColEmployee
                            select new { Id = emp.Id, Name = emp.PersonId != null ? emp.PersonId.PersonName : "-Semua Kolektor-" };
            viewModel.CollectorList = new SelectList(collector, "Id", "Id", string.Empty);

            var listTlsEmployee = mEmployeeRepository.GetEmployeeByDept(EnumDepartment.TLS.ToString());
            listTlsEmployee.Insert(0, employee);

            var tls = from emp in listTlsEmployee
                      select new { Id = emp.Id, Name = emp.PersonId != null ? emp.PersonId.PersonName : "-Semua Team Leader-" };
            viewModel.TLSList = new SelectList(tls, "Id", "Id", string.Empty);

            var listZone = mZoneRepository.GetAll();
            MZone z = new MZone();
            z.ZoneName = "-Semua Wilayah-";
            listZone.Insert(0, z);
            var zones = from zo in listZone
                        select new { Id = zo.Id, Name = zo.ZoneName };
            viewModel.ZoneList = new SelectList(zones, "Id", "Name", string.Empty);

            return viewModel;
        }

        public bool ShowInstallmentSubGrid { get; internal set; }
        public bool ShowLegend { get; internal set; }
        public SelectList ZoneList { get; internal set; }
        public SelectList SalesmanList { get; internal set; }
        public SelectList TLSList { get; internal set; }
        public SelectList SurveyorList { get; internal set; }
        public SelectList CollectorList { get; internal set; }
    }
}
