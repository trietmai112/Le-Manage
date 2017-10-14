using System;
using System.Collections.Generic;
using System.Web.Mvc;
using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.RegisterLeave;
using mtv_management_leave.Models.Response;
using System.Linq;

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
        public JsonResult ToList(SearchRequest model)
        {
            var listUid = new List<int>();
            if (!string.IsNullOrEmpty(model.Uids))
            {
                var lstInputID = model.Uids.Split(new char[] { ',', ':', ';' }).Where(m => !string.IsNullOrEmpty(m)).ToList();
                foreach (var item in lstInputID)
                {
                    listUid.Add(int.Parse(item));
                }
            }

            var result = _leaveBase.GetLeave(model.DateStart, model.DateEnd, listUid);
          
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
