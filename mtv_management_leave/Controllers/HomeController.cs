using mtv_management_leave.Lib.Repository;
using System.Web.Mvc;

namespace mtv_management_leave.Controllers
{
    [Authorize]
    public class HomeController : ControllerExtendsion
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}