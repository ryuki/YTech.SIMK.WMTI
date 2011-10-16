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

namespace YTech.SIMK.WMTI.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IMEmployeeRepository _mEmployeeRepository;
        private readonly IRefAddressRepository _refAddressRepository;
        private readonly IRefPersonRepository _refPersonRepository;
        private readonly IMDepartmentRepository _mDepartmentRepository;
        public HomeController(IMEmployeeRepository mEmployeeRepository, IRefAddressRepository refAddressRepository, IRefPersonRepository refPersonRepository, IMDepartmentRepository mDepartmentRepository)
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

        public ActionResult Calculate()
        {
            return View();
        }
    }
}
