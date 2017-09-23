using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class LeaveBase : Base, ILeaveBase
    {
        LeaveManagementEntities context;
        ICommonLeaveBase commonLeaveBase;

        public LeaveBase()
        {
            commonLeaveBase = new CommonLeaveBase();
        }

        public double GetLeaveRemain(int uid, DateTime dateStart)
        {
            //lây số phép đầu năm
            //lấy số phép thâm niên
            //lấy số phép bảng cộng
            //trừ đi số phép đa đăng ký trong năm
            InitContext(out context);

            double AvailableBeginYear = commonLeaveBase.getAvailableBeginYear(context, uid, dateStart.Year);
            int Seniority = commonLeaveBase.GetSeniority(context, uid, dateStart);
            double annualBonus = commonLeaveBase.getAnnualBonus(context, uid, dateStart.Year);
            double leaveInYear = commonLeaveBase.getHourLeaveInYear(context, uid, dateStart.Year);
            var result = AvailableBeginYear + Seniority + annualBonus - leaveInYear;

            DisposeContext(context);
            return result;

        }
        public void RegisterLeave(RegisterLeave leave)
        {
            InitContext(out context);
            context.RegisterLeaves.Add(leave);
            context.SaveChanges();
            DisposeContext(context);
        }
        public void ApproveLeave(int leaveId)
        {
            InitContext(out context);
            var leave = context.RegisterLeaves.Where(m => m.Id == leaveId).FirstOrDefault();
            leave.Status = (int)Common.StatusLeave.E_Approve;
            context.SaveChanges();
            DisposeContext(context);

        }
        public void RejectLeave(int leaveId)
        {
            InitContext(out context);
            var leave = context.RegisterLeaves.Where(m => m.Id == leaveId).FirstOrDefault();
            leave.Status = (int)Common.StatusLeave.E_Reject;
            context.SaveChanges();
            DisposeContext(context);
        }
    }
}