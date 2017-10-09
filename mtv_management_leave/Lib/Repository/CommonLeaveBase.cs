using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class CommonLeaveBase : Base, ICommonLeaveBase
    {
        public double GetAvailableBeginYear(LeaveManagementContext context,int uid, int year)
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
        public double GetAnnualBonus(LeaveManagementContext context, int uid, int year)
        {
            double annualBonus = context.AddLeaves.Where(m => m.Uid == uid && m.DateAdd != null && m.DateAdd.Value.Year == year)
                .Sum(m => m.AddLeaveHour) ?? 0;
            return annualBonus;
        }
        public double GetHourLeaveInYear(LeaveManagementContext context, int uid, int year)
        {
            //1. bỏ đi số ngày từ chối
            //2. bỏ đi loại thai sản
            //3. bỏ đi loại company trip
            //4. bỏ đi loại non-paid
            //5. bỏ đi loại other
            var lstLeaveTypeIds = context.MasterLeaveTypes.Where(m => m.LeaveCode == Common.TypeLeave.E_AnnualLeave.ToString()).Select(m => m.Id).ToList();
            //int rejectType = (int)Common.StatusLeave.E_Reject;
            double leaveInYear = context.RegisterLeaves.Where(m => m.Uid == uid && m.DateRegister.Year == year && m.Status != Common.StatusLeave.E_Reject
                        && lstLeaveTypeIds.Contains(m.LeaveTypeId)
                        ).Sum(m=> m.RegisterHour) ?? 0;
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
            throw new NotImplementedException();
        }

        public int GetLeaveTypeId(LeaveManagementContext context, string CodeLeave)
        {
            throw new NotImplementedException();
        }
    }
}