using System;
using System.Linq;
using System.Web.Mvc;
using SharpArch.Core;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Data.Repository;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.Master
{
    [HandleError]
    public class CommissionController : Controller
    {
        //public CommissionController()
        //    : this(new MCommissionRepository())
        //{ }

        private readonly IMCommissionRepository _mCommissionRepository;
        private readonly IMCommissionDetRepository _mCommissionDetRepository;

        public CommissionController(IMCommissionRepository mCommissionRepository, IMCommissionDetRepository mCommissionDetRepository)
        {
            Check.Require(mCommissionRepository != null, "mCommissionRepository may not be null");
            Check.Require(mCommissionDetRepository != null, "mCommissionDetRepository may not be null");

            this._mCommissionRepository = mCommissionRepository;
            this._mCommissionDetRepository = mCommissionDetRepository;
        }

        public ActionResult Index(string department)
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows, string department)
        {
            int totalRecords = 0;
            var commissions = _mCommissionRepository.GetPagedCommissionList(sidx, sord, page, rows, ref totalRecords, department);
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from commission in commissions
                    select new
                    {
                        id = commission.Id.ToString(),
                        cell = new string[] {
                            string.Empty,
                            Helper.CommonHelper.ConvertToString(commission.CommissionStartDate),
                            Helper.CommonHelper.ConvertToString(commission.CommissionEndDate),
                            Helper.CommonHelper.ConvertToString(commission.CommissionValue)
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Insert(MCommission viewModel, FormCollection formCollection, string department)
        {
            var commission = new MCommission();

            TransferFormValuesTo(commission, formCollection);
            commission.SetAssignedIdTo(Guid.NewGuid().ToString());
            commission.CommissionStatus = department;
            commission.CreatedDate = DateTime.Now;
            commission.CreatedBy = User.Identity.Name;
            commission.DataStatus = EnumDataStatus.New.ToString();
            _mCommissionRepository.Save(commission);

            try
            {
                _mCommissionRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCommissionRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Komisi berhasil dimasukkan");
        }

        [Transaction]
        public ActionResult Delete(MEmployee viewModel, FormCollection formCollection)
        {
            MCommission commission = _mCommissionRepository.Get(viewModel.Id);

            if (commission != null)
            {
                _mCommissionRepository.Delete(commission);
            }

            try
            {
                _mCommissionRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCommissionRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Komisi Berhasil Dihapus");
        }

         [Transaction]
        public ActionResult Update(MCommission viewModel, FormCollection formCollection)
        {

            MCommission commission = _mCommissionRepository.Get(viewModel.Id);

            TransferFormValuesTo(commission, formCollection);
            commission.ModifiedDate = DateTime.Now;
            commission.ModifiedBy = User.Identity.Name;
            commission.DataStatus = EnumDataStatus.Updated.ToString();

            _mCommissionRepository.Update(commission);

            try
            {
                _mCommissionRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mCommissionRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Komisi Berhasil Diupdate");
        }

        private static void TransferFormValuesTo(MCommission commission, FormCollection formCollection)
        {
            commission.CommissionStartDate = Helper.CommonHelper.ConvertToDate(formCollection["CommissionStartDate"]);
            commission.CommissionEndDate = Helper.CommonHelper.ConvertToDate(formCollection["CommissionEndDate"]);
            commission.CommissionValue = Helper.CommonHelper.ConvertToDecimal(formCollection["CommissionValue"]);
        }

        [Transaction]
        public ActionResult ListForSubGrid(string id)
        {
            var commissionDets = _mCommissionDetRepository.GetCommissionDetListById(id);

            var jsonData = new
            {
                rows = (
                    from commissionDet in commissionDets
                    select new
                    {
                        i = commissionDet.Id.ToString(),
                        cell = new string[]
                        {
                            commissionDet.DetailType,
                            Helper.CommonHelper.ConvertToString(commissionDet.DetailLowTarget),
                            Helper.CommonHelper.ConvertToString(commissionDet.DetailHighTarget),
                            Helper.CommonHelper.ConvertToString(commissionDet.DetailValue)
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
