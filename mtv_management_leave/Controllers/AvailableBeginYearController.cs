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

    public class AvailableBeginYearController : Controller
    {
        private DataBeginYearBase _dataBeginYearBase;

        public AvailableBeginYearController(DataBeginYearBase dataBeginYearBase)
        {
            _dataBeginYearBase = dataBeginYearBase;
        }
        public ActionResult Index()
        {
            return View(new Models.AvailableLeave.SearchRequest());
        }

        public PartialViewResult RegisterAvailableBeginYear()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult RegisterAvailableBeginYear(DataBeginYear registerAvailableInput)
        {
            DataBeginYear registerLeave = registerAvailableInput;
            if (ModelState.IsValid)
            {
                try
                {
                    _dataBeginYearBase.SaveDataBeginYear(registerLeave);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ex.Message);
                }
            }
            return PartialView(registerAvailableInput);
        }

        [HttpPost]
        public JsonResult Delete(ToogleApprove model)
        {
            try
            {
                _dataBeginYearBase.deleteDataBeginYear(model.Ids);
                return Json(new { Status = 0, Message = "Action complete" });
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Json(new { status = 400, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, AllowAnonymous]
        public JsonResult ToList(Models.AvailableLeave.SearchRequest model)
        {

            try
            {
                List<ResponseAvailableBeginYear> result = new List<ResponseAvailableBeginYear>();
                if (model.Year.Year != 1)
                {
                    if (model.Uids != null && model.Uids.Count == 1 && model.Uids[0] == 0)
                    {
                        model.Uids = null;
                    }
                    result = _dataBeginYearBase.GetDataBeginYear(model.Year.Year, model.Uids);
                }
                var resultJson = Json(new Lib.Repository.BootGridReponse<ResponseAvailableBeginYear>
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
