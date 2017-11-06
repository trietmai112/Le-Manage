using System;
using System.Collections.Generic;
using System.Web.Mvc;
using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.RegisterLeave;
using mtv_management_leave.Models.Response;
using System.Linq;
using mtv_management_leave.Models.Request;
using System.Net;

namespace mtv_management_leave.Controllers
{
    public class LeaveManagementController : Controller
    {
        private LeaveBase _leaveBase;

        public LeaveManagementController(LeaveBase leaveBase)
        {
            _leaveBase = leaveBase;
        }

        [Authorize(Roles = "Super admin, Admin")]
        public ActionResult Index(int? uid)
        {
            return View();
        }

        [HttpPost]
        public JsonResult Approve(ToogleApprove model)
        {
            _leaveBase.ApproveLeave(model.Ids);
            return Json(new { Status = 0, Message = "Action complete" });
        }

        [HttpPost]
        public JsonResult DisApprove(ToogleApprove model)
        {
            _leaveBase.RejectLeave(model.Ids);
            return Json(new { Status = 0, Message = "Action complete" });
        }

        [HttpPost]
        public JsonResult ToList(SearchRequest model)
        {
            try
            {
                if(model.Uids!= null && model.Uids.Count()==1 && model.Uids[0] == 0)
                {
                    model.Uids = null;
                }

                var result = _leaveBase.GetLeave(model.DateStart, model.DateEnd, model.Uids);

                return Json(new BootGridReponse<ResponseLeave>
                {
                    current = 1,
                    total = result.Count,
                    rowCount = -1,
                    rows = result
                });
            }
            catch (Exception e)
            {
                return Json(new { Status = (int)HttpStatusCode.BadRequest, Message = e.Message });
            }
        }
    }
}
