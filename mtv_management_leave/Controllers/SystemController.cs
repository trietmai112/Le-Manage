using mtv_management_leave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace mtv_management_leave.Controllers
{
    public class SystemController: Controller
    {
        private LeaveManagementContext _context;

        public SystemController(LeaveManagementContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> LeaveTypeToDropdown()
        {
            var result = _context.MasterLeaveTypes.Select(m => new SelectListItem {
                Text = m.Name,
                Value = m.Id.ToString()            
            }).ToList();
            return result;
        }
    }
}
