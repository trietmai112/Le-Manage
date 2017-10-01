using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class LeaveTotalMonthly : Base, ILeaveTotalMonthly
    {
        LeaveManagementContext context;
        ICommonLeaveBase commonLeaveBase;

        public LeaveTotalMonthly()
        {
            commonLeaveBase = new CommonLeaveBase();
        }

        public List<LeaveMonthly> GetLastTotalMonthly(DateTime monthYear)
        {
            var lstResult = new List<LeaveMonthly>();
            InitContext(out context);
            lstResult = context.LeaveMonthlies.Where(m => m.Month == monthYear).ToList();
            DisposeContext(context);
            return lstResult;
        }

        public List<LeaveMonthly> GetLastTotalMonthly(DateTime monthYear, int uid)
        {
            var lstResult = new List<LeaveMonthly>();
            InitContext(out context);
            lstResult = context.LeaveMonthlies.Where(m => m.Month == monthYear && m.Uid == uid).ToList();
            DisposeContext(context);
            return lstResult;
        }

        public void SaveLastTotalMonthly(DateTime MonthTo)
        {
            // lấy toàn bộ số ngày nghỉ từ đâu năm tới hiện tại => Duyệt , chỉ được lấy những loại tính phép năm
            // lấy số phép avalable đầu năm
            // lấy số phép thâm niên tính đến thời điểm hiện tại
            // lấy số phép cộng thêm cho nhân viên tính tới thời điểm hiện tại
            InitContext(out context);
            DateTime beginYear = new DateTime(MonthTo.Year, 1, 1);
            DateTime endMonth = MonthTo.AddMonths(1).AddHours(-1);
            DateTime lastMonth = endMonth.AddMonths(-1);
            var annualID = context.MasterLeaveTypes.Where(m => m.LeaveCode == Common.TypeLeave.E_AnnualLeave.ToString()).Select(m => m.Id).FirstOrDefault();
            var lstLeaveFromBeginYear = context.RegisterLeaves.Where(m => m.LeaveTypeId == annualID && m.Status == Common.StatusLeave.E_Approve && m.DateStart>= beginYear && m.DateStart<= endMonth).Select(m => new { m.Uid, m.DateStart, m.RegisterHour }).ToList();
            var lstAvailableBeginYear = context.DataBeginYears.Where(m => m.DateBegin.Year == beginYear.Year).Select(m => new { m.Uid, m.DateBegin, m.AnnualLeave }).ToList();
            var lstSeniorities = context.UserSeniorities.Where(m => m.Year == beginYear.Year).ToList();
            var lstLeaveAdd = context.AddLeaves.Where(m => m.DateAdd != null && m.DateAdd.Value >= beginYear && m.DateAdd.Value <= endMonth).Select(m=>new { m.Uid, m.DateAdd,m.AddLeaveHour}).ToList();
            var lstUserId = context.Users.Where(m => m.DateResign == null || (m.DateResign >= beginYear)).Select(m => m.Id).ToList();

            foreach (var uid in lstUserId)
            {
                LeaveMonthly leaveMon = new LeaveMonthly();
                var AnnualLeave_LastMonth_ByUser = lstLeaveFromBeginYear.Where(m => m.Uid == uid && m.DateStart <= lastMonth).Sum(m => m.RegisterHour);
                var AnnualLeave_ThisMonth_ByUser = lstLeaveFromBeginYear.Where(m => m.Uid == uid && m.DateStart > lastMonth && m.DateStart<= endMonth).Sum(m => m.RegisterHour);
                var Annual_Available_BeginYear = lstAvailableBeginYear.Where(m => uid == uid && m.DateBegin <= endMonth).Select(m => m.AnnualLeave).FirstOrDefault();
                var SeniorityObj = lstSeniorities.Where(m => m.Uid == uid).FirstOrDefault();
                double Seniority_ByUser = 0;
                if (SeniorityObj!= null)
                {
                    switch (MonthTo.Month)
                    {
                        case 1: Seniority_ByUser = SeniorityObj.Month1 ?? 0; break;
                        case 2: Seniority_ByUser = SeniorityObj.Month2 ?? 0; break;
                        case 3: Seniority_ByUser = SeniorityObj.Month3 ?? 0; break;
                        case 4: Seniority_ByUser = SeniorityObj.Month4 ?? 0; break;
                        case 5: Seniority_ByUser = SeniorityObj.Month5 ?? 0; break;
                        case 6: Seniority_ByUser = SeniorityObj.Month6 ?? 0; break;
                        case 7: Seniority_ByUser = SeniorityObj.Month7 ?? 0; break;
                        case 8: Seniority_ByUser = SeniorityObj.Month8 ?? 0; break;
                        case 9: Seniority_ByUser = SeniorityObj.Month9 ?? 0; break;
                        case 10: Seniority_ByUser = SeniorityObj.Month10 ?? 0; break;
                        case 11: Seniority_ByUser = SeniorityObj.Month11 ?? 0; break;
                        case 12: Seniority_ByUser = SeniorityObj.Month12 ?? 0; break;
                    }
                }
                var Annual_Add_ByUser = lstLeaveAdd.Where(m => m.Uid == uid).Sum(m => m.AddLeaveHour);
                leaveMon.LeaveAvailable = Annual_Available_BeginYear - AnnualLeave_LastMonth_ByUser + Seniority_ByUser + Annual_Add_ByUser;

            }
            







            DisposeContext(context);
        }

        public void SaveLastTotalMonthly(DateTime MonthTo, int uid)
        {
            throw new NotImplementedException();
        }

        public void ReCalculateTotalMonthly(DateTime MonthTo)
        {
            throw new NotImplementedException();
        }

        public void ReCalculateTotalMonthly(DateTime MonthTo, int uid)
        {
            throw new NotImplementedException();
        }

        public void CalculateTotalMonthly(DateTime MonthYear)
        {
            throw new NotImplementedException();
        }

        public void CalculateTotalMonthly(DateTime MonthYear, int uid)
        {
            throw new NotImplementedException();
        }
    }
}