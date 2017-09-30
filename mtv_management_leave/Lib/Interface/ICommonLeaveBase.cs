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
        double GetAvailableBeginYear(LeaveManagementContext context, int uid, int year);
        int GetSeniority(LeaveManagementContext context, int uid, DateTime dateStart);
        double GetAnnualBonus(LeaveManagementContext context, int uid, int year);
        double GetHourLeaveInYear(LeaveManagementContext context, int uid, int year);
        List<DateTime> GetListDayOffCompany(LeaveManagementContext context, int Year);
        List<DateTime> GetListDayOffCompany(LeaveManagementContext context, DateTime dateStart, DateTime dateEnd);
        bool IsDateOffCompany(LeaveManagementContext context, DateTime date);
        int GetLeaveTypeId(LeaveManagementContext context, string CodeLeave);
    }
}
