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

        public List<LeaveMonthly> GetLastTotalMonthly(int year, int uid)
        {
            var lstResult = new List<LeaveMonthly>();
            InitContext(out context);
            lstResult = context.LeaveMonthlies.Where(m => m.Month.Year == year && m.Uid == uid).ToList();
            DisposeContext(context);
            return lstResult;
        }

        public void SaveLastTotalMonthly(DateTime MonthTo)
        {
            ExecuteSaveTotalMonthLeave(MonthTo , null);
        }

        public void SaveLastTotalMonthly(DateTime MonthTo, int uid)
        {
            ExecuteSaveTotalMonthLeave(MonthTo , uid);
        }

        public void ReCalculateTotalMonthlyAllYear(DateTime MonthTo)
        {
            throw new NotImplementedException();
        }

        public void ReCalculateTotalMonthlyAllYear(DateTime MonthTo, int uid)
        {
            throw new NotImplementedException();
        }

        #region Private method - class
        private class RepoAddLeave
        {
            public int Uid { get; set; }
            public DateTime? DateAdd { get; set; }
            public double? AddLeaveHour { get; set; }
        }

        private class RepoDataBeginYear
        {
            public int Uid { get; set; }
            public DateTime DateBegin { get; set; }
            public double AnnualLeave { get; set; }
        }
        private class RepoRegisterLeave
        {
            public int Uid { get; set; }
            public DateTime DateStart { get; set; }
            public double? RegisterHour { get; set; }
        }
        private void ExecuteSaveTotalMonthLeave(DateTime MonthTo, int? uidInput)
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
            var lstLeaveFromBeginYear_Query = context.RegisterLeaves.Where(m => m.LeaveTypeId == annualID && m.Status == Common.StatusLeave.E_Approve && m.DateStart >= beginYear && m.DateStart <= endMonth);
            var lstAvailableBeginYear_Query = context.DataBeginYears.Where(m => m.DateBegin.Year == beginYear.Year).Select(m => new RepoDataBeginYear { Uid = m.Uid, DateBegin = m.DateBegin, AnnualLeave = m.AnnualLeave });
            var lstSeniorities_Query = context.UserSeniorities.Where(m => m.Year == beginYear.Year);
            var lstLeaveAdd_Query = context.AddLeaves.Where(m => m.DateAdd != null && m.DateAdd.Value >= beginYear && m.DateAdd.Value <= endMonth).Select(m => new RepoAddLeave { Uid = m.Uid, DateAdd = m.DateAdd, AddLeaveHour = m.AddLeaveHour });
            var lstUserId_Query = context.Users.Where(m => m.DateResign == null || (m.DateResign >= beginYear)).Select(m => m.Id);
            var lstTotalMonthlyAlready_Query = context.LeaveMonthlies.Where(m => m.Month == MonthTo);
            if (uidInput != null)
            {
                lstLeaveFromBeginYear_Query = lstLeaveFromBeginYear_Query.Where(m => m.Uid == uidInput);
                lstAvailableBeginYear_Query = lstAvailableBeginYear_Query.Where(m => m.Uid == uidInput);
                lstSeniorities_Query = lstSeniorities_Query.Where(m => m.Uid == uidInput);
                lstLeaveAdd_Query = lstLeaveAdd_Query.Where(m => m.Uid == uidInput);
                lstUserId_Query = lstUserId_Query.Where(m => m == uidInput);
                lstTotalMonthlyAlready_Query = lstTotalMonthlyAlready_Query.Where(m => m.Uid == uidInput);
            }

            var lstLeaveFromBeginYear = lstLeaveFromBeginYear_Query.Select(m => new RepoRegisterLeave { Uid = m.Uid, DateStart = m.DateStart, RegisterHour = m.RegisterHour }).ToList();
            var lstAvailableBeginYear = lstAvailableBeginYear_Query.ToList();
            var lstSeniorities = lstSeniorities_Query.ToList();
            var lstLeaveAdd = lstLeaveAdd_Query.ToList();
            var lstUserId = lstUserId_Query.ToList();
            var lstTotalMonthlyAlready = lstTotalMonthlyAlready_Query.ToList();


            foreach (var uid in lstUserId)
            {
                bool alreadyHave = true;
                LeaveMonthly leaveMon = lstTotalMonthlyAlready.Where(m => m.Uid == uid).FirstOrDefault();
                if (leaveMon == null)
                {
                    leaveMon = new LeaveMonthly();
                    alreadyHave = false;
                }
                var AnnualLeave_LastMonth_ByUser = lstLeaveFromBeginYear.Where(m => m.Uid == uid && m.DateStart <= lastMonth).Sum(m => m.RegisterHour);
                var AnnualLeave_ThisMonth_ByUser = lstLeaveFromBeginYear.Where(m => m.Uid == uid && m.DateStart > lastMonth && m.DateStart <= endMonth).Sum(m => m.RegisterHour);
                var Annual_Available_BeginYear = lstAvailableBeginYear.Where(m => m.Uid == uid && m.DateBegin <= endMonth).Select(m => m.AnnualLeave).FirstOrDefault();
                var SeniorityObj = lstSeniorities.Where(m => m.Uid == uid).FirstOrDefault();
                double Seniority_ByUser = 0;
                if (SeniorityObj != null)
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
                leaveMon.LeaveUsed = AnnualLeave_ThisMonth_ByUser;
                leaveMon.LeaveRemain = leaveMon.LeaveAvailable - leaveMon.LeaveUsed;
                leaveMon.Month = MonthTo;
                leaveMon.Uid = uid;
                if (!alreadyHave)
                {
                    context.LeaveMonthlies.Add(leaveMon);
                }
            }
            context.SaveChanges();
            DisposeContext(context);
        }

       
        #endregion
    }
}