using System.Web.Mvc;

namespace YTech.SIMK.WMTI.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
