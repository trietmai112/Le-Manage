using AutoMapper;
using Microsoft.AspNet.Identity;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Account;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.Request;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace mtv_management_leave.Controllers
{
    public class EmployeeController : Controller
    {
        private LeaveManagementContext _context;
        private AccountBase _accountBase;

        public EmployeeController(AccountBase accountBase,
            LeaveManagementContext context)
        {
            _accountBase = accountBase;
            _context = context;
           
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

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
        public async Task<ActionResult> Update(RegisterViewModel model)
        {
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
            return RedirectToAction("Index");
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
            return RedirectToAction("Register");
        }     
        
        [HttpPost]
        public JsonResult ToList(RequestUserManagement condition)
        {
            return Json(_accountBase.ToList(condition));
        }   
    }
}
