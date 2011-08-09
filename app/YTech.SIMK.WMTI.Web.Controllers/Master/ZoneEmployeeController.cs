using System;
using System.Linq;
using System.Web.Mvc;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Data.Repository;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.Master
{
    [HandleError]
    public class ZoneEmployeeController : Controller
    {
        public ZoneEmployeeController()
            : this(new MZoneEmployeeRepository(), new MEmployeeRepository(), new MZoneRepository())
        { }

        private readonly IMZoneEmployeeRepository _mZoneEmployeeRepository;
        private readonly IMEmployeeRepository _mEmployeeRepository;
        private readonly IMZoneRepository _mZoneRepository;

        public ZoneEmployeeController(IMZoneEmployeeRepository mZoneEmployeeRepository, IMEmployeeRepository mEmployeeRepository, IMZoneRepository mZoneRepository)
        {
            Check.Require(mZoneEmployeeRepository != null, "mZoneEmployeeRepository may not be null");
            Check.Require(mEmployeeRepository != null, "mEmployeeRepository may not be null");
            Check.Require(mZoneRepository != null, "mZoneRepository may not be null");

            this._mZoneEmployeeRepository = mZoneEmployeeRepository;
            this._mEmployeeRepository = mEmployeeRepository;
            this._mZoneRepository = mZoneRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var zoneEmployees = _mZoneEmployeeRepository.GetPagedZoneEmployeeList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from zoneEmployee in zoneEmployees
                    select new
                    {
                        id = zoneEmployee.Id.ToString(),
                        cell = new string[] {
                            zoneEmployee.Id, 
                            zoneEmployee.EmployeeId.Id,
                            zoneEmployee.EmployeeId.PersonId.PersonFirstName,
                            Helper.CommonHelper.ConvertToString(zoneEmployee.StartDate),
                            Helper.CommonHelper.ConvertToString(zoneEmployee.EndDate),
                            zoneEmployee.ZoneId.Id,
                            zoneEmployee.ZoneId.ZoneName
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(MZoneEmployee viewModel, FormCollection formCollection)
        {
            var zoneEmployee = new MZoneEmployee();

            zoneEmployee.EmployeeId = _mEmployeeRepository.Get(formCollection["EmployeeId"]);

            zoneEmployee.StartDate = Helper.CommonHelper.ConvertToDate(formCollection["StartDate"]);
            zoneEmployee.EndDate = Helper.CommonHelper.ConvertToDate(formCollection["EndDate"]);

            zoneEmployee.SetAssignedIdTo(Guid.NewGuid().ToString());
            zoneEmployee.CreatedDate = DateTime.Now;
            zoneEmployee.CreatedBy = User.Identity.Name;
            zoneEmployee.DataStatus = EnumDataStatus.New.ToString();

            zoneEmployee.ZoneId = _mZoneRepository.Get(formCollection["ZoneId"]);

            _mZoneEmployeeRepository.Save(zoneEmployee);

            try
            {
                _mZoneEmployeeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mZoneEmployeeRepository.DbContext.RollbackTransaction();

                //throw e.GetBaseException();
                return Content(e.GetBaseException().Message);
            }

            return Content("Data Pembagian Wilayah Kerja Berhasil Disimpan");
        }

        [Transaction]
        public ActionResult Delete(MZoneEmployee viewModel, FormCollection formCollection)
        {
            MZoneEmployee zoneEmployee = _mZoneEmployeeRepository.Get(viewModel.Id);

            if (zoneEmployee != null)
            {
                _mZoneEmployeeRepository.Delete(zoneEmployee);
            }

            try
            {
                _mZoneEmployeeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mZoneEmployeeRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Pembagian Wilayah Kerja Berhasil Dihapus");
        }

        [Transaction]
        public ActionResult Update(MZoneEmployee viewModel, FormCollection formCollection)
        {
            MZoneEmployee zoneEmployee = _mZoneEmployeeRepository.Get(viewModel.Id);

            zoneEmployee.EmployeeId = _mEmployeeRepository.Get(formCollection["EmployeeId"]);

            zoneEmployee.StartDate = Helper.CommonHelper.ConvertToDate(formCollection["StartDate"]);
            zoneEmployee.EndDate = Helper.CommonHelper.ConvertToDate(formCollection["EndDate"]);

            zoneEmployee.ZoneId = _mZoneRepository.Get(formCollection["ZoneId"]);

            zoneEmployee.ModifiedDate = DateTime.Now;
            zoneEmployee.ModifiedBy = User.Identity.Name;
            zoneEmployee.DataStatus = EnumDataStatus.Updated.ToString();

            _mZoneEmployeeRepository.Update(zoneEmployee);

            try
            {
                _mZoneEmployeeRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mZoneEmployeeRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Pembagian Wilayah Kerja Berhasil Diupdate");
        }
    }
}
