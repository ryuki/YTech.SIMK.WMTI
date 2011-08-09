using System;
using System.Linq;
using System.Text;
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
    public class ZoneController : Controller
    {
        public ZoneController()
            : this(new MZoneRepository())
        { }

        private readonly IMZoneRepository _mZoneRepository;
        public ZoneController(IMZoneRepository mZoneRepository)
        {
            Check.Require(mZoneRepository != null, "mZoneRepository may not be null");

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
            var zones = _mZoneRepository.GetPagedZoneList(sidx, sord, page, rows, ref totalRecords);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from zone in zones
                    select new
                    {
                        id = zone.Id.ToString(),
                        cell = new string[] {
                            zone.Id, 
                            zone.ZoneName, 
                            zone.ZoneCity,
                            zone.ZoneDesc
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(MZone viewModel, FormCollection formCollection)
        {
            MZone zone = new MZone();

            TransferFormValuesTo(zone, viewModel);
            zone.SetAssignedIdTo(viewModel.Id);
            zone.CreatedDate = DateTime.Now;
            zone.CreatedBy = User.Identity.Name;
            zone.DataStatus = EnumDataStatus.New.ToString();

            _mZoneRepository.Save(zone);

            try
            {
                _mZoneRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mZoneRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Delete(MZone viewModel, FormCollection formCollection)
        {
            MZone mZone = _mZoneRepository.Get(viewModel.Id);

            if (mZone != null)
            {
                _mZoneRepository.Delete(mZone);
            }

            try
            {
                _mZoneRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mZoneRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MZone viewModel, FormCollection formCollection)
        {
            MZone mZone = _mZoneRepository.Get(viewModel.Id);
            TransferFormValuesTo(mZone, viewModel);
            mZone.ModifiedDate = DateTime.Now;
            mZone.ModifiedBy = User.Identity.Name;
            mZone.DataStatus = EnumDataStatus.Updated.ToString();
            _mZoneRepository.Update(mZone);

            try
            {
                _mZoneRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mZoneRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        private void TransferFormValuesTo(MZone zone, MZone viewModel)
        {
            zone.ZoneName = viewModel.ZoneName;
            zone.ZoneCity = viewModel.ZoneCity;
            zone.ZoneDesc = viewModel.ZoneDesc;
            zone.ZoneStatus = viewModel.ZoneStatus;
        }

        [Transaction]
        public virtual ActionResult GetList()
        {
            var zones = _mZoneRepository.GetAll();
            StringBuilder sb = new StringBuilder();
            MZone mZone = new MZone();
            sb.AppendFormat("{0}:{1}", string.Empty, "-Pilih Wilayah-");
            for (int i = 0; i < zones.Count; i++)
            {
                mZone = zones[i];
                sb.AppendFormat(";{0}:{1}", mZone.Id, mZone.ZoneName);
            }
            return Content(sb.ToString());
        }
    }
}
