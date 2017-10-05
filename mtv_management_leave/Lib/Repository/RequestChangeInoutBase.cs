using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class RequestChangeInoutBase : Base, IRequestChangeInout
    {
        LeaveManagementContext context;

        public RequestChangeInoutBase()
        {
        }

        public void ApproveRequestChange(List<int> lstIdRequest)
        {
            #region logic 
            //1. Update thông tin cho bảng Inout
            //2. Nếu thông tin bản inout chua có thì tạo mới
            //3. Update cho cột modify trong bảng inout = true
            //4. chuyển trạng thái của Request Change
            #endregion
        }

        public void RejectRequestChange(List<int> lstIdRequest)
        {
            #region logic 
            //1. chuyển trạng thái của RequestChange
            #endregion
        }

        public void GetRequestChangeInout(DateTime dateFrom, DateTime DateTo)
        {
            #region logic 
            //1. lấy toàn bộ dữ liệu trong khoảng từ và đến
            #endregion
        }

        public void GetRequestChangeInout(DateTime dateFrom, DateTime DateTo, int uid)
        {
            #region logic 
            //1. lấy toàn bộ dữ liệu trong khoảng từ và đến theo Uid
            #endregion
        }

        public void GetRequestChangeInout(DateTime dateFrom, DateTime DateTo, List<int> lstUid)
        {
            #region logic 
            //1. lấy toàn bộ dữ liệu trong khoảng từ và đến theo danh sách uid
            #endregion
        }

        public void SaveRequestChange(RequestChangeInout request)
        {
            #region logic 
            //1. Validate dữ liệu trùng
            //2. lưu dữ liệu vs status là request
            #endregion
        }

        public void DeleteRequestChange(int requestId)
        {
            #region logic 
            //1. Validate dữ liệu vs trạng thái khác request
            //2. delete dữ liệu vs requestId
            #endregion
        }
    }
}