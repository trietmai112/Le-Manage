using System.Linq;
using mtv_management_leave.Lib.Interface;
using mtv_management_leave.Models;

namespace mtv_management_leave.Lib.Repository
{
    public class AccountBase : Base, IAccountBase
    {
        LeaveManagementContext context;
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
    }
}