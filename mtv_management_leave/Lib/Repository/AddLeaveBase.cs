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
    public class AddLeaveBase : Base, IAddLeave
    {
        LeaveManagementContext context;

        public AddLeaveBase()
        {
        }

        public void ApproveAddLeaveBonus(List<int> lstIds)
        {
            InitContext(out context);
            var addLeavesApprove = context.AddLeaves.Where(m => lstIds.Contains(m.Id)).ToList();
            string E_Approve = Common.StatusLeave.E_Approve.ToString();
            addLeavesApprove.ForEach(m => m.Status = E_Approve);
            context.SaveChanges();
            DisposeContext(context);
        }

        public void DeleteAddLeaveBonus(List<int> lstIds)
        {
            InitContext(out context);
            var addLeavesDelete = context.AddLeaves.Where(m => lstIds.Contains(m.Id)).ToList();
            string E_Approve = Common.StatusLeave.E_Approve.ToString();
            string E_Reject = Common.StatusLeave.E_Reject.ToString();
            if (addLeavesDelete.Any(m => m.Status == E_Approve || m.Status == E_Reject))
            {
                throw new Exception("Please delete only data register!!");
            }
            context.AddLeaves.RemoveRange(addLeavesDelete);
            context.SaveChanges();
            DisposeContext(context);
        }

        public void DeleteAddLeaveBonus(DateTime dateFrom, DateTime dateTo)
        {

            InitContext(out context);
            var addLeavesDelete = context.AddLeaves.Where(m => m.DateAdd >= dateFrom && m.DateAdd <= dateTo).ToList();
            context.AddLeaves.RemoveRange(addLeavesDelete);
            context.SaveChanges();
            DisposeContext(context);
        }

        public void DeleteAddLeaveBonus(DateTime dateFrom, DateTime dateTo, List<int> lstUid)
        {

            InitContext(out context);
            var addLeavesDelete = context.AddLeaves.Where(m => m.DateAdd >= dateFrom && m.DateAdd <= dateTo && lstUid.Contains(m.Uid)).ToList();
            context.AddLeaves.RemoveRange(addLeavesDelete);
            context.SaveChanges();
            DisposeContext(context);
        }


        public List<ResponseLeaveBonus> GetAddLeaveBonus(DateTime DateTo)
        {
            return PrivateGetAddLeaveBonus(new DateTime(DateTo.Year, 1, 1), DateTo, null);
        }

        public List<ResponseLeaveBonus> GetAddLeaveBonus(DateTime dateFrom, DateTime DateTo)
        {
            return PrivateGetAddLeaveBonus(dateFrom, DateTo, null);
        }

        public List<ResponseLeaveBonus> GetAddLeaveBonus(DateTime dateFrom, DateTime DateTo, List<int> lstUid)
        {
            return PrivateGetAddLeaveBonus(dateFrom, DateTo, lstUid);
        }

        public void RejectAddLeaveBonus(List<int> lstIds)
        {
            InitContext(out context);
            var addLeavesReject = context.AddLeaves.Where(m => lstIds.Contains(m.Id)).ToList();
            string E_Reject = Common.StatusLeave.E_Reject.ToString();
            addLeavesReject.ForEach(m => m.Status = E_Reject);
            context.SaveChanges();
            DisposeContext(context);
        }

        public void SaveAddLeaveBonus(AddLeave addLeaveInput)
        {
            InitContext(out context);
            string E_Reject = Common.StatusLeave.E_Reject.ToString();
            var addLeaves = context.AddLeaves.Where(m => m.DateAdd != null && m.DateAdd == addLeaveInput.DateAdd && m.Uid == addLeaveInput.Uid && m.Status!= E_Reject).FirstOrDefault();
            if (addLeaves != null)
            {
                throw new Exception("Data Duplicate!");
            }
            else
            {
                context.AddLeaves.Add(addLeaveInput);
            }
            context.SaveChanges();
            DisposeContext(context);
        }

        public void SaveAddLeaveBonus(AddLeave addLeaveInput, List<int> lstUid)
        {
            InitContext(out context);
            var addLeavesDb = context.AddLeaves.Where(m => m.DateAdd != null && m.DateAdd == addLeaveInput.DateAdd && lstUid.Contains(m.Uid)).ToList();
            foreach (var uid in lstUid)
            {
                var leaveDB = addLeavesDb.Where(m => m.Uid == uid).FirstOrDefault();
                if (leaveDB != null)
                {
                    leaveDB.AddLeaveHour = addLeaveInput.AddLeaveHour;
                }
                else
                {
                    AddLeave newAdd = new AddLeave();
                    newAdd.Uid = addLeaveInput.Uid;
                    newAdd.AddLeaveHour = addLeaveInput.AddLeaveHour;
                    newAdd.DateAdd = addLeaveInput.DateAdd;
                    newAdd.Reason = addLeaveInput.Reason;
                    context.AddLeaves.Add(newAdd);
                }
            }

            context.SaveChanges();
            DisposeContext(context);
        }

        #region Private method
        private List<ResponseLeaveBonus> PrivateGetAddLeaveBonus(DateTime dateFrom, DateTime DateTo, List<int> lstUid)
        {
            InitContext(out context);
            if(DateTo< dateFrom)
            {
                throw new Exception("Please select From less or equal with To!");
            }
            if(dateFrom.Year!= DateTo.Year || dateFrom.Month!= DateTo.Month)
            {
                throw new Exception("Please select search in month!");
            }
            var query = context.AddLeaves.Where(m => m.DateAdd != null && m.DateAdd.Value >= dateFrom && m.DateAdd.Value <= DateTo);
            if (lstUid != null && lstUid.Count > 0)
            {
                query = query.Where(m => lstUid.Contains(m.Uid));
            }
            var lstResult = query.Select(m => new ResponseLeaveBonus() { Id = m.Id, Uid = m.Uid, FullName = m.UserInfo.FullName, DateAdd = m.DateAdd, AddLeaveHour = m.AddLeaveHour, Reason = m.Reason , Status= m.Status , Type= m.Type}).ToList();
            lstResult.ForEach(m => m.Status = Common.ConvertLeaveStatusToString(m.Status));
            DisposeContext(context);
            return lstResult;
        }
        #endregion

    }
}