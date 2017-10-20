using Microsoft.AspNet.Identity;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Account;
using mtv_management_leave.Models.Request;
using mtv_management_leave.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    interface IAccountBase
    {
        int GetRoleByName(string RoleName);
        void UpdateUserInfo(RepoUserUpdateInfo UserInfo);
        BootGridReponse<ResponseUserManagement> ToList(RequestUserManagement contrain);
        Task<IdentityResult> Register(RegisterViewModel model);
        UpdatedViewModel GetById(int id);
    }
}
