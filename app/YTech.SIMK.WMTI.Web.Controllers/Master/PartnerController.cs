using System;
using System.Linq;
using System.Web.Mvc;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.Master
{
    [HandleError]
    public class PartnerController : Controller
    {
        private readonly IMPartnerRepository _mPartnerRepository;
        private readonly IRefAddressRepository _refAddressRepository;

        public PartnerController(IMPartnerRepository mPartnerRepository, IRefAddressRepository refAddressRepository)
        {
            Check.Require(mPartnerRepository != null, "mPartnerRepository may not be null");
            Check.Require(refAddressRepository != null, "refAddressRepository may not be null");

            _mPartnerRepository = mPartnerRepository;
            _refAddressRepository = refAddressRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Transaction]
        public virtual ActionResult List(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            var partners = _mPartnerRepository.GetPagedPartnerList(sidx, sord, page, rows, ref totalRecords);

            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from partner in partners
                    select new
                    {
                        i = partner.Id,
                        cell = new string[]
                                                {
                                                partner.Id,
                                                partner.PartnerName,
                                                partner.AddressId != null ? partner.AddressId.AddressLine1 : null,
                                                partner.PartnerDesc
                                                }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult Insert(MPartner partnerVM, RefAddress addressVM, MPartner viewModel, FormCollection formCollection)
        {
            //bool isSave = true;
            //MPartner mCompanyToInsert = new MPartner();
            //TransferFormValuesTo(mCompanyToInsert, viewModel);
            MPartner partner = new MPartner();
            RefAddress address = new RefAddress();

            //save address
            address.AddressLine1 = addressVM.AddressLine1;

            address.SetAssignedIdTo(Guid.NewGuid().ToString());
            address.CreatedDate = DateTime.Now;
            address.CreatedBy = User.Identity.Name;
            address.DataStatus = EnumDataStatus.New.ToString();
            _refAddressRepository.Save(address);

            //save partner
            partner.AddressId = address;

            partner.PartnerName = partnerVM.PartnerName;
            partner.PartnerDesc = partnerVM.PartnerDesc;
            partner.PartnerStatus = partnerVM.PartnerStatus;

            partner.SetAssignedIdTo(viewModel.Id);
            partner.CreatedDate = DateTime.Now;
            partner.CreatedBy = User.Identity.Name;
            partner.DataStatus = EnumDataStatus.New.ToString();

            _mPartnerRepository.Save(partner);

            try
            {
                _mPartnerRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {
                _mPartnerRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("success");
        }

        [Transaction]
        public ActionResult Update(MPartner viewModel, RefAddress addressVm, FormCollection formCollection)
        {
            try
            {
                _mPartnerRepository.DbContext.BeginTransaction();

                MPartner partner = _mPartnerRepository.Get(viewModel.Id);
                
                partner.ModifiedDate = DateTime.Now;
                partner.ModifiedBy = User.Identity.Name;
                partner.DataStatus = EnumDataStatus.Updated.ToString();

                RefAddress address = partner.AddressId;

                if (address == null)
                {
                    address = new RefAddress();

                    partner.AddressId = address;

                    address.AddressLine1 = addressVm.AddressLine1;
                    address.SetAssignedIdTo(Guid.NewGuid().ToString());
                    address.CreatedDate = DateTime.Now;
                    address.CreatedBy = User.Identity.Name;
                    address.DataStatus = EnumDataStatus.New.ToString();
                    _refAddressRepository.Save(address);
                }
                else
                {
                    address.AddressLine1 = addressVm.AddressLine1;
                    address.ModifiedDate = DateTime.Now;
                    address.ModifiedBy = User.Identity.Name;
                    address.DataStatus = EnumDataStatus.Updated.ToString();
                    _refAddressRepository.Update(address);
                }

                _mPartnerRepository.Update(partner);

                _mPartnerRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {
                _mPartnerRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Toko Berhasil Diupdate");
        }

        [Transaction]
        public ActionResult Delete(MPartner viewModel, FormCollection formCollection)
        {
            MPartner partner = _mPartnerRepository.Get(viewModel.Id);

            if (partner != null)
            {
                _mPartnerRepository.Delete(partner);
            }

            try
            {
                _mPartnerRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {

                _mPartnerRepository.DbContext.RollbackTransaction();

                return Content(e.GetBaseException().Message);
            }

            return Content("Data Toko Berhasil Dihapus");
        }
    }
}
