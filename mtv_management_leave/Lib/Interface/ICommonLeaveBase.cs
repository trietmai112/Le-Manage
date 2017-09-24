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
        double GetAvailableBeginYear(LeaveManagementEntities context, int uid, int year);
        int GetSeniority(LeaveManagementEntities context, int uid, DateTime dateStart);
        double GetAnnualBonus(LeaveManagementEntities context, int uid, int year);
        double GetHourLeaveInYear(LeaveManagementEntities context, int uid, int year);
        List<DateTime> GetListDayOffCompany(LeaveManagementEntities context, int Year);
        List<DateTime> GetListDayOffCompany(LeaveManagementEntities context, DateTime dateStart, DateTime dateEnd);
        bool IsDateOffCompany(LeaveManagementEntities context, DateTime date);
        int GetLeaveTypeId(LeaveManagementEntities context, string CodeLeave);
    }
}
