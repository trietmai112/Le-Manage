using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    /// <summary>
    /// Summerize tổng leave cho từng tháng
    /// </summary>
    interface ILeaveTotalMonthly
    {
        List<LeaveMonthly> GetLastTotalMonthly(DateTime monthYear);
        List<LeaveMonthly> GetLastTotalMonthly(DateTime monthYear, List<int> lstUid);
        List<ResponseLeaveTotalMonthly> GetTotalMonthlyBeginYear(DateTime monthYear, List<int> lstUid);

        void SaveLastTotalMonthly(DateTime MonthTo);
        void SaveLastTotalMonthly(DateTime MonthTo, List<int> lstUid);

        /// <summary>
        /// Hàm tính lại từ đầu Năm
        /// </summary>
        void ReCalculateTotalMonthlyAllYear(DateTime MonthTo);
        void ReCalculateTotalMonthlyAllYear(DateTime MonthTo, List<int> lstUid);





    }
}
