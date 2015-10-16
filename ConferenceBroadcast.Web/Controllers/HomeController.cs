using System.Web.Mvc;

namespace ConferenceBroadcast.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}