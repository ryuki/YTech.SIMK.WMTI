using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Data.Repository;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.Master
{
    [HandleError]
    public class EmployeeController : Controller
    {
        public EmployeeController() : this(new MEmployeeRepository(), new RefAddressRepository(), new RefPersonRepository(),new MDepartmentRepository())
        {}
        private readonly IMEmployeeRepository _mEmployeeRepository;
        private readonly IRefAddressRepository _refAddressRepository;
        private readonly IRefPersonRepository _refPersonRepository;
        private readonly IMDepartmentRepository _mDepartmentRepository;
        public EmployeeController(IMEmployeeRepository mEmployeeRepository, IRefAddressRepository refAddressRepository, IRefPersonRepository refPersonRepository,IMDepartmentRepository mDepartmentRepository)
        {
            Check.Require(mEmployeeRepository != null, "mEmployeeRepository may not be null");
            Check.Require(refAddressRepository != null, "refAddressRepository may not be null");
            Check.Require(refPersonRepository != null, "refPersonRepository may not be null");
            Check.Require(mDepartmentRepository != null, "mDepartmentRepository may not be null");

            this._mEmployeeRepository = mEmployeeRepository;
            this._refAddressRepository = refAddressRepository;
            this._refPersonRepository = refPersonRepository;
            this._mDepartmentRepository = mDepartmentRepository;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var employees = _mEmployeeRepository.GetPagedEmployeeList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from employee in employees
                    select new
                    {
                        i = employee.Id.ToString(),
                        cell = new string[] {
                            employee.Id, 
                            employee.Id, 
                        employee.PersonId != null ?    employee.PersonId.PersonFirstName : null, 
                          employee.PersonId != null ?    employee.PersonId.PersonLastName : null, 
                         employee.EmployeeStatus, 
                          employee.PersonId != null ?    employee.PersonId.PersonGender : null, 
                        employee.DepartmentId != null?  employee.DepartmentId.Id:null, 
                        employee.DepartmentId != null?  employee.DepartmentId.DepartmentName:null,
                          employee.PersonId != null ?    employee.PersonId.PersonPob : null,  
                          employee.PersonId != null ?    employee.PersonId.PersonDob.Value.ToString(Helper.CommonHelper.DateFormat) : null, 
                          employee.PersonId != null ?    employee.PersonId.PersonMobile : null,  
                          employee.PersonId != null ?    employee.PersonId.PersonReligion : null,  
                          employee.AddressId != null ?    employee.AddressId.AddressLine1 : null,
                         employee.EmployeeDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public virtual ActionResult ListSearch(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var employees = _mEmployeeRepository.GetPagedEmployeeList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from employee in employees
                    select new
                    {
                        i = employee.Id.ToString(),
                        cell = new string[] {
                            employee.Id, 
                        employee.PersonId != null ?    employee.PersonId.PersonName : null, 
                        employee.PersonId != null ?    employee.PersonId.PersonGender : null,  
                        employee.DepartmentId != null?  employee.DepartmentId.DepartmentName:null,  
                         employee.EmployeeDesc
                        }
                    }).ToArray()
            }; 
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(MEmployee viewModel, FormCollection formCollection)
        { 
            MEmployee mEmployeeToInsert = new MEmployee();
            RefPerson person = new RefPerson();
            RefAddress address = new RefAddress(); 
            
            TransferFormValuesTo(person, formCollection);
            person.SetAssignedIdTo(Guid.NewGuid().ToString());
            person.CreatedDate = DateTime.Now;
            person.CreatedBy = User.Identity.Name;
            person.DataStatus = EnumDataStatus.New.ToString();
            _refPersonRepository.Save(person);

            TransferFormValuesTo(address, formCollection);
            address.SetAssignedIdTo(Guid.NewGuid().ToString());
            address.CreatedDate = DateTime.Now;
            address.CreatedBy = User.Identity.Name;
            address.DataStatus = EnumDataStatus.New.ToString();
            _refAddressRepository.Save(address); 
            
            TransferFormValuesTo(mEmployeeToInsert, viewModel);
            mEmployeeToInsert.DepartmentId = _mDepartmentRepository.Get(formCollection["DepartmentId"]);
            mEmployeeToInsert.SetAssignedIdTo(viewModel.Id);
            mEmployeeToInsert.CreatedDate = DateTime.Now;
            mEmployeeToInsert.CreatedBy = User.Identity.Name;
            mEmployeeToInsert.DataStatus = EnumDataStatus.New.ToString();

            mEmployeeToInsert.PersonId = person;
            mEmployeeToInsert.AddressId = address;
            _mEmployeeRepository.Save(mEmployeeToInsert);

            try
            {
                _mEmployeeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mEmployeeRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("Data Karyawan Berhasil Disimpan");
        }

        private void TransferFormValuesTo(RefAddress address, FormCollection formCollection)
        {
            address.AddressLine1 = formCollection["AddressLine1"];
        }

        private void TransferFormValuesTo(RefPerson person, FormCollection formCollection)
        {
            person.PersonFirstName = formCollection["PersonFirstName"];
            person.PersonLastName = formCollection["PersonLastName"];
            person.PersonGender = formCollection["PersonGender"];
            if (!string.IsNullOrEmpty(formCollection["PersonDob"]))
                person.PersonDob = Convert.ToDateTime(formCollection["PersonDob"]);
            person.PersonPob = formCollection["PersonPob"];
            person.PersonPhone = formCollection["PersonPhone"];
            person.PersonMobile = formCollection["PersonMobile"];
            person.PersonEmail = formCollection["PersonEmail"];
            person.PersonReligion = formCollection["PersonReligion"];
            person.PersonRace = formCollection["PersonRace"];
            person.PersonIdCardType = formCollection["PersonIdCardType"];
            person.PersonIdCardNo = formCollection["PersonIdCardNo"];
        }

        [Transaction]
        public ActionResult Delete(MEmployee viewModel, FormCollection formCollection)
        {
            MEmployee mEmployeeToDelete = _mEmployeeRepository.Get(viewModel.Id);

            if (mEmployeeToDelete != null)
            {
                _mEmployeeRepository.Delete(mEmployeeToDelete);
            }

            try
            {
                _mEmployeeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mEmployeeRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Karyawan Berhasil Dihapus");
        }

        [Transaction]
        public ActionResult Update(MEmployee viewModel, FormCollection formCollection)
        { 
            MEmployee mEmployeeToUpdate = _mEmployeeRepository.Get(viewModel.Id);
            TransferFormValuesTo(mEmployeeToUpdate, viewModel);
            mEmployeeToUpdate.DepartmentId = _mDepartmentRepository.Get(formCollection["DepartmentId"]);
            mEmployeeToUpdate.ModifiedDate = DateTime.Now;
            mEmployeeToUpdate.ModifiedBy = User.Identity.Name;
            mEmployeeToUpdate.DataStatus = EnumDataStatus.Updated.ToString();

            RefPerson person = mEmployeeToUpdate.PersonId;
            if (person == null)
            {
                person = new RefPerson();
                TransferFormValuesTo(person, formCollection);
                person.SetAssignedIdTo(Guid.NewGuid().ToString());
                person.CreatedDate = DateTime.Now;
                person.CreatedBy = User.Identity.Name;
                person.DataStatus = EnumDataStatus.New.ToString();
                _refPersonRepository.Save(person);

                mEmployeeToUpdate.PersonId = person;
            }
            else
            {
                TransferFormValuesTo(person, formCollection);
                person.ModifiedDate = DateTime.Now;
                person.ModifiedBy = User.Identity.Name;
                person.DataStatus = EnumDataStatus.Updated.ToString();
            }

            RefAddress address = mEmployeeToUpdate.AddressId;
            if (address == null)
            {
                address = new RefAddress();
                TransferFormValuesTo(address, formCollection);
                address.SetAssignedIdTo(Guid.NewGuid().ToString());
                address.CreatedDate = DateTime.Now;
                address.CreatedBy = User.Identity.Name;
                address.DataStatus = EnumDataStatus.New.ToString();
                _refPersonRepository.Save(person);

                mEmployeeToUpdate.AddressId = address;
            }
            else
            {
                TransferFormValuesTo(address, formCollection);
                address.ModifiedDate = DateTime.Now;
                address.ModifiedBy = User.Identity.Name;
                address.DataStatus = EnumDataStatus.Updated.ToString();
            }


            _mEmployeeRepository.Update(mEmployeeToUpdate);

            try
            {
                _mEmployeeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mEmployeeRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Karyawan Berhasil Diupdate");
        } 

        private void TransferFormValuesTo(MEmployee mEmployeeToUpdate, MEmployee mEmployeeFromForm)
        {
            mEmployeeToUpdate.DepartmentId = mEmployeeFromForm.DepartmentId;
            mEmployeeToUpdate.EmployeeStatus = mEmployeeFromForm.EmployeeStatus;
            mEmployeeToUpdate.EmployeeDesc = mEmployeeFromForm.EmployeeDesc;
        }


        [Transaction]
        public virtual ActionResult GetList()
        {
            var employees = _mEmployeeRepository.GetAll();
            StringBuilder sb = new StringBuilder();
            MEmployee mEmployee = new MEmployee();
            sb.AppendFormat("{0}:{1}", string.Empty, "-Pilih Karyawan-");
            for (int i = 0; i < employees.Count; i++)
            {
                mEmployee = employees[i];
                sb.AppendFormat(";{0}:{1} {2}", mEmployee.Id, mEmployee.PersonId.PersonFirstName, mEmployee.PersonId.PersonLastName);
            }
            return Content(sb.ToString());
        } 
    }
}
