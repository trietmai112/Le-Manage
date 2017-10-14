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
    public class LeaveBase : Base, ILeaveBase
    {
        LeaveManagementContext context;
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
            double annualBonus = commonLeaveBase.GetAnnualBonus(context, uid, dateStart);
            double leaveInYear = commonLeaveBase.GetHourLeaveInYear(context, uid, dateStart);
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
            if (context.RegisterLeaves.Any(m => m.Status != Common.StatusLeave.E_Reject && m.Uid == leave.Uid && m.DateStart < leave.DateEnd && m.DateEnd >= leave.DateStart))
            {
                DisposeContext(context);
                throw new Exception("Duplicate Data!");
            }


            var maternityLeaveId = commonLeaveBase.GetLeaveTypeId(context, Common.TypeLeave.E_Materity.ToString());
            if (leave.LeaveTypeId == maternityLeaveId)
            {
                context.RegisterLeaves.Add(leave);
            }
            else if (leave.DateStart.Date == leave.DateEnd.Date)
            {
                if (leave.DateStart.Hour < 8 || leave.DateEnd.Hour > 17)
                {
                    DisposeContext(context);
                    throw new Exception("Please register leave in 8:00 to 17:00!");
                }
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
                    leaveRegister.DateEnd = dateLeave.Date.AddHours(17);
                    leaveRegister.DateRegister = leave.DateRegister;
                    leaveRegister.DateStart = dateLeave.Date.AddHours(8);
                    leaveRegister.LeaveTypeId = leave.LeaveTypeId;
                    leaveRegister.Reason = leave.Reason;
                    leaveRegister.RegisterHour = 8;
                    leaveRegister.Status = Common.StatusLeave.E_Register;
                    leaveRegister.Uid = leave.Uid;
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
            PrivateApproveLeave(new List<int>() { leaveId });
        }
        public void ApproveLeave(List<int> lstleaveId)
        {
            PrivateApproveLeave(lstleaveId);
        }

        public void RejectLeave(int leaveId)
        {
            PrivateRejectLeave(new List<int>() { leaveId });
        }

        public void RejectLeave(List<int> lstleaveId)
        {
            PrivateRejectLeave(lstleaveId);
        }

        public List<ResponseLeave> GetLeave(DateTime dateStart, DateTime dateEnd)
        {
            return PrivateGetLeave(dateStart, dateEnd, null);

        }
        public List<ResponseLeave> GetLeave(DateTime dateStart, DateTime dateEnd, List<int> lstUid)
        {
            return PrivateGetLeave(dateStart, dateEnd, lstUid);
        }

        public void DeleteLeave(List<int> lstLeaveId)
        {
            InitContext(out context);
            var lstLeave = context.RegisterLeaves.Where(m => lstLeaveId.Contains(m.Id)).ToList();
            if (lstLeave.Any(m => m.Status != Common.StatusLeave.E_Register))
            {
                DisposeContext(context);
                throw new Exception("Please delete only value register status!");
            }
            context.RegisterLeaves.RemoveRange(lstLeave);
            DisposeContext(context);

        }
        public void DeleteLeave(DateTime dateStart, DateTime dateEnd)
        {
            PrivateDeleteLeave(dateStart, dateEnd, null,true);
        }

        public void DeleteLeave(DateTime dateStart, DateTime dateEnd, List<int> lstUid)
        {
            PrivateDeleteLeave(dateStart, dateEnd, lstUid,true);
        }
        #region Private Method

        /// <summary>
        /// Hàm này chỉ tính thời gian leave trong 1 ngày
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        private double GetLeaveHourInDay(DateTime timeStart, DateTime timeEnd)
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
            if (commonLeaveBase.IsDateOffCompany(context, timeStart.Date))
            {
                DisposeContext(context);
                throw new Exception("It's a day off company");
            }

            DateTime start = timeStart.Date.AddHours(8);
            DateTime end = timeStart.Date.AddHours(17);
            DateTime startBreak = timeStart.Date.AddHours(12);
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
                DisposeContext(context);
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
                result = Math.Round((end - start).TotalHours - 1, 1);
            }
            return result;
        }

        private List<ResponseLeave> PrivateGetLeave(DateTime dateStart, DateTime dateEnd, List<int> lstUid)
        {
            if(dateStart == DateTime.MinValue && dateEnd == DateTime.MinValue)
            {
                return new List<ResponseLeave>();
            }

            if (dateStart.Year != dateEnd.Year || dateStart.Month != dateEnd.Month)
            {
                throw new Exception("Please select range date in month!");
            }
            List<ResponseLeave> lstResult = new List<ResponseLeave>();
            InitContext(out context);
            var query = context.RegisterLeaves.Where(m => m.DateStart <= dateEnd && m.DateEnd >= dateStart).Select(m => new { m.Id, m.Uid, m.UserInfo.FullName, m.DateStart, m.DateEnd, LeaveTypeName = m.MasterLeaveType.Name, m.RegisterHour, LeaveStatus = m.Status });
            if (lstUid != null && lstUid.Count > 0)
            {
                query = query.Where(m => lstUid.Contains(m.Uid));
            }
            var lstObj = query.ToList();
            foreach (var obj in lstObj)
            {
                ResponseLeave resLeave = new ResponseLeave();
                resLeave.Id = obj.Id;
                resLeave.LeaveTo = obj.DateEnd.ToString("yyyy-MM-dd HH:mm");
                resLeave.LeaveFrom = obj.DateStart.ToString("yyyy-MM-dd HH:mm");
                resLeave.FullName = obj.FullName;
                resLeave.LeaveStatus = Common.ConvertLeaveStatusToString((int) obj.LeaveStatus);
                resLeave.LeaveTypeName = Common.ConvertLeaveTypeToString(obj.LeaveTypeName);
                resLeave.RegisterHour = obj.RegisterHour;
                resLeave.Uid = obj.Uid;
                lstResult.Add(resLeave);
            }

            DisposeContext(context);
            return lstResult;
        }

        private void PrivateDeleteLeave(DateTime dateStart, DateTime dateEnd, List<int> lstUid, bool IsValidate)
        {
            InitContext(out context);
            dateEnd = dateEnd.Date.AddDays(1).AddMinutes(-1);
            var query = context.RegisterLeaves.Where(m => m.DateStart <= dateEnd && m.DateEnd >= dateStart);
            if (lstUid != null && lstUid.Count > 0)
            {
                query = query.Where(m => lstUid.Contains(m.Uid));
            }
            var lstLeave = query.ToList();
            if (lstLeave.Any(m => m.Status != Common.StatusLeave.E_Register) && IsValidate == true)
            {
                DisposeContext(context);
                throw new Exception("Please delete only value register status!");
            }
            context.RegisterLeaves.RemoveRange(lstLeave);
            context.SaveChanges();
            DisposeContext(context);
        }

        private void PrivateApproveLeave(List<int> lstleaveId)
        {
            InitContext(out context);
            var lstleave = context.RegisterLeaves.Where(m => lstleaveId.Contains(m.Id)).ToList();
            lstleave.ForEach(m => m.Status = Common.StatusLeave.E_Approve);
            context.SaveChanges();
            DisposeContext(context);
        }
        private void PrivateRejectLeave(List<int> lstleaveId)
        {
            InitContext(out context);
            var lstleave = context.RegisterLeaves.Where(m => lstleaveId.Contains(m.Id)).ToList();
            lstleave.ForEach(m => m.Status = Common.StatusLeave.E_Reject);
            context.SaveChanges();
            DisposeContext(context);
        }

        public void DeleteLeaveWithoutValidate(DateTime dateStart, DateTime dateEnd, List<int> lstUid)
        {
            PrivateDeleteLeave(dateStart, dateEnd, lstUid, false);
        }
        #endregion
    }
}