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
    interface IAddLeave
    {
        List<ResponseLeaveBonus> GetAddLeaveBonus(DateTime DateTo);
        List<ResponseLeaveBonus> GetAddLeaveBonus(DateTime dateFrom, DateTime DateTo);
        List<ResponseLeaveBonus> GetAddLeaveBonus(DateTime dateFrom, DateTime DateTo, List<int> lstUid);
        void SaveAddLeaveBonus(AddLeave addLeaveInput);
        void SaveAddLeaveBonus(AddLeave addLeaveInput, List<int> lstUid);
        void DeleteAddLeaveBonus(List<int> lstIds);
        void DeleteAddLeaveBonus(DateTime dateFrom, DateTime dateTo);
        void DeleteAddLeaveBonus(DateTime dateFrom, DateTime dateTo, List<int> lstUid);
    }
}
