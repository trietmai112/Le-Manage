using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.Request;
using mtv_management_leave.Models.Response;

namespace mtv_management_leave.Controllers
{
    [Authorize(Roles ="Admin,Super admin")]
    public class ChangeInOutManagementController : ControllerExtendsion
    {
        private RequestChangeInoutBase _inout;

        public ChangeInOutManagementController(RequestChangeInoutBase inout)
        {
            _inout = inout;
        }
        public ActionResult Index()
        {
            return View(new Models.LeaveBonus.SearchRequest());
        }


        [HttpPost]
        public JsonResult Approve(ToogleApprove model)
        {
            _inout.ApproveRequestChange(model.Ids);
            return Json(new { Status = 0, Message = "Action complete" });
        }

        [HttpPost]
        public JsonResult Reject(ToogleApprove model)
        {
            _inout.RejectRequestChange(model.Ids);
            return Json(new { Status = 0, Message = "Action complete" });
        }

        [HttpPost, AllowAnonymous]
        public JsonResult ToList(Models.LeaveBonus.SearchRequest model)
        {
            try
            {

                List<ResponseChangeInout> result = new List<ResponseChangeInout>();

                if (model.DateStart != null && model.DateEnd != null)
                {
                    if(model.Uids!= null && model.Uids.Count== 1 && model.Uids[0] ==0)
                    {
                        model.Uids = null;
                    }
                    result = _inout.GetRequestChangeInout(model.DateStart.Value, model.DateEnd.Value, model.Uids);
                }
                var resultJson = Json(new Lib.Repository.BootGridReponse<ResponseChangeInout>
                {
                    current = 1,
                    rowCount = -1,
                    total = result.Count,
                    rows = result
                });
                return resultJson;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return null;
            }
        }
    }
}
