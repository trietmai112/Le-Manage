using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.Register;
using mtv_management_leave.Models.Request;
using mtv_management_leave.Models.Response;

namespace mtv_management_leave.Controllers
{
    [Authorize(Roles = "Super admin, Admin")]
    public class MasterDayOffCompanyController : ControllerExtendsion
    {
        private DayOffCompanyBase _dayOffCompanyBase;

        public MasterDayOffCompanyController(DayOffCompanyBase dayOffCompanyBase)
        {
            _dayOffCompanyBase = dayOffCompanyBase;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Delete(ToogleApprove model)
        {
            try
            {
                _dayOffCompanyBase.DeleteLeaveDayCompany(model.Ids);
                return Json(new { Status = 200, Message = "Action complete" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public JsonResult ToList(RequestMasterDayOff modelRequest)
        {
            try
            {
                var resultApi = new List<ResponseMasterDayOff>();
                if (modelRequest.DateStart != null && modelRequest.DateEnd != null)
                {
                    resultApi = _dayOffCompanyBase.GetLeaveDayCompany(modelRequest.DateStart.Value, modelRequest.DateEnd.Value).Select(m => new ResponseMasterDayOff() { Id = m.Id, DateLeave = m.Date, Reason = m.Description }).ToList();
                }
                return Json(new BootGridReponse<ResponseMasterDayOff>
                {
                    current = 1,
                    total = resultApi.Count,
                    rowCount = 20,
                    rows = resultApi
                });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        public PartialViewResult RegisterDayOffCompany()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult RegisterDayOffCompany(RegisterMasterDayOff registerDayOffCompany)
        {
            MasterLeaveDayCompany registerDayOff = new MasterLeaveDayCompany();
            registerDayOff.Date = registerDayOffCompany.Date;
            registerDayOff.Description = registerDayOffCompany.Description;
            if (ModelState.IsValid)
            {
                try
                {
                    _dayOffCompanyBase.SaveLeaveDayCompany(registerDayOff);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            return Json(new { Status = 200, Message = "Action complete" });
        }
    }
}
