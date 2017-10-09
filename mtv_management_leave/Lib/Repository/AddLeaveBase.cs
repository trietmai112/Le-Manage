using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class AddLeaveBase : Base, IAddLeave
    {
        LeaveManagementContext context;

        public AddLeaveBase()
        {
        }

        public void DeleteAddLeaveBonus(List<int> lstIds)
        {
            InitContext(out context);
            var addLeavesDelete = context.AddLeaves.Where(m => lstIds.Contains(m.Id)).ToList();
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


        public List<AddLeave> GetAddLeaveBonus(DateTime DateTo)
        {
            return PrivateGetAddLeaveBonus(new DateTime(DateTo.Year, 1, 1), DateTo, null);
        }

        public List<AddLeave> GetAddLeaveBonus(DateTime dateFrom, DateTime DateTo)
        {
            return PrivateGetAddLeaveBonus(dateFrom, DateTo, null);
        }

        public List<AddLeave> GetAddLeaveBonus(DateTime dateFrom, DateTime DateTo, List<int> lstUid)
        {
            return PrivateGetAddLeaveBonus(dateFrom, DateTo, lstUid);
        }

        public void SaveAddLeaveBonus(AddLeave addLeaveInput)
        {
            InitContext(out context);
            var addLeaves = context.AddLeaves.Where(m => m.DateAdd != null && m.DateAdd == addLeaveInput.DateAdd && m.Uid == addLeaveInput.Uid).FirstOrDefault();
            if (addLeaves != null)
            {
                addLeaves.AddLeaveHour = addLeaveInput.AddLeaveHour;
            }
            else
            {
                context.AddLeaves.Add(addLeaveInput);
            }
            context.SaveChanges();
            DisposeContext(context);
        }

        #region Private method
        private List<AddLeave> PrivateGetAddLeaveBonus(DateTime dateFrom, DateTime DateTo, List<int> lstUid)
        {
            InitContext(out context);
            var query = context.AddLeaves.Where(m => m.DateAdd != null && m.DateAdd.Value >= dateFrom && m.DateAdd.Value <= DateTo);
            if (lstUid != null && lstUid.Count > 0)
            {
                query = query.Where(m => lstUid.Contains(m.Uid));
            }
            var lstResult = query.ToList();
            DisposeContext(context);
            return lstResult;
        }
        #endregion

    }
}