﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.RegisterLeave;
using System.Linq;
using mtv_management_leave.Models.Request;

namespace mtv_management_leave.Controllers
{

    public class RegisterLeaveController : ControllerExtendsion
    {
        private InOutBase _inOutBase;
        private LeaveBase _leaveBase;

        public RegisterLeaveController(InOutBase inOutBase, LeaveBase leaveBase)
        {
            _inOutBase = inOutBase;
            _leaveBase = leaveBase;
        }
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult RegisterLeave(int? userId)
        {
            var now = DateTime.Now;
            return PartialView(new Models.RegisterLeave.RegisterLeaveRequest {
                Uid = userId ?? User.Identity.GetUserId<int>(),
                DateStart = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0),
                DateStart_Time = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0),
                DateEnd = new DateTime(now.Year, now.Month, now.Day, 17, 0, 0),
                DateEnd_Time = new DateTime(now.Year, now.Month, now.Day, 17, 0, 0)
                
            });
        }

        [HttpPost]
        public JsonResult RegisterLeave(RegisterLeaveRequest registerLeaveRequest)
        {
            Models.Entity.RegisterLeave registerLeave = Mapper.Map<RegisterLeave>(registerLeaveRequest);
            if (ModelState.IsValid)
            {
                try
                {
                    registerLeave.DateStart = registerLeave.DateStart.Date.AddHours(registerLeaveRequest.DateStart_Time.Hour).AddMinutes(registerLeaveRequest.DateStart_Time.Minute);
                    registerLeave.DateEnd = registerLeave.DateEnd.Date.AddHours(registerLeaveRequest.DateEnd_Time.Hour).AddMinutes(registerLeaveRequest.DateEnd_Time.Minute);
                    _leaveBase.RegisterLeave(registerLeave);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            return Json(new { Status = 0, Message = "Action complete" });
        }

        [HttpPost]
        public JsonResult Delete(ToogleApprove model)
        {
            try
            {
                _leaveBase.DeleteLeave(model.Ids);
                return Json(new { Status = 0, Message = "Action complete" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost, AllowAnonymous]
        public JsonResult ToList(Models.RegisterLeave.SearchRequest model)
        {

            try
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
                var result = _leaveBase.GetLeave(model.DateStart, model.DateEnd, lstUid);
                var resultJson = Json(new Lib.Repository.BootGridReponse<Models.Response.ResponseLeave>
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
                return BadRequest(ex);
            }
        }
    }
}
