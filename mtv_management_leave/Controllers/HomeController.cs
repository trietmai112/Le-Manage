using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using zkemkeeper;
namespace mtv_management_leave.Controllers
{
    public class HomeController : Controller
    {
        private int dwMachineNumber = 1;
        private string dwEnrollNumber;
        private string name;
        private string password;
        private int privilege;
        private bool enabled;
        private int dwVerifyMode;
        private int dwInOutMode;
        private int year = 2017;
        private int month = 9;
        private int day = 27;
        private int h;
        private int m;
        private int s;
        private int dwWorkCode;
        public zkemkeeper.CZKEMClass driver = new zkemkeeper.CZKEMClass();
        public ActionResult Index()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var isConnect = driver.Connect_Net("10.10.10.20", 4370);
            string content = "";
            while (driver.SSR_GetAllUserInfo(dwMachineNumber, out dwEnrollNumber, out name, out password, out privilege, out enabled))
            {
                content += (string.Format("{0}\t| {1}\t\t\t| {2}\t| {3}\t| {4}", dwEnrollNumber, name, password, privilege, enabled)) +  "</br>";
            }
            driver.Disconnect();
            return Content(content);
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