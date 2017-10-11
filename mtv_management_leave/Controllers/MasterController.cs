using mtv_management_leave.Lib.Extendsions;
using System.Web.Mvc;

namespace mtv_management_leave.Controllers
{
    public class MasterController:Controller
    {        
        public ActionResult LeftSidebar()
        {
            return PartialView("_LeftSidebar", model: User.GetRoleName());
        }
    }
}
