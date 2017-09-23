using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    /// <summary>
    /// Những hàm chung để tính toán leave
    /// </summary>
    interface ICommonLeaveBase
    {
        double getAvailableBeginYear(LeaveManagementEntities context, int uid, int year);
        int GetSeniority(LeaveManagementEntities context, int uid, DateTime dateStart);
        double getAnnualBonus(LeaveManagementEntities context, int uid, int year);
        double getHourLeaveInYear(LeaveManagementEntities context, int uid, int year);
    }
}
