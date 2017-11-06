using System;
using System.Collections.Generic;
using System.Web.Mvc;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.Response;

namespace mtv_management_leave.Controllers
{

    public class SummaryLeaveAnnualManagementController : Controller
    {
        private LeaveTotalMonthlyBase _leaveTotal;

        public SummaryLeaveAnnualManagementController(LeaveTotalMonthlyBase leaveTotal)
        {
            _leaveTotal = leaveTotal;
        }

        [Authorize(Roles = "Super admin, Admin")]
        public ActionResult Index()
        {
            return View(new Models.LeaveBonus.SearchRequest());
        }

        public JsonResult ReCalculateMonth(Models.LeaveBonus.SearchRequest model)
        {
            if (model.DateStart != null)
            {
                if (model.Uids != null && model.Uids.Count == 1 && model.Uids[0] == 0)
                {
                    model.Uids = null;
                }
                _leaveTotal.SaveLastTotalMonthly(model.DateStart.Value, model.Uids);
            }
            return Json(new { Status = 0, Message = "Action complete" });
        }

        public JsonResult ReCalculateYear(Models.LeaveBonus.SearchRequest model)
        {
            if (model.DateStart != null)
            {
                if (model.Uids != null && model.Uids.Count == 1 && model.Uids[0] == 0)
                {
                    model.Uids = null;
                }
                _leaveTotal.ReCalculateTotalMonthlyAllYear(model.DateStart.Value, model.Uids);
            }
            return Json(new { Status = 0, Message = "Action complete" });
        }

        [HttpPost, AllowAnonymous]
        public JsonResult ToList(Models.LeaveBonus.SearchRequest model)
        {
            try
            {

                List<ResponseLeaveTotalMonthly> result = new List<ResponseLeaveTotalMonthly>();

                if (model.DateStart != null)
                {
                    if (model.Uids != null && model.Uids.Count == 1 && model.Uids[0] == 0)
                    {
                        model.Uids = null;
                    }

                    result = _leaveTotal.GetTotalMonthlyBeginYear(model.DateStart.Value, model.Uids);
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
