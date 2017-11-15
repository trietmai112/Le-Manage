using System;
using System.Collections.Generic;
using System.Linq;
using mtv_management_leave.Lib.Interface;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class CommonLeaveBase : Base, ICommonLeaveBase
    {
        public double GetAvailableBeginYear(LeaveManagementContext context, int uid, int year)
        {
            return context.DataBeginYears.Where(m => m.Uid == uid && m.DateBegin.Year == year).Select(m => m.AnnualLeave).FirstOrDefault();
        }
        public int GetSeniority(LeaveManagementContext context, int uid, DateTime dateStart)
        {
            int Seniority = 0;
            var SeniorityItem = context.UserSeniorities.Where(m => m.Uid == uid && m.Year == dateStart.Year).FirstOrDefault();
            if (SeniorityItem != null)
            {
                switch (dateStart.Month)
                {
                    case 1:
                        {
                            Seniority = SeniorityItem.Month1 ?? 0;
                            break;
                        }
                    case 2:
                        {
                            Seniority = SeniorityItem.Month2 ?? 0;
                            break;
                        }
                    case 3:
                        {
                            Seniority = SeniorityItem.Month3 ?? 0;
                            break;
                        }
                    case 4:
                        {
                            Seniority = SeniorityItem.Month4 ?? 0;
                            break;
                        }
                    case 5:
                        {
                            Seniority = SeniorityItem.Month5 ?? 0;
                            break;
                        }
                    case 6:
                        {
                            Seniority = SeniorityItem.Month6 ?? 0;
                            break;
                        }
                    case 7:
                        {
                            Seniority = SeniorityItem.Month7 ?? 0;
                            break;
                        }
                    case 8:
                        {
                            Seniority = SeniorityItem.Month8 ?? 0;
                            break;
                        }
                    case 9:
                        {
                            Seniority = SeniorityItem.Month9 ?? 0;
                            break;
                        }
                    case 10:
                        {
                            Seniority = SeniorityItem.Month10 ?? 0;
                            break;
                        }
                    case 11:
                        {
                            Seniority = SeniorityItem.Month11 ?? 0;
                            break;
                        }
                    case 12:
                        {
                            Seniority = SeniorityItem.Month12 ?? 0;
                            break;
                        }
                }
            }
            return Seniority;
        }
        public double GetAnnualBonus(LeaveManagementContext context, int uid, DateTime dateTo)
        {
            DateTime BeginYear = new DateTime(dateTo.Year, 1, 1);
            string approved = Common.StatusLeave.E_Approve.ToString();
            double annualBonus = context.AddLeaves.Where(m => m.Uid == uid && m.DateAdd != null && m.DateAdd >= BeginYear && m.DateAdd.Value <= dateTo && m.Status == approved).ToList().Sum(m => m.AddLeaveHour ?? 0);
            return annualBonus;
        }
        public double GetHourLeaveInYear(LeaveManagementContext context, int uid, DateTime dateTo)
        {
            //1. bỏ đi số ngày từ chối
            //2. bỏ đi loại thai sản
            //3. bỏ đi loại company trip
            //4. bỏ đi loại non-paid
            //5. bỏ đi loại other
            var lstLeaveTypeIds = context.MasterLeaveTypes.Where(m => m.LeaveCode == Common.TypeLeave.E_AnnualLeave.ToString()).Select(m => m.Id).ToList();
            //int rejectType = (int)Common.StatusLeave.E_Reject;
            DateTime beginYear = new DateTime(dateTo.Year, 1, 1);
            double leaveInYear = context.RegisterLeaves.Where(m => m.Uid == uid && m.DateStart >= beginYear && m.DateStart <= dateTo && m.Status != Common.StatusLeave.E_Reject
            && lstLeaveTypeIds.Contains(m.LeaveTypeId)
            ).Select(m => m.RegisterHour).ToList().Sum(m => m ?? 0);
            return leaveInYear;
        }
        public List<DateTime> GetListDayOffCompany(LeaveManagementContext context, int Year)
        {
            return context.MasterLeaveDayCompanies.Where(m => m.Date.Year == Year).Select(m => m.Date).ToList();
        }
        public bool IsDateOffCompany(LeaveManagementContext context, DateTime date)
        {
            return context.MasterLeaveDayCompanies.Any(m => m.Date == date.Date);
        }

        public List<DateTime> GetListDayOffCompany(LeaveManagementContext context, DateTime dateStart, DateTime dateEnd)
        {
            return context.MasterLeaveDayCompanies.Where(m => m.Date >= dateStart && m.Date <= dateEnd).Select(m => m.Date).ToList();
        }

        public int GetLeaveTypeId(LeaveManagementContext context, string CodeLeave)
        {
            return context.MasterLeaveTypes.Where(m => m.LeaveCode == CodeLeave).Select(m => m.Id).FirstOrDefault();
        }


        public List<LeaveMonthly> GetTotalLeaveMonthly(LeaveManagementContext context, DateTime monthTo, List<int> lstUid)
        {

            // lấy toàn bộ số ngày nghỉ từ đâu năm tới hiện tại => Duyệt , chỉ được lấy những loại tính phép năm
            // lấy số phép avalable đầu năm
            // lấy số phép thâm niên tính đến thời điểm hiện tại
            // lấy số phép cộng thêm cho nhân viên tính tới thời điểm hiện tại
            string E_Approved = Common.StatusLeave.E_Approve.ToString();
            DateTime beginYear = new DateTime(monthTo.Year, 1, 1);
            DateTime endMonth = monthTo.AddMonths(1).AddHours(-1);
            DateTime lastMonth = endMonth.AddMonths(-1);
            var annualID = context.MasterLeaveTypes.Where(m => m.LeaveCode == Common.TypeLeave.E_AnnualLeave.ToString()).Select(m => m.Id).FirstOrDefault();
            var lstLeaveFromBeginYear_Query = context.RegisterLeaves.Where(m => m.LeaveTypeId == annualID && m.Status == Common.StatusLeave.E_Approve && m.DateStart >= beginYear && m.DateStart <= endMonth);
            var lstAvailableBeginYear_Query = context.DataBeginYears.Where(m => m.DateBegin.Year == beginYear.Year).Select(m => new RepoDataBeginYear { Uid = m.Uid, DateBegin = m.DateBegin, AnnualLeave = m.AnnualLeave });
            var lstSeniorities_Query = context.UserSeniorities.Where(m => m.Year == beginYear.Year);
            var lstLeaveAdd_Query = context.AddLeaves.Where(m => m.DateAdd != null && m.DateAdd.Value >= beginYear && m.DateAdd.Value <= endMonth && m.Status == E_Approved).Select(m => new RepoAddLeave { Uid = m.Uid, DateAdd = m.DateAdd, AddLeaveHour = m.AddLeaveHour });
            var lstUserId_Query = context.Users.Where(m => m.DateResign == null || (m.DateResign >= beginYear)).Select(m => m.Id);

            if (lstUid != null && lstUid.Count > 0)
            {
                lstLeaveFromBeginYear_Query = lstLeaveFromBeginYear_Query.Where(m => lstUid.Contains(m.Uid));
                lstAvailableBeginYear_Query = lstAvailableBeginYear_Query.Where(m => lstUid.Contains(m.Uid));
                lstSeniorities_Query = lstSeniorities_Query.Where(m => lstUid.Contains(m.Uid));
                lstLeaveAdd_Query = lstLeaveAdd_Query.Where(m => lstUid.Contains(m.Uid));
                lstUserId_Query = lstUserId_Query.Where(m => lstUid.Contains(m));
            }
            var lstLeaveFromBeginYear = lstLeaveFromBeginYear_Query.Select(m => new RepoRegisterLeave { Uid = m.Uid, DateStart = m.DateStart, RegisterHour = m.RegisterHour }).ToList();
            var lstAvailableBeginYear = lstAvailableBeginYear_Query.ToList();
            var lstSeniorities = lstSeniorities_Query.ToList();
            var lstLeaveAdd = lstLeaveAdd_Query.ToList();
            var lstUserId = lstUserId_Query.ToList();
            List<LeaveMonthly> lstResult = new List<LeaveMonthly>();
            foreach (var uid in lstUserId)
            {
                var AnnualLeave_LastMonth_ByUser = lstLeaveFromBeginYear.Where(m => m.Uid == uid && m.DateStart <= lastMonth).Select(m => m.RegisterHour).ToList().Sum(m => m);
                var AnnualLeave_ThisMonth_ByUser = lstLeaveFromBeginYear.Where(m => m.Uid == uid && m.DateStart > lastMonth && m.DateStart <= endMonth).Select(m => m.RegisterHour).ToList().Sum(m => m);
                var Annual_Available_BeginYear = lstAvailableBeginYear.Where(m => m.Uid == uid && m.DateBegin <= endMonth).Select(m => m.AnnualLeave).FirstOrDefault();
                var SeniorityObj = lstSeniorities.Where(m => m.Uid == uid).FirstOrDefault();
                double Seniority_ByUser = 0;
                if (SeniorityObj != null)
                {
                    switch (monthTo.Month)
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
                var Annual_Add_ByUser = lstLeaveAdd.Where(m => m.Uid == uid).ToList().Sum(m => m.AddLeaveHour);
                LeaveMonthly leaveMon = new LeaveMonthly();
                leaveMon.LeaveAvailable = Annual_Available_BeginYear - AnnualLeave_LastMonth_ByUser + (8 * Seniority_ByUser) + Annual_Add_ByUser;
                leaveMon.LeaveUsed = AnnualLeave_ThisMonth_ByUser;
                leaveMon.LeaveRemain = leaveMon.LeaveAvailable - leaveMon.LeaveUsed;
                leaveMon.Month = monthTo;
                leaveMon.Uid = uid;
                lstResult.Add(leaveMon);
            }
            return lstResult;
        }
    }
}