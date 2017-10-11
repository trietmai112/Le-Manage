using System;
using System.Collections.Generic;
using System.Web.Mvc;
using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.RegisterLeave;
using mtv_management_leave.Models.Response;
using mtv_management_leave.Models.Request;
using System.Linq;
using AutoMapper;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.Register;

namespace mtv_management_leave.Controllers
{
    public class MasterDayOffCompanyController : Controller
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
        public JsonResult ToList(RequestMasterDayOff modelRequest)
        {
            DateTime dateStart = modelRequest.DateStart ?? new DateTime(2000, 1, 1);
            DateTime dateEnd = modelRequest.DateEnd ?? new DateTime(2100, 1, 1);


            var resultApi = _dayOffCompanyBase.GetLeaveDayCompany(dateStart, dateEnd).Select(m => new ResponseMasterDayOff() { DateLeave = m.Date, Reason = m.Description }).ToList();


            return Json(new BootGridReponse<ResponseMasterDayOff>
            {
                current = 1,
                total = resultApi.Count,
                rowCount = 20,
                rows = resultApi
            });
        }

        public PartialViewResult RegisterDayOffCompany()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult RegisterDayOffCompany(RegisterMasterDayOff registerDayOffCompany)
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
                    ModelState.AddModelError("Error", ex.Message);
                }
            }
            return PartialView(registerDayOffCompany);
        }
    }
}
