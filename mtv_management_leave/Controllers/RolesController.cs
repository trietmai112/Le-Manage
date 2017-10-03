using mtv_management_leave.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace mtv_management_leave.Controllers
{
    public class RolesController: Controller
    {
        private LeaveManagementContext _context;

        public RolesController(LeaveManagementContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> RolesToList()
        {
            return _context.Roles
                .OrderByDescending(m=> m.Id)
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
        }
    }
}
