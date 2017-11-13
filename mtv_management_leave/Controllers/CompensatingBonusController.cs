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
using mtv_management_leave.Lib;

namespace mtv_management_leave.Controllers
{
    public class CompensatingBonusController : Controller
    {
        private AddLeaveBase _leaveBase;

        public CompensatingBonusController(AddLeaveBase leaveBase)
        {
            _leaveBase = leaveBase;
        }
        public ActionResult Index()
        {
            return View(new Models.LeaveBonus.SearchRequest());
        }

        public PartialViewResult RegisterCompensatingBonus()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult RegisterLeaveBonus(AddLeave registerLeaveBonusRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    registerLeaveBonusRequest.Status = Common.StatusLeave.E_Register.ToString();
                    registerLeaveBonusRequest.Type = Common.AddLeaveType.E_Compensating.ToString();
                    registerLeaveBonusRequest.Uid = int.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    _leaveBase.SaveAddLeaveBonus(registerLeaveBonusRequest);
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 400;
                    return Json(new { status = 400, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Status = 0, Message = "Action complete" });
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
                    var lstUid = new List<int>();
                    if (model.Uid != null)
                    {
                        lstUid.Add(model.Uid.Value);
                    }
                    else
                    {
                        lstUid.Add(int.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId()));
                    }
                    result = _leaveBase.GetAddLeaveBonus(model.DateStart.Value, model.DateEnd.Value, lstUid);
                    result = result.Where(m => m.Type == Common.AddLeaveType.E_Compensating.ToString()).ToList();
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
                Response.StatusCode = 400;
                return Json(new { status = 400, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
