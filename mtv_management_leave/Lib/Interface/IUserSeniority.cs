using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    interface IUserSeniority
    {
        List<UserSeniority> GetUserSeniority(int year);
        List<UserSeniority> GetUserSeniority(int year, List<int> lstUid);
        void GenerateUserSeniority(int year);
        void GenerateUserSeniority(int year, List<int> lstUid);

    }
}
