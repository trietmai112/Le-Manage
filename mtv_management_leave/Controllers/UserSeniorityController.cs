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
    public class UserSeniorityController : Controller
    {
        private UserSeniorityBase _userSeniorityBase;

        public UserSeniorityController(UserSeniorityBase userSeniorityBase)
        {
            _userSeniorityBase = userSeniorityBase;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ToList(RequestUserSeniority modelRequest)
        {
            int year = modelRequest.DateYear.Year;
            var resultApi = _userSeniorityBase.GetUserSeniority(year);
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
            int year = modelRequest.DateYear.Year;
            _userSeniorityBase.GenerateUserSeniority(year);
            return Json(string.Empty);
        }
    }
}
