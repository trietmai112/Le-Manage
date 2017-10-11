using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace mtv_management_leave.Controllers
{
    public class MasterController:Controller
    {
        public PartialViewResult LeftSidebar()
        {
            return PartialView("_LeftSidebar", model: (User.IsInRole("Admin") || User.IsInRole("Super admin") ? "admin" : "user"));
        }
    }
}
