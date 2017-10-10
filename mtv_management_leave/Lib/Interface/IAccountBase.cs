using mtv_management_leave.Models;

namespace mtv_management_leave.Lib.Interface
{
    interface IAccountBase
    {
        int GetRoleByName(string RoleName);
        void UpdateUserInfo(RepoUserUpdateInfo UserInfo);

    }
}
