using mtv_management_leave.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using mtv_management_leave.Lib.Extendsions;
using System.Web.Routing;

namespace mtv_management_leave.Controllers
{
    public class RolesController: Controller
    {
        private LeaveManagementContext _context;
        private ApplicationUserManager _userManager;

        public RolesController(LeaveManagementContext context, ApplicationUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Initializing(RequestContext requestContext)
        {
            Initialize(requestContext);
        }
                
        public IEnumerable<SelectListItem> RolesToList(List<int> selected)
        {
            var roleName = User.GetRoleName();

            List<SelectListItem> roles = _context.Roles
                                                .OrderByDescending(m => m.Id)
                                                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();


            if (!string.Equals("super admin", roleName, System.StringComparison.CurrentCultureIgnoreCase))
                roles.vRemove(m => string.Equals(m.Text, "super admin", System.StringComparison.CurrentCultureIgnoreCase));

            if (!selected.vIsEmpty())
            {
                foreach(var item in roles.Where(m=> selected.Contains(int.Parse( m.Value))))
                {
                    item.Selected = true;
                }
            }
            return roles;
        }
    }
}
