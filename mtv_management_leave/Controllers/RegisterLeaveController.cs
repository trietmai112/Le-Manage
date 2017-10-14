using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.RegisterLeave;
using System.Linq;

namespace mtv_management_leave.Controllers
{

    public class RegisterLeaveController : Controller
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
            return View(new Models.RegisterLeave.SearchRequest());
        }

        public PartialViewResult RegisterLeave(int? userId)
        {
            return PartialView(new Models.RegisterLeave.RegisterLeaveRequest { Uid = userId ?? User.Identity.GetUserId<int>() });
        }

        [HttpPost]
        public PartialViewResult RegisterLeave(RegisterLeaveRequest registerLeaveRequest)
        {
            Models.Entity.RegisterLeave registerLeave = Mapper.Map<RegisterLeave>(registerLeaveRequest);
            if (ModelState.IsValid)
            {
                try
                {
                    _leaveBase.RegisterLeave(registerLeave);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ex.Message);
                }
            }
            return PartialView(registerLeaveRequest);
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
                ModelState.AddModelError("Error", ex.Message);
                return null;
            }
        }
    }
}
