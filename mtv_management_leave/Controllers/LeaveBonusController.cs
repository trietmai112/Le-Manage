using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.RegisterLeave;
using System.Linq;
using mtv_management_leave.Models.Request;
using mtv_management_leave.Models.Response;

namespace mtv_management_leave.Controllers
{
    [Authorize(Roles = "Super admin, Admin")]
    public class LeaveBonusController : Controller
    {
        private AddLeaveBase _leaveBase;

        public LeaveBonusController(AddLeaveBase leaveBase)
        {
            _leaveBase = leaveBase;
        }
        [Authorize(Roles= "Super admin, Admin")]
        public ActionResult Index()
        {
            return View(new Models.LeaveBonus.SearchRequest());
        }

        public PartialViewResult RegisterLeaveBonus()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult RegisterLeaveBonus(AddLeave registerLeaveBonusRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _leaveBase.SaveAddLeaveBonus(registerLeaveBonusRequest);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ex.Message);
                }
            }
            return PartialView(registerLeaveBonusRequest);
        }

        [HttpPost]
        public JsonResult Delete(ToogleApprove model)
        {
            try
            {
                _leaveBase.DeleteAddLeaveBonus(model.Ids);
                return Json(new { Status = 0, Message = "Action complete" });
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Json(new { status = 400, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, AllowAnonymous]
        public JsonResult ToList(Models.LeaveBonus.SearchRequest model)
        {
            try
            {

                List<ResponseLeaveBonus> result = new List<ResponseLeaveBonus>();

                if (model.DateStart != null && model.DateEnd != null)
                {
                    if (model.Uids != null && model.Uids.Count == 1 && model.Uids[0] == 0)
                    {
                        model.Uids = null;
                    }
                    result = _leaveBase.GetAddLeaveBonus(model.DateStart.Value, model.DateEnd.Value, model.Uids);
                }
                var resultJson = Json(new Lib.Repository.BootGridReponse<ResponseLeaveBonus>
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
