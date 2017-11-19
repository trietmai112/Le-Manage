using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.Response;

namespace mtv_management_leave.Controllers
{

    public class SummaryLeaveAnnualController : ControllerExtendsion
    {
        private LeaveTotalMonthlyBase _leaveTotal;

        public SummaryLeaveAnnualController(LeaveTotalMonthlyBase leaveTotal)
        {
            _leaveTotal = leaveTotal;
        }
        public ActionResult Index()
        {
            return View(new Models.LeaveBonus.SearchRequest());
        }

        [HttpPost, AllowAnonymous]
        public JsonResult ToList(Models.LeaveBonus.SearchRequest model)
        {
            try
            {

                List<ResponseLeaveTotalMonthly> result = new List<ResponseLeaveTotalMonthly>();

                if (model.DateStart != null)
                {
                    var uid = int.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    result = _leaveTotal.GetTotalMonthlyBeginYear(model.DateStart.Value, new List<int>() { uid });
                }
                var resultJson = Json(new Lib.Repository.BootGridReponse<ResponseLeaveTotalMonthly>
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
