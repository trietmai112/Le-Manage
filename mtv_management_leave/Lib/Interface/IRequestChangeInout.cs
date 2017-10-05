using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    interface IRequestChangeInout
    {
        void GetRequestChangeInout(DateTime dateFrom, DateTime DateTo);
        void GetRequestChangeInout(DateTime dateFrom, DateTime DateTo, int uid);
        void GetRequestChangeInout(DateTime dateFrom, DateTime DateTo, List<int> lstUid);
        void ApproveRequestChange(List<int> lstIdRequest);
        void RejectRequestChange(List<int> lstIdRequest);
        void SaveRequestChange(RequestChangeInout request);
        void DeleteRequestChange(int requestId);
    }
}
