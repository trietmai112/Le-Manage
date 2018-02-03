using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using mtv_management_leave.Lib.Interface;
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
            leave.Status = Common.StatusLeave.E_Register;
            if (leave.DateStart >= leave.DateEnd)
            {
                throw new Exception("End Time must be after Start Time!");

            }


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
                if (leave.DateStart.Hour < 8 || leave.DateEnd > leave.DateStart.Date.AddHours(Common.EndShift))
                {
                    DisposeContext(context);
                    throw new Exception("Please register leave in 8:00 to 17:30!");
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
            if (lstLeave.Any(m => m.Status != Common.StatusLeave.E_Register && m.Status != Common.StatusLeave.E_Reject))
            {
                DisposeContext(context);
                throw new Exception("Please delete only value register or disapprove status!");
            }
            context.RegisterLeaves.RemoveRange(lstLeave);
            context.SaveChanges();
            DisposeContext(context);

        }
        public void DeleteLeave(DateTime dateStart, DateTime dateEnd)
        {
            PrivateDeleteLeave(dateStart, dateEnd, null, true);
        }

        public void DeleteLeave(DateTime dateStart, DateTime dateEnd, List<int> lstUid)
        {
            PrivateDeleteLeave(dateStart, dateEnd, lstUid, true);
        }

        public void AutoCreateLeave(List<RepoMappingInOut> lstInoutInvalid)
        {
            if (lstInoutInvalid == null || lstInoutInvalid.Count == 0)
                return;
            // Nếu như ngày hôm đó nhân viên đã đăng ký hoặc duyệt rồi thì ko tự động đăng ký nữa
            // Suy nghĩ đến trường hợp nhân viên đã đăng ký nhưng không đủ  (ví dụ đi trễ 1.5 tiếng nhưng mới chỉ đăng ký 1 tiếng?)
            InitContext(out context);
            List<RegisterLeave> lstLeaveResult = new List<Models.Entity.RegisterLeave>();

            DateTime dateMin = lstInoutInvalid.Min(m => m.Date);
            DateTime dateMax = lstInoutInvalid.Max(m => m.Date);
            dateMax = dateMax.Date.AddDays(1).AddMinutes(-1);
            List<int> lstUids = lstInoutInvalid.Select(m => m.Uid).Distinct().ToList();
            var e_reject = Common.StatusLeave.E_Reject;
            var e_approved = Common.StatusLeave.E_Approve;

            var lstLeaveAlready = context.RegisterLeaves.Where(m => m.Status != e_reject
                                    && m.DateStart <= dateMax
                                    && m.DateEnd >= dateMin
                                    && lstUids.Contains(m.Uid)).Select(m => new { m.Uid, m.DateStart, m.DateEnd }).ToList();
            string E_AnnualLeave = Common.TypeLeave.E_AnnualLeave.ToString();
            var leaveTypeAnnualId = context.MasterLeaveTypes.Where(m => m.LeaveCode == E_AnnualLeave).Select(m => m.Id).FirstOrDefault();
            if (leaveTypeAnnualId == null || leaveTypeAnnualId == 0)
            {
                DisposeContext(context);
                throw new Exception("miss data master annual leave!");
            }


            foreach (var inoutInValid in lstInoutInvalid)
            {
                if (lstLeaveAlready.Any(m => m.Uid == inoutInValid.Uid && m.DateStart.Date <= inoutInValid.Date && m.DateEnd.Date >= inoutInValid.Date))
                {
                    continue;
                }
                DateTime date = inoutInValid.Date;

                //Thiếu in- out
                if (string.IsNullOrEmpty(inoutInValid.Intime) || string.IsNullOrEmpty(inoutInValid.Outtime))
                {
                    RegisterLeave leaveRegister = new Models.Entity.RegisterLeave();
                    leaveRegister.DateRegister = DateTime.Today;
                    leaveRegister.DateApprove = DateTime.Today;
                    leaveRegister.DateStart = date.AddHours(Common.BeginShift);
                    leaveRegister.DateEnd = date.AddHours(Common.EndShift);
                    leaveRegister.LeaveTypeId = leaveTypeAnnualId;
                    leaveRegister.Reason = "Auto created by miss inout";
                    leaveRegister.RegisterHour = 8;
                    leaveRegister.Status = e_approved;
                    leaveRegister.Uid = inoutInValid.Uid;
                    leaveRegister.UserApprove = int.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    lstLeaveResult.Add(leaveRegister);
                }
                //Trễ
                if (inoutInValid.IntimeByDateTime > date.AddHours(Common.BeginShift).AddMinutes(Common.minuteLatePermit))
                {
                    RegisterLeave leaveRegister = new Models.Entity.RegisterLeave();
                    leaveRegister.DateRegister = DateTime.Today;
                    leaveRegister.DateApprove = DateTime.Today;
                    leaveRegister.DateStart = date.AddHours(Common.BeginShift);

                    DateTime dateEnd = date.AddHours(Common.EndShift);
                    double registerHours = 0;
                    RoundLate(inoutInValid.IntimeByDateTime.Value, out dateEnd, out registerHours);

                    leaveRegister.DateEnd = dateEnd;
                    leaveRegister.LeaveTypeId = leaveTypeAnnualId;
                    leaveRegister.Reason = "Auto created by late";
                    leaveRegister.RegisterHour = registerHours;
                    leaveRegister.Status = e_approved;
                    leaveRegister.Uid = inoutInValid.Uid;
                    leaveRegister.UserApprove = int.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    lstLeaveResult.Add(leaveRegister);
                }
                //Sớm
                if (inoutInValid.OuttimeByDateTime < date.AddHours(Common.EndShift).AddMinutes(-Common.minuteEarlyPermit))
                {
                    RegisterLeave leaveRegister = new Models.Entity.RegisterLeave();
                    leaveRegister.DateRegister = DateTime.Today;
                    leaveRegister.DateApprove = DateTime.Today;

                    DateTime dateStart = date.AddHours(Common.BeginShift);
                    double registerHours = 0;
                    RoundEarly(inoutInValid.OuttimeByDateTime.Value, out dateStart, out registerHours);

                    leaveRegister.DateStart = dateStart;
                    leaveRegister.DateEnd = date.AddHours(Common.EndShift); ;
                    leaveRegister.LeaveTypeId = leaveTypeAnnualId;
                    leaveRegister.Reason = "Auto created by Early";
                    leaveRegister.RegisterHour = registerHours;
                    leaveRegister.Status = e_approved;
                    leaveRegister.Uid = inoutInValid.Uid;
                    leaveRegister.UserApprove = int.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    lstLeaveResult.Add(leaveRegister);
                }
            }
            if (lstLeaveResult.Count > 0)
            {
                context.RegisterLeaves.AddRange(lstLeaveResult);
                context.SaveChanges();
            }
            DisposeContext(context);
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

            DateTime start = timeStart.Date.AddHours(Common.BeginShift);
            DateTime end = timeStart.Date.AddHours(Common.EndShift);
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
                result = Math.Round((end - start).TotalHours, 2);
            }
            //check có nằm trong khoảng giờ trưa hay không
            else if (start < endBreak && start > startBreak)
            {
                start = endBreak;
                result = Math.Round((end - start).TotalHours, 2);
            }
            else if (end > startBreak && end < endBreak)
            {
                end = startBreak;
                result = Math.Round((end - start).TotalHours, 2);
            }
            else
            {
                result = Math.Round((end - start).TotalHours - 1, 2);
            }
            double timeminus = 0;
            if (timeStart.Hour <= 8 && timeEnd.Hour >= 17)
            {
                timeminus = 0;
            }
            else
            {
                timeminus = 0.25;
            }
            result = result - 0.25;
            if (result > 8)
                result = 8;
            return result;
        }

        private List<ResponseLeave> PrivateGetLeave(DateTime dateStart, DateTime dateEnd, List<int> lstUid)
        {
            if (dateStart == DateTime.MinValue && dateEnd == DateTime.MinValue)
            {
                return new List<ResponseLeave>();
            }

            if (dateStart.Year != dateEnd.Year || dateStart.Month != dateEnd.Month)
            {
                throw new Exception("Please select range date in month!");
            }
            dateEnd = dateEnd.Date.AddDays(1).AddMilliseconds(-1);
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
                resLeave.LeaveStatus = Common.ConvertLeaveStatusToString((int)obj.LeaveStatus);
                resLeave.LeaveTypeName = obj.LeaveTypeName;
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
            if (lstLeave.Any(m => m.Status != Common.StatusLeave.E_Register && m.Status != Common.StatusLeave.E_Reject) && IsValidate == true)
            {
                DisposeContext(context);
                throw new Exception("Please delete only value register and disapprove status!");
            }
            context.RegisterLeaves.RemoveRange(lstLeave);
            context.SaveChanges();
            DisposeContext(context);
        }

        private void PrivateDeleteLeave(List<int> lstIds)
        {
            if (lstIds == null || lstIds.Count == 0)
                return;
            InitContext(out context);
            var lstLeave = context.RegisterLeaves.Where(m => lstIds.Contains(m.Id)).ToList();
            if (lstLeave.Any(m => m.Status != Common.StatusLeave.E_Register && m.Status != Common.StatusLeave.E_Reject))
            {
                DisposeContext(context);
                throw new Exception("Please delete only value register and disapprove status!");
            }
            context.RegisterLeaves.RemoveRange(lstLeave);
            context.SaveChanges();
            DisposeContext(context);
        }



        private void PrivateApproveLeave(List<int> lstleaveId)
        {
            if (lstleaveId == null || lstleaveId.Count == 0)
                return;
            InitContext(out context);
            var lstleave = context.RegisterLeaves.Where(m => lstleaveId.Contains(m.Id)).ToList();
            lstleave.ForEach(m => m.Status = Common.StatusLeave.E_Approve);
            context.SaveChanges();
            DisposeContext(context);
        }
        private void PrivateRejectLeave(List<int> lstleaveId)
        {
            if (lstleaveId == null || lstleaveId.Count == 0)
                return;
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

        public void RoundLate(DateTime inTime, out DateTime registerLate, out double HourRegister)
        {
            DateTime beginShift = inTime.Date.AddHours(Common.BeginShift);

            if (inTime.Minute >= 0 && inTime.Minute < 15)
            {
                registerLate = inTime.Date.AddHours(inTime.Hour).AddMinutes(15);
                HourRegister = Math.Round((double)(registerLate.AddMinutes(-15) - beginShift).Minutes / 60, 2);
            }
            else if (inTime.Minute >= 15 && inTime.Minute < 30)
            {
                registerLate = inTime.Date.AddHours(inTime.Hour).AddMinutes(30);
                HourRegister = Math.Round((double)(registerLate.AddMinutes(-15) - beginShift).Minutes / 60, 2);
            }
            else if (inTime.Minute >= 30 && inTime.Minute < 45)
            {
                registerLate = inTime.Date.AddHours(inTime.Hour).AddMinutes(45);
                HourRegister = Math.Round((double)(registerLate.AddMinutes(-15) - beginShift).Minutes / 60, 2);
            }
            else
            {
                registerLate = inTime.Date.AddHours(inTime.Hour).AddMinutes(60);
                HourRegister = Math.Round((double)(registerLate.AddMinutes(-15) - beginShift).Minutes / 60, 2);
            }
        }

        public void RoundEarly(DateTime outTime, out DateTime registerEarly, out double HourRegister)
        {
            DateTime endShift = outTime.Date.AddHours(Common.EndShift);

            if (outTime.Minute >= 0 && outTime.Minute < 15)
            {
                registerEarly = outTime.Date.AddHours(outTime.Hour).AddMinutes(0);
                HourRegister = (endShift - registerEarly.AddMinutes(15)).Hours;
            }
            else if (outTime.Minute >= 15 && outTime.Minute < 30)
            {
                registerEarly = outTime.Date.AddHours(outTime.Hour).AddMinutes(15);
                HourRegister = (endShift - registerEarly.AddMinutes(15)).Hours;
            }
            else if (outTime.Minute >= 30 && outTime.Minute < 45)
            {
                registerEarly = outTime.Date.AddHours(outTime.Hour).AddMinutes(30);
                HourRegister = (endShift - registerEarly.AddMinutes(15)).Hours;
            }
            else
            {
                registerEarly = outTime.Date.AddHours(outTime.Hour).AddMinutes(45);
                HourRegister = (endShift - registerEarly.AddMinutes(15)).Hours;
            }
        }
        #endregion
    }
}