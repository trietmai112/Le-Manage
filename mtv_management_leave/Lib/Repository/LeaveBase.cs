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

            double AvailableBeginYear = commonLeaveBase.GetAvailableBeginYear(context, uid, dateStart.Year);
            int Seniority = commonLeaveBase.GetSeniority(context, uid, dateStart);
            double annualBonus = commonLeaveBase.GetAnnualBonus(context, uid, dateStart.Year);
            double leaveInYear = commonLeaveBase.GetHourLeaveInYear(context, uid, dateStart.Year);
            var result = AvailableBeginYear + Seniority + annualBonus - leaveInYear;

            DisposeContext(context);
            return result;

        }
        public void RegisterLeave(RegisterLeave leave)
        {
            #region logic
            //1. nếu đăng ký trong ngày thì phải tính toán được số giờ
            //2. nếu đăng ký khác ngày thì phải tách ra
            //  2.1 phải trừ đi ngày nghỉ thứ 7, cn, ngày nghỉ cty

            #endregion
            leave.DateRegister = DateTime.Today;
            InitContext(out context);
            var maternityLeaveId = commonLeaveBase.GetLeaveTypeId(context, Common.TypeLeave.E_Materity.ToString());
            if (leave.LeaveTypeId == maternityLeaveId)
            {
                context.RegisterLeaves.Add(leave);
            }
            else if (leave.DateStart.Date == leave.DateEnd.Date)
            {
                //gọi lại 1 lần nữa tránh việc modify rồi send lên server
                leave.RegisterHour = GetLeaveHourInDay(leave.DateStart, leave.DateEnd);
                context.RegisterLeaves.Add(leave);
            }
            else //break từng ngày
            {
                var lstDayRegister = new List<RegisterLeave>();
                var lstDayOff = commonLeaveBase.GetListDayOffCompany(context, leave.DateStart.Date, leave.DateEnd.Date);
                for (DateTime dateLeave = leave.DateStart.Date; dateLeave <= leave.DateEnd.Date; dateLeave = dateLeave.AddDays(1))
                {
                    if (dateLeave.Date.DayOfWeek == DayOfWeek.Saturday || dateLeave.Date.DayOfWeek == DayOfWeek.Sunday || lstDayOff.Any(m => m.Date == dateLeave))
                    {
                        continue;
                    }
                    RegisterLeave leaveRegister = new Models.Entity.RegisterLeave();
                    leaveRegister.DateApprove = leave.DateApprove;
                    leaveRegister.DateEnd = leave.DateEnd;
                    leaveRegister.DateRegister = leave.DateRegister;
                    leaveRegister.DateStart = leave.DateStart;
                    leaveRegister.LeaveTypeId = leave.LeaveTypeId;
                    leaveRegister.Reason = leave.Reason;
                    leaveRegister.RegisterHour = 8;
                    leaveRegister.Status = leave.Status;
                    leaveRegister.Uid = leave.Uid;
                    leaveRegister.UserApprove = leave.UserApprove;
                    lstDayRegister.Add(leaveRegister);
                }
                if (lstDayRegister.Count > 0)
                    context.RegisterLeaves.AddRange(lstDayRegister);
            }
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

        /// <summary>
        /// Hàm này chỉ tính thời gian leave trong 1 ngày
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public double GetLeaveHourInDay(DateTime timeStart, DateTime timeEnd)
        {
            double result = 0;

            if (timeStart.Date != timeEnd.Date)
            {
                throw new Exception("You have register in one day");
            }
            if (timeStart.Date.DayOfWeek == DayOfWeek.Saturday || timeStart.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new Exception("It's weekend");
            }
            InitContext(out context);
            if (commonLeaveBase.IsDateOffCompany(context, timeStart.Date))
            {
                DisposeContext(context);
                throw new Exception("It's a day off company");
            }

            DateTime start = timeStart.Date.AddHours(8);
            DateTime end = timeStart.Date.AddHours(17).AddMinutes(15);
            DateTime startBreak = timeStart.Date.AddHours(11).AddMinutes(45);
            DateTime endBreak = timeStart.Date.AddHours(13);
            if (timeStart > start)
            {
                start = timeStart;
            }
            if (timeEnd < end)
            {
                end = timeEnd;
            }

            if (start > startBreak && end < endBreak)
            {
                throw new Exception("Register in the breaktime");
            }

            //check có đi qua giờ trưa hay không
            if (end <= startBreak || start >= endBreak)
            {
                result = Math.Round((end - start).TotalHours, 1);
            }
            //check có nằm trong khoảng giờ trưa hay không
            else if (start < endBreak && start > startBreak)
            {
                start = endBreak;
                result = Math.Round((end - start).TotalHours, 1);
            }
            else if (end > startBreak && end < endBreak)
            {
                end = startBreak;
                result = Math.Round((end - start).TotalHours, 1);
            }
            else
            {
                result = Math.Round((end - start).TotalHours - 1.25, 1);
            }
            DisposeContext(context);
            return result;
        }
    }
}