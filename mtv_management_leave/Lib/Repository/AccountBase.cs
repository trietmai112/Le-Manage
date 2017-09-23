using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class AccountBase : Base, IAccountBase
    {
        LeaveManagementEntities context;
        public string GetRoleByName(string RoleName)
        {
            InitContext(out context);
            var roleId = context.AspNetRoles.Where(m => m.Name == RoleName).Select(m => m.Id).FirstOrDefault();
            DisposeContext(context);
            return roleId;
        }
    }
}