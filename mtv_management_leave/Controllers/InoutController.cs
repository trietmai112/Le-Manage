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
using mtv_management_leave.Models;

namespace mtv_management_leave.Controllers
{

    public class InOutController : ControllerExtendsion
    {
        private InOutBase _inOutBase;
        private LeaveBase _leaveBase;

        public InOutController(InOutBase inOutBase, LeaveBase leaveBase)
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
                var result = _inOutBase.MappingInoutLeave(model.DateStart, model.DateEnd, lstUid);
                var resultJson = Json(new Lib.Repository.BootGridReponse<RepoMappingInOut>
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

                return this.BadRequest(ex);
            }
        }
    }
}
