using AutoMapper;
using Microsoft.AspNet.Identity;
using mtv_management_leave.Lib;
using mtv_management_leave.Lib.Interface;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Account;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.InOut;
using mtv_management_leave.Models.Request;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace mtv_management_leave.Controllers
{
    public class EmployeeController : ControllerExtendsion
    {
        private LeaveManagementContext _context;
        private AccountBase _accountBase;
        private InOutBase _inoutBase;
        private IDataBeginYear _dataBeginYear;


        public EmployeeController(AccountBase accountBase,
            LeaveManagementContext context, InOutBase inoutBase, DataBeginYearBase dataBeginYear)
        {
            _accountBase = accountBase;
            _context = context;
            _inoutBase = inoutBase;
            _dataBeginYear = dataBeginYear;

        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public PartialViewResult ViewInfoBasic()
        {
            var model = _accountBase.GetById(HttpContext.User.Identity.GetUserId<int>());
            return PartialView("_ViewInfoBasic", model);
        }


        [HttpPost]
        public JsonResult InvidualInOutRefresh(SearchRequest model)
        {
            var now = DateTime.Now;
            var result = _inoutBase.MappingInoutLeave(
                model.DateStart,
                model.DateEnd,
                new List<int> { HttpContext.User.Identity.GetUserId<int>() }).OrderByDescending(m => m.Date).ToList();
            return Json(new BootGridReponse<RepoMappingInOut>
            {
                current = 1,
                total = result.Count,
                rowCount = -1,
                rows = result
            });
        }

        public PartialViewResult ViewInvidualInOut()
        {
            var now = DateTime.Now;
            return PartialView("_ViewInvidualInOut", new SearchRequest
            {
                DateStart = new DateTime(now.Year, now.Month, 1),
                DateEnd = new DateTime(now.Year, now.Month + 1, 1).AddDays(-1),
            });
        }
        [AllowAnonymous]
        public ActionResult Profile()
        {
            return View(new mtv_management_leave.Models.Account.RegisterViewModel
            {
                DateBeginWork = DateTime.Now
            });
        }

        [Authorize(Roles = "Super admin, Admin")]
        public void Excel()
        {
            string path = Path.Combine(Server.MapPath("~/"), $"{DateTime.Now.Ticks}.xlsx");
            ExcelCommon common = new ExcelCommon();
            var result = _accountBase.ToList(new RequestUserManagement());
            common.AddHeader<Models.Response.ResponseUserManagement>();
            common.AddRecords(result.rows);
            common.Save(path);


            Response.AppendHeader("content-disposition", "attachment;filename=FileEName.xlsx");
            Response.Charset = "";
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.ContentType = "Application/x-msexcel";
            Response.WriteFile(path);
            Response.Flush();
            Response.End();

            System.IO.File.Delete(path);
        }

        [Authorize(Roles = "Super admin, Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Super admin, Admin")]
        public ActionResult Update(int id)
        {
            if (id == 0)
                return RedirectToAction("Register");
            var model = _accountBase.GetById(id);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(UpdatedViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_context.Users.Count(m => m.Email.Equals(model.Email, StringComparison.CurrentCultureIgnoreCase)
                                 && m.Id != model.Id) > 0)
            {
                ModelState.AddModelError("Email", "Email ready used by another user");
                return View(model);
            }
            var transaction = _context.Database.BeginTransaction();

            var result = await _accountBase.Update(model);
            if (!result.Succeeded)
            {
                AddErrors(result);
                transaction.Rollback();
                return View(model);
            }
            transaction.Commit();
            _dataBeginYear.AutoSaveDataBeginYear(model.Id);

            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var result = _accountBase.ChangePassword(model);
            if (result.Succeeded)
                return Json(new { Result = result.Succeeded });
            return Json(new { Result = result.Succeeded, Message = result.Errors.FirstOrDefault() });
        }

        [Authorize(Roles = "Super admin, Admin")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ///Transnsaction maybe not work
            if (!ModelState.IsValid) return View(model);

            if (_context.Users.Count(m => m.Email.Equals(model.Email, StringComparison.CurrentCultureIgnoreCase)
                                 && m.Id != model.Id) > 0)
            {
                ModelState.AddModelError("Email", "Email ready used by another user");
                return View(model);
            }
            var transaction = _context.Database.BeginTransaction();

            var result = await _accountBase.Register(model);
            if (!result.Succeeded)
            {
                AddErrors(result);
                transaction.Rollback();
                return View(model);
            }
            transaction.Commit();
            _dataBeginYear.AutoSaveDataBeginYear(model.Email);
            return RedirectToAction("Register");
        }

        [HttpPost]
        public JsonResult ToList(RequestUserManagement condition)
        {
            return Json(_accountBase.ToList(condition));
        }
    }
}
