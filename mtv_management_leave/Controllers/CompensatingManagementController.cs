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
using mtv_management_leave.Lib;

namespace mtv_management_leave.Controllers
{
    [Authorize(Roles = "Super admin, Admin")]
    public class CompensatingManagementController : ControllerExtendsion
    {
        private AddLeaveBase _addLeaveBase;

        public CompensatingManagementController(AddLeaveBase addLeaveBase)
        {
            _addLeaveBase = addLeaveBase;
        }

        [Authorize(Roles = "Super admin, Admin")]
        public ActionResult Index(int? uid)
        {
            return View();
        }

        [HttpPost]
        public JsonResult Approve(ToogleApprove model)
        {
            try
            {
                _addLeaveBase.ApproveAddLeaveBonus(model.Ids);
                return Json(new { Status = 0, Message = "Action complete" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public JsonResult DisApprove(ToogleApprove model)
        {
            try
            {
                _addLeaveBase.RejectAddLeaveBonus(model.Ids);
                return Json(new { Status = 0, Message = "Action complete" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public JsonResult ToList(SearchRequest model)
        {
            try
            {
                if (model.Uids != null && model.Uids.Count() == 1 && model.Uids[0] == 0)
                {
                    model.Uids = null;
                }

                var result = _addLeaveBase.GetAddLeaveBonus(model.DateStart, model.DateEnd, model.Uids);
                result = result.Where(m => m.Type == Common.AddLeaveType.E_Compensating.ToString()).ToList();
                return Json(new BootGridReponse<ResponseLeaveBonus>
                {
                    current = 1,
                    total = result.Count,
                    rowCount = -1,
                    rows = result
                });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
