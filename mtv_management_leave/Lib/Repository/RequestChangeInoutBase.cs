using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.Response;

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
            InitContext(out context);

            var lstRequestChange = context.RequestChangeInouts.Where(m => lstIdRequest.Contains(m.Id)).ToList();
            var lstUid = lstRequestChange.Select(m => m.Uid).Distinct().ToList();
            DateTime min = lstRequestChange.OrderBy(m => m.Date).Select(m => m.Date).FirstOrDefault();
            DateTime max = lstRequestChange.OrderByDescending(m => m.Date).Select(m => m.Date).FirstOrDefault();
            var lstInout = context.InOuts.Where(m => lstUid.Contains(m.Uid) && m.Date >= min && m.Date <= max).ToList();
            List<InOut> lstInoutAdd = new List<InOut>();
            foreach (var Req in lstRequestChange)
            {
                Req.status = Common.StatusLeave.E_Approve;
                var inout = lstInout.Where(m => m.Uid == Req.Uid && m.Date == Req.Date).FirstOrDefault();
                if (inout == null)
                {
                    // tao moi in out
                    InOut inOutAdd = new InOut();
                    inOutAdd.Date = Req.Date;
                    inOutAdd.Intime = Req.Intime ?? Req.Date.Date.AddHours(8);
                    inOutAdd.OutTime = Req.OutTime;
                    inOutAdd.IsModify = true;
                    inOutAdd.Uid = Req.Uid;
                    lstInoutAdd.Add(inOutAdd);
                }
                else
                {
                    if (Req.Intime != null)
                    {
                        inout.Intime = Req.Intime.Value;
                    }
                    if (Req.OutTime != null)
                    {
                        inout.OutTime = Req.OutTime;
                    }
                    inout.IsModify = true;
                }
            }

            if (lstInoutAdd.Count > 0)
            {
                context.InOuts.AddRange(lstInoutAdd);
            }
            context.SaveChanges();
            DisposeContext(context);



        }

        public void RejectRequestChange(List<int> lstIdRequest)
        {
            #region logic 
            //1. chuyển trạng thái của RequestChange
            #endregion
            InitContext(out context);
            var lstReject = context.RequestChangeInouts.Where(m => lstIdRequest.Contains(m.Id)).ToList();
            if (lstReject.Any(m => m.status == Common.StatusLeave.E_Approve))
            {
                DisposeContext(context);
                throw new Exception("Please select value not approved!");
            }
            lstReject.ForEach(m => m.status = Common.StatusLeave.E_Reject);
            context.SaveChanges();
            DisposeContext(context);
        }

        public List<ResponseChangeInout> GetRequestChangeInout(DateTime dateFrom, DateTime DateTo)
        {
            #region logic 
            //1. lấy toàn bộ dữ liệu trong khoảng từ và đến
            #endregion

            return getRequestChange(dateFrom, DateTo, null);
        }

        public List<ResponseChangeInout> GetRequestChangeInout(DateTime dateFrom, DateTime DateTo, int uid)
        {
            #region logic 
            //1. lấy toàn bộ dữ liệu trong khoảng từ và đến theo Uid
            #endregion
            return getRequestChange(dateFrom, DateTo, new List<int>() { uid });
        }

        public List<ResponseChangeInout> GetRequestChangeInout(DateTime dateFrom, DateTime DateTo, List<int> lstUid)
        {
            #region logic 
            //1. lấy toàn bộ dữ liệu trong khoảng từ và đến theo danh sách uid
            #endregion
            return getRequestChange(dateFrom, DateTo, lstUid);
        }

        public void SaveRequestChange(RequestChangeInout request)
        {
            #region logic 
            //1. Validate dữ liệu trùng
            //2. lưu dữ liệu vs status là request
            #endregion
            if (request.Intime == null && request.OutTime == null)
            {
                throw new Exception("Invalidate Input Inout!");
            }
            if (request.Intime != null && request.OutTime != null && request.Intime.Value.Date != request.OutTime.Value.Date)
            {
                throw new Exception("Please Update Inout In Day!");
            }
            InitContext(out context);

            if (context.RequestChangeInouts.Any(m => m.Uid == request.Uid && m.Date == request.Date && m.status != Common.StatusLeave.E_Reject))
            {
                DisposeContext(context);
                throw new Exception("Duplicate!");
            }
            request.status = Common.StatusLeave.E_Register;
            context.RequestChangeInouts.Add(request);
            context.SaveChanges();

            DisposeContext(context);
        }

        public void DeleteRequestChange(int idRequest)
        {
            #region logic 
            //1. Validate dữ liệu vs trạng thái khác request
            //2. delete dữ liệu vs requestId
            #endregion

            DeleteRequestChange(new List<int>() { idRequest });

        }

        public void DeleteRequestChange(List<int> lstIdRequest)
        {
            #region logic 
            //1. Validate dữ liệu vs trạng thái khác request
            //2. delete dữ liệu vs requestId
            #endregion

            InitContext(out context);
            var lstdelete = context.RequestChangeInouts.Where(m => lstIdRequest.Contains(m.Id)).ToList();
            if (lstdelete.Any(m => m.status != Common.StatusLeave.E_Register))
            {
                DisposeContext(context);
                throw new Exception("Please select only value register!");
            }
            context.RequestChangeInouts.RemoveRange(lstdelete);
            context.SaveChanges();
            DisposeContext(context);

        }

        #region Private method
        private List<ResponseChangeInout> getRequestChange(DateTime dateFrom, DateTime DateTo, List<int> lstUid)
        {
            var lstResult = new List<ResponseChangeInout>();
            InitContext(out context);
            var query = context.RequestChangeInouts.Where(m => m.Date >= dateFrom && m.Date <= DateTo);
            var queryInout = context.InOuts.Where(m => m.Date >= dateFrom && m.Date <= DateTo);
            if (lstUid != null && lstUid.Count > 0)
            {
                query = query.Where(m => lstUid.Contains(m.Uid));
                queryInout = queryInout.Where(m => lstUid.Contains(m.Uid));
            }
            var lstRqChange = query.Select(m => new { m.Uid, m.UserInfo.FullName, m.Intime, m.OutTime, m.Id, m.Reason, m.status, m.Date }).ToList();
            var lstInout = queryInout.Select(m => new { m.Date, m.Uid, m.Intime, m.OutTime }).ToList();
            foreach (var item in lstRqChange)
            {
                ResponseChangeInout rp = new ResponseChangeInout();
                rp.Id = item.Id;
                rp.Uid = item.Uid;
                rp.Date = item.Date;
                rp.FullName = item.FullName;
                rp.IntimeRequest = item.Intime!= null? item.Intime.Value.ToString("HH:mm") : string.Empty;
                rp.OutTimeRequest = item.OutTime != null ? item.OutTime.Value.ToString("HH:mm"): string.Empty;
                rp.Reason = item.Reason;
                rp.Status = Common.ConvertLeaveStatusToString((int)item.status);
                var inout = lstInout.Where(m => m.Uid == item.Uid && m.Date == item.Date).FirstOrDefault();
                if (inout != null)
                {
                    rp.Intime = inout.Intime != null ? inout.Intime.ToString("HH:mm"): string.Empty;
                    rp.OutTime = inout.OutTime != null ? inout.OutTime.Value.ToString("HH:mm") : string.Empty; 
                }
                lstResult.Add(rp);
            }


            DisposeContext(context);
            return lstResult;
        }
        #endregion
    }
}