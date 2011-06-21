using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace YTech.SIMK.WMTI.Web.Controllers.Transaction
{
    [HandleError]
    public class SurveyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
