using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    interface ILeaveBase
    {
        void RegisterLeave(RegisterLeave leave);

        void ApproveLeave(int leaveId);

        void RejectLeave(int leaveId);
        /// <summary>
        /// 1. lấy cho từng người
        /// 2. tính trong năm nay
        /// </summary>
        /// <param name="uid"></param>
        /// <param name=""></param>
        /// <returns></returns>
        double GetLeaveRemain(int uid, DateTime dateStart);

        double GetLeaveHourInDay(DateTime timeStart, DateTime timeEnd);       

    }
}
