using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    interface IAddLeave
    {
        List<AddLeave> GetAddLeaveBonus(DateTime DateTo);
        List<AddLeave> GetAddLeaveBonus(DateTime dateFrom, DateTime DateTo);
        List<AddLeave> GetAddLeaveBonus(DateTime dateFrom, DateTime DateTo, List<int> lstUid);
        void SaveAddLeaveBonus(AddLeave UserSeniorityInput);
        void DeleteAddLeaveBonus(List<int> lstIds);
        void DeleteAddLeaveBonus(DateTime dateFrom, DateTime dateTo);
        void DeleteAddLeaveBonus(DateTime dateFrom, DateTime dateTo, List<int> lstUid);
    }
}
