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
    interface ILeaveBase
    {
        void RegisterLeave(RegisterLeave leave);

        void ApproveLeave(int leaveId);
        void ApproveLeave(List<int> lstleaveId);

        void RejectLeave(int leaveId);
        void RejectLeave(List<int> lstleaveId);
        /// <summary>
        /// 1. lấy cho từng người
        /// 2. tính trong năm nay
        /// </summary>
        /// <param name="uid"></param>
        /// <param name=""></param>
        /// <returns></returns>
        double GetLeaveRemain(int uid, DateTime dateStart);

        List<ResponseLeave> GetLeave(DateTime dateStart, DateTime dateEnd);
        List<ResponseLeave> GetLeave(DateTime dateStart, DateTime dateEnd, List<int> lstUid);

        void DeleteLeave(DateTime dateStart, DateTime dateEnd);
        void DeleteLeave(DateTime dateStart, DateTime dateEnd, List<int> lstUid);
        void DeleteLeave(List<int> lstLeaveId);
        void DeleteLeaveWithoutValidate(DateTime dateStart, DateTime dateEnd, List<int> lstUid);
    }
}
