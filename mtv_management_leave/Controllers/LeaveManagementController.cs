using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.RegisterLeave;
using mtv_management_leave.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace mtv_management_leave.Controllers
{
    public class LeaveManagementController: Controller
    {
        private LeaveBase _leaveBase;

        public LeaveManagementController(LeaveBase leaveBase)
        {
            _leaveBase = leaveBase;
        }
        public ActionResult Index(int? uid)
        {
            return View(new mtv_management_leave.Models.RegisterLeave.SearchRequest {
                Uid = uid
            });
        }

        [HttpPost]
        public JsonResult ToList( SearchRequest  model)
        {
            var result = model.Uid.HasValue ?
               _leaveBase.GetLeave(model.DateStart, model.DateEnd, model.Uid.Value) :
               _leaveBase.GetLeave(model.DateStart, model.DateEnd);

            result.vAdd(new ResponseLeave
            {
                FullName = "123",
                LeaveStatus = Lib.Common.StatusLeave.E_Approve,
                LeaveFrom = DateTime.Now.ToShortDateString(),
                LeaveTo = DateTime.Now.ToShortDateString(),
                LeaveTypeName = "Year",
                RegisterHour = 10,
                Uid = 1
            });
            return Json(new BootGridReponse<ResponseLeave> {
                current = 1,
                total = result.Count,
                rowCount = -1,
                rows = result
            });
        }
    }
}
