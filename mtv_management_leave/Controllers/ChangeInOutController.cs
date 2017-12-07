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
  
    public class ChangeInOutController : ControllerExtendsion
    {
        private RequestChangeInoutBase _inout;

        public ChangeInOutController(RequestChangeInoutBase inout)
        {
            _inout = inout;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View(new Models.LeaveBonus.SearchRequest());
        }

        public PartialViewResult RegisterChangeInout()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult RegisterChangeInout(RequestChangeInout rqChangeInout)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    rqChangeInout.Uid = int.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    if (rqChangeInout.Intime != null)
                        rqChangeInout.Intime = rqChangeInout.Date.Date.AddHours(rqChangeInout.Intime.Value.Hour).AddMinutes(rqChangeInout.Intime.Value.Minute);
                    if (rqChangeInout.OutTime != null)
                        rqChangeInout.OutTime = rqChangeInout.Date.Date.AddHours(rqChangeInout.OutTime.Value.Hour).AddMinutes(rqChangeInout.OutTime.Value.Minute);
                    _inout.SaveRequestChange(rqChangeInout);
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
                _inout.DeleteRequestChange(model.Ids);
                return Json(new { Status = 0, Message = "Action complete" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost, AllowAnonymous]
        public JsonResult ToList(Models.LeaveBonus.SearchRequest model)
        {
            try
            {

                List<ResponseChangeInout> result = new List<ResponseChangeInout>();

                if (model.DateStart != null && model.DateEnd != null)
                {
                    var uid = int.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    result = _inout.GetRequestChangeInout(model.DateStart.Value, model.DateEnd.Value, new List<int>() { uid });
                    result = result.OrderBy(m => m.Date).ToList();
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
