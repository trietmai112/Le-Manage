using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.Identity;
using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Interface;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Account;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.Request;
using mtv_management_leave.Models.Response;

namespace mtv_management_leave.Lib.Repository
{
    public class AccountBase : Base, IAccountBase
    {
        LeaveManagementContext context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountBase()
        {
            _userManager = HttpContext.Current.GetFromNInject<ApplicationUserManager>();
            _signInManager = HttpContext.Current.GetFromNInject<ApplicationSignInManager>();
        }

        public int GetRoleByName(string RoleName)
        {
            InitContext(out context);
            var roleId = context.Roles.Where(m => m.Name == RoleName).Select(m => m.Id).FirstOrDefault();
            DisposeContext(context);
            return roleId;
        }

        public void UpdateUserInfo(RepoUserUpdateInfo UserInfo)
        {
            InitContext(out context);
            var userDB = context.Users.Where(m => m.Id == UserInfo.Id).FirstOrDefault();
            if (userDB != null)
            {
                userDB.FullName = UserInfo.FullName;
                userDB.DateBeginProbation = UserInfo.DateBeginProbation;
                userDB.DateBeginWork = UserInfo.DateBeginWork;
                userDB.DateOfBirth = UserInfo.DateOfBirth;
                userDB.DateResign = UserInfo.DateResign;
            }
            context.SaveChanges();
            DisposeContext(context);
        }

        public BootGridReponse<ResponseUserManagement> ToList(RequestUserManagement condition)
        {
            var loginRole = HttpContext.Current.User.GetRoleName();
            var loginRoleId = GetRoleByName(loginRole);
            InitContext(out context);

            var iquery = from user in context.Users
                         join userRole in context.Set<UserRole>() on user.Id equals userRole.UserId into gUserRole
                         from gur in gUserRole.DefaultIfEmpty()
                         join role in context.Roles on gur.RoleId equals role.Id into gRole
                         from gr in gRole.DefaultIfEmpty()
                         join employee in context.EmployeeInfos on user.Id equals employee.Id into gEmployee
                         from ge in gEmployee.DefaultIfEmpty()
                         where gr.Id >= loginRoleId || gr == null
                         select new ResponseUserManagement
                         {
                             DateBeginProbation = user.DateBeginProbation,
                             DateBeginWork = user.DateBeginWork,
                             DateOfBirth = user.DateOfBirth,
                             DateResign = user.DateResign,
                             Email = user.Email,
                             FPId = ge == null ? (int?)null : ge.FPId,
                             FullName = user.FullName,
                             Id = user.Id,
                             PhoneNumber = user.PhoneNumber,
                             RoleName = gr == null ? null : gr.Name,
                             RoleId = gr == null ? 0 : gr.Id
                         };

            if (!string.IsNullOrEmpty(condition.SearchString))
            {
                string searchLower = condition.SearchString.ToLower();
                iquery = iquery.Where(m => m.Id.ToString().Contains(searchLower)
                                            || m.FullName.ToLower().Contains(searchLower)
                                            || m.Email.ToLower().Contains(searchLower)
                                            || m.PhoneNumber.ToLower().Contains(searchLower)
                                            || m.RoleName.ToLower().Contains(searchLower));
            }
            if (condition.Roles != null)
            {
                var roleList = condition.Roles.Where(m => m > 0);
                if (roleList.Count() > 0)
                    iquery = iquery.Where(m => roleList.Contains(m.RoleId));
            }
            if (condition.sort != null && condition.sort.Count > 0)
            {
                Func<ResponseUserManagement, dynamic> OrderFunction = (m) =>
                {
                    switch (condition.sort.Keys.FirstOrDefault()?.ToLower())
                    {
                        case "id":
                            return m.Id;
                        case "fullname":
                            return m.FullName;
                        case "phonenumber":
                            return m.PhoneNumber;
                        case "RoleName":
                            return m.RoleName;
                        default:
                            return m.Id;
                    }
                };
                iquery = condition.sort.Values.FirstOrDefault() == "asc" ?
                    iquery.AsEnumerable().OrderBy(OrderFunction).AsQueryable()
                    : iquery.AsEnumerable().OrderByDescending(OrderFunction).AsQueryable();
            }
            var totalRecord = iquery.Count();
            var result = iquery.ToList();
            DisposeContext(context);
            return new BootGridReponse<ResponseUserManagement>
            {
                current = condition.current,
                rowCount = condition.rowCount,
                rows = result,
                total = totalRecord
            };
        }

        public async Task<IdentityResult> Register(RegisterViewModel model)
        {
            InitContext(out context);
            var transaction = context.Database.BeginTransaction();
            var result = await RegisterFunction(model);
            if (!result.Succeeded)
                transaction.Rollback();
            else
                transaction.Commit();
            DisposeContext(context);
            return result;
        }

        private async Task<IdentityResult> RegisterFunction(RegisterViewModel model)
        {

            var user = Mapper.Map<UserInfo>(model);
            IdentityResult identityResult = null;
            _userManager.UserValidator = new UserValidator<UserInfo, int>(_userManager) { AllowOnlyAlphanumericUserNames = false };
            if (user.Id > 0) identityResult = await _userManager.UpdateAsync(user);

            else identityResult = await _userManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded) return identityResult;

            var roles = await context.Roles.Where(m => model.RoleIds.Contains(m.Id)).ToListAsync();
            var userInRoles = await context.Set<UserRole>().Where(m => m.UserId == user.Id).Select(m => m.RoleId).ToListAsync();
            if (userInRoles.Count > 0)
                identityResult = _userManager.RemoveFromRoles(user.Id,
                    context.Roles.Where(m => userInRoles.Contains(m.Id)).Select(m => m.Name).ToArray());

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
                    context.EmployeeInfos.RemoveRange(context.EmployeeInfos.Where(m => m.Id == user.Id));
                    var result = await context.SaveChangesAsync();

                    context.EmployeeInfos.Add(new EmployeeInfo { Id = user.Id, FPId = model.FPId.Value });
                    result = await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(ex.Message);
            }
            return IdentityResult.Success;
        }

        public UpdatedViewModel GetById(int id)
        {
            InitContext(out context);
            var iquery = from user in context.Users
                         join userRole in context.Set<UserRole>() on user.Id equals userRole.UserId into gUserRole
                         from gur in gUserRole.DefaultIfEmpty()
                         join role in context.Roles on gur.RoleId equals role.Id into gRole
                         from gr in gRole.DefaultIfEmpty()
                         join employee in context.EmployeeInfos on user.Id equals employee.Id into gEmployee
                         from ge in gEmployee.DefaultIfEmpty()
                         where user.Id == id
                         select new UpdatedViewModel
                         {
                             DateBeginProbation = user.DateBeginProbation,
                             DateBeginWork = user.DateBeginWork,
                             DateOfBirth = user.DateOfBirth,
                             DateResign = user.DateResign,
                             Email = user.Email,
                             FPId = ge == null ? (int?)null : ge.FPId,
                             FullName = user.FullName,
                             Id = user.Id,
                             PhoneNumber = user.PhoneNumber,
                             RoleIds = gUserRole.Select(x => x.RoleId).ToList()
                         };
            var result = iquery.FirstOrDefault();
            DisposeContext(context);
            return result;
        }

        public IdentityResult ChangePassword(ChangePasswordViewModel model)
        {
            var result = _userManager.ChangePassword(model.Id.GetValueOrDefault(), model.Password, model.NewPassword);
            return result;
        }

        public async Task<IdentityResult> Update(UpdatedViewModel model)
        {
            InitContext(out context);
            var userInfo = _userManager.FindById(model.Id);
            userInfo.DateBeginProbation = model.DateBeginProbation;
            userInfo.DateBeginWork = model.DateBeginWork;
            userInfo.DateOfBirth = model.DateOfBirth;
            userInfo.DateResign = model.DateResign;
            userInfo.FullName = model.FullName;
            userInfo.PhoneNumber = model.PhoneNumber;
            _userManager.UserValidator = new UserValidator<UserInfo, int>(_userManager) { AllowOnlyAlphanumericUserNames = false };
            var updateUserResult = _userManager.Update(userInfo);
            if (updateUserResult.Succeeded == false) return updateUserResult;

            var transaction = context.Database.BeginTransaction();
            var employeeInfo = await context.EmployeeInfos.FirstOrDefaultAsync(m => m.Id == userInfo.Id);
            if (employeeInfo == null)
            {
                employeeInfo = new EmployeeInfo { Id = userInfo.Id, FPId = model.FPId.GetValueOrDefault() };
                context.EmployeeInfos.Add(employeeInfo);
            }
            else
            {
                employeeInfo.FPId = model.FPId.GetValueOrDefault();
                context.EmployeeInfos.Attach(employeeInfo);
                context.Entry<EmployeeInfo>(employeeInfo).State = EntityState.Modified;
            }


            var result = await context.SaveChangesAsync();

            var roles = await context.Roles.Where(m => model.RoleIds.Contains(m.Id)).ToListAsync();
            var userInRoles = await context.Set<UserRole>().Where(m => m.UserId == userInfo.Id).Select(m => m.RoleId).ToListAsync();
            if (userInRoles.Count > 0)
                updateUserResult = _userManager.RemoveFromRoles(userInfo.Id,
                    context.Roles.Where(m => userInRoles.Contains(m.Id)).Select(m => m.Name).ToArray());

            if (!updateUserResult.Succeeded)
            {
                transaction.Rollback();
                return updateUserResult;
            }

            if (roles.Count > 0)
            {
                updateUserResult = _userManager.AddToRoles(userInfo.Id, roles.Select(m => m.Name).ToArray());
            }
            if (!updateUserResult.Succeeded)
            {
                transaction.Rollback();
                return updateUserResult;
            }
            transaction.Commit();
            return updateUserResult;

        }
    }
}