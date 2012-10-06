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
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Data.Repository;
using YTech.SIMK.WMTI.Enums;
using YTech.SIMK.WMTI.Web.Controllers.ViewModel;

namespace YTech.SIMK.WMTI.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IMEmployeeRepository _mEmployeeRepository;
        private readonly IRefAddressRepository _refAddressRepository;
        private readonly IRefPersonRepository _refPersonRepository;
        private readonly IMDepartmentRepository _mDepartmentRepository;
        private readonly ITNewsRepository _tNewsRepository;
        private readonly ITLoanRepository _tLoanRepository;
        public HomeController(IMEmployeeRepository mEmployeeRepository, IRefAddressRepository refAddressRepository, IRefPersonRepository refPersonRepository, IMDepartmentRepository mDepartmentRepository, ITNewsRepository tNewsRepository, ITLoanRepository tLoanRepository)
        {
            Check.Require(mEmployeeRepository != null, "mEmployeeRepository may not be null");
            Check.Require(refAddressRepository != null, "refAddressRepository may not be null");
            Check.Require(refPersonRepository != null, "refPersonRepository may not be null");
            Check.Require(mDepartmentRepository != null, "mDepartmentRepository may not be null");
            Check.Require(tNewsRepository != null, "tNewsRepository may not be null");
            Check.Require(tLoanRepository != null, "tLoanRepository may not be null");

            this._mEmployeeRepository = mEmployeeRepository;
            this._refAddressRepository = refAddressRepository;
            this._refPersonRepository = refPersonRepository;
            this._mDepartmentRepository = mDepartmentRepository;
            this._tNewsRepository = tNewsRepository;
            this._tLoanRepository = tLoanRepository;
        }

        public ActionResult Index()
        {
            HomeViewModel viewModel = HomeViewModel.Create(_tNewsRepository, _tLoanRepository);
            return View(viewModel);
        }

        public ActionResult Calculate()
        {
            return View();
        }

        [Transaction]
        [ValidateInput(false)] 
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SavePromo(string promoNews)
        {
            return SaveNews(EnumNewsType.Promo, promoNews);

        }

        [Transaction]
        [ValidateInput(false)] 
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAnnouncement(string announcementNews)
        {
            return SaveNews(EnumNewsType.Announcement, announcementNews);

        }

        public ActionResult SaveNews(EnumNewsType newsType, string newsDesc)
        {
            string Message = string.Empty;
            bool Success = true;
            try
            {
                TNews news = _tNewsRepository.GetByType(newsType);
                if (news == null)
                {
                    news = new TNews();
                    news.SetAssignedIdTo(Guid.NewGuid().ToString());
                    news.NewsDesc = newsDesc;
                    news.NewsType = newsType.ToString();
                    news.DataStatus = EnumDataStatus.New.ToString();
                    news.CreatedBy = User.Identity.Name;
                    news.CreatedDate = DateTime.Now;
                    _tNewsRepository.Save(news);
                }
                else
                {
                    news.NewsDesc = newsDesc;
                    news.DataStatus = EnumDataStatus.Updated.ToString();
                    news.ModifiedBy = User.Identity.Name;
                    news.ModifiedDate = DateTime.Now;
                    _tNewsRepository.Update(news);
                }

                Success = true;
                Message = "Sukses";
            }
            catch (Exception ex)
            {
                Success = false;
                Message = "Error :\n" + ex.GetBaseException().Message;
            }
            var e = new
            {
                Success,
                Message
            };
            return Json(e, JsonRequestBehavior.AllowGet);

        }
    }
}
