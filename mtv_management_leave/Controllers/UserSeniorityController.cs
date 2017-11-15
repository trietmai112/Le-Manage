using System.Collections.Generic;
using System.Web.Mvc;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.Request;
using mtv_management_leave.Models.Response;
using System;

namespace mtv_management_leave.Controllers
{
    public class UserSeniorityController : ControllerExtendsion
    {
        private UserSeniorityBase _userSeniorityBase;

        public UserSeniorityController(UserSeniorityBase userSeniorityBase)
        {
            _userSeniorityBase = userSeniorityBase;
        }
        [Authorize (Roles = "Super admin, Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ToList(RequestUserSeniority modelRequest)
        {
            int year = modelRequest.Year;
            var resultApi = new List<ResponseUserSeniority>();
            if (year != 1 && year != 0)
            {
                if (modelRequest.Uids != null && modelRequest.Uids.Count == 1 && modelRequest.Uids[0] == 0)
                {
                    modelRequest.Uids = null;
                }
                resultApi = _userSeniorityBase.GetUserSeniority(year, modelRequest.Uids);
            }
            return Json(new BootGridReponse<ResponseUserSeniority>
            {
                current = 1,
                total = resultApi.Count,
                rowCount = 20,
                rows = resultApi
            });
        }

        [HttpPost]
        public JsonResult Generate(RequestUserSeniority modelRequest)
        {
            if (modelRequest.Year != 0)
            {
                int year = modelRequest.Year;
                _userSeniorityBase.GenerateUserSeniority(year);
            }
            else
            {
                return BadRequest(new Exception("Please select year!"));
            }
            return Json(string.Empty);
        }
    }
}
