using System;
using System.Collections.Generic;
using System.Web.Mvc;
using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.InOut;
using mtv_management_leave.Models.Response;
using System.Linq;
using mtv_management_leave.Models.Request;
using System.Net;
using mtv_management_leave.Models;

namespace mtv_management_leave.Controllers
{
    public class InOutManagementController : Controller
    {
        private InOutBase _inoutBase;
       // private DataRawBase _dataRawBase;

        public InOutManagementController(InOutBase inOutBase)
        {
            _inoutBase = inOutBase;
            //_dataRawBase = dataRawBase;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetFPdata()
        {
            try
            {
               // _dataRawBase.SaveDataRaw();
                return Json(new { Status = 0, Message = "Action complete" });
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Json(new { status = 400, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SumaryInout(SearchRequest model)
        {
            try
            {
                if (model.Uids != null && model.Uids.Count() == 1 && model.Uids[0] == 0)
                {
                    model.Uids = null;
                }
                _inoutBase.SaveGenerateInout(model.DateStart, model.DateEnd, model.Uids);
                return Json(new { Status = 0, Message = "Action complete" });
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Json(new { status = 400, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        

        [HttpPost]
        public JsonResult ToList(SearchRequest model)
        {
            try
            {
                if (model.Uids != null && model.Uids.Count() == 1 && model.Uids[0] == 0)
                {
                    model.Uids = null;
                }

                var result = _inoutBase.MappingInoutLeave(model.DateStart, model.DateEnd, model.Uids);
                if (model.InValidData != null && model.InValidData.Value == true)
                {
                    result = result.Where(m => m.IsValid == false).ToList();
                }
                return Json(new BootGridReponse<RepoMappingInOut>
                {
                    current = 1,
                    total = result.Count,
                    rowCount = -1,
                    rows = result
                });
            }
            catch (Exception e)
            {
                return Json(new { Status = (int)HttpStatusCode.BadRequest, Message = e.Message });
            }
        }
    }
}
