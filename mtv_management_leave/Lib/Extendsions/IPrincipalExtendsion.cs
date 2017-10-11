using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Extendsions
{
    public static class IPrincipalExtendsion
    {
        public static string GetRoleName(this IPrincipal principal)
        {
            foreach(var item in PropertiesStatic.AdminRoleNames)
            {
                if (principal.IsInRole(item)) return item;
            }
            return "User";
        }
    }
}
