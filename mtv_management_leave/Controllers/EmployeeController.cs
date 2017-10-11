using AutoMapper;
using Microsoft.AspNet.Identity;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Account;
using mtv_management_leave.Models.Entity;
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
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private LeaveManagementContext _context;

        public EmployeeController(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            LeaveManagementContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

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

            var result = await RegisterFunction(model);
            if (!result.Succeeded)
            {
                AddErrors(result);
                transaction.Rollback();
                return View(model);
            }
            transaction.Commit();
            return RedirectToAction("Register");
        }

        private async Task<IdentityResult> RegisterFunction(RegisterViewModel model)
        {
            var user = Mapper.Map<UserInfo>(model);
            IdentityResult identityResult = null;

            if (user.Id > 0) identityResult = await _userManager.UpdateAsync(user);
            
            else identityResult = await _userManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded) return identityResult;

            var roles = await _context.Roles.Where(m => model.RoleIds.Contains(m.Id)).ToListAsync();
            var userInRoles = await _context.Set<UserRole>().Where(m => m.UserId == user.Id).Select(m=> m.RoleId).ToListAsync();
            if (userInRoles.Count > 0)
                identityResult = _userManager.RemoveFromRoles(user.Id, 
                    _context.Roles.Where(m => userInRoles.Contains(m.Id)).Select(m=> m.Name).ToArray());

            if (!identityResult.Succeeded) return identityResult;

            if (roles.Count > 0)
            {
                identityResult = _userManager.AddToRoles(user.Id, roles.Select(m => m.Name).ToArray());
            }

            if (!identityResult.Succeeded) return identityResult;

            try
            {
                if (model.FPId.HasValue)
                {
                    _context.EmployeeInfos.RemoveRange(_context.EmployeeInfos.Where(m => m.Id == user.Id));
                    var result = await _context.SaveChangesAsync();

                    _context.EmployeeInfos.Add(new EmployeeInfo { Id = user.Id, FPId = model.FPId.Value });
                    result = await _context.SaveChangesAsync();
                }
            }catch(Exception ex)
            {
                return IdentityResult.Failed(ex.Message);
            }
            return IdentityResult.Success;
            // await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            //string code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //await _userManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            // If we got this far, something failed, redisplay form
        }
    }
}
