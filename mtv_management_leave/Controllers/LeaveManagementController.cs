﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.RegisterLeave;
using mtv_management_leave.Models.Response;
using System.Linq;
using mtv_management_leave.Models.Request;

namespace mtv_management_leave.Controllers
{
    public class LeaveManagementController : Controller
    {
        private LeaveBase _leaveBase;

        public LeaveManagementController(LeaveBase leaveBase)
        {
            _leaveBase = leaveBase;
        }
        public ActionResult Index(int? uid)
        {
            return View(new mtv_management_leave.Models.RegisterLeave.SearchRequest
            {
                Uid = uid
            });
        }

        [HttpPost]
        public JsonResult Approve(ToogleApprove model)
        {
            _leaveBase.ApproveLeave(model.Uids);
            return Json(new { Status = 0, Message = "Action complete" });
        }

        [HttpPost]
        public JsonResult DisApprove(ToogleApprove model)
        {
            _leaveBase.RejectLeave(model.Uids);
            return Json(new { Status = 0, Message = "Action complete" });
        }

        [HttpPost]
        public JsonResult ToList(SearchRequest model)
        {
            var result = _leaveBase.GetLeave(model.DateStart, model.DateEnd, model.Uids);

            return Json(new BootGridReponse<ResponseLeave>
            {
                current = 1,
                total = result.Count,
                rowCount = -1,
                rows = result
            });
        }
    }
}
