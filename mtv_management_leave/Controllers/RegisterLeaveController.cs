using mtv_management_leave.Lib.Repository;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace mtv_management_leave.Controllers
{

    public class RegisterLeaveController: Controller
    {
        private InOutBase _inOutBase;

        public RegisterLeaveController(InOutBase inOutBase)
        {
            _inOutBase = inOutBase;
        }
        public ActionResult Index()
        {
            return View(new Models.RegisterLeave.SearchRequest {
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now
            });
        }

        [HttpPost, Route("api/registerleave/search"), AllowAnonymous]
        public JsonResult Search(Models.RegisterLeave.SearchRequest  model)
        {
            var result = _inOutBase.MappingInoutLeave(model.DateStart, model.DateEnd, model.Uid.Value);
            return Json(new Lib.Repository.BootGridReponse<object>
            {
                current = 1,
                rowCount = 0,
                rows = new List<object>()
            });
        }
    }
}
