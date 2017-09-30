using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class InOutBase : Base, IInOut
    {
        LeaveManagementEntities context;

        public InOutBase()
        {
        }

        public List<MappingInOut> MappingInoutInValid(DateTime DateStart, DateTime DateEnd)
        {
            return MappingInoutLeave(DateStart, DateEnd).Where(m => m.IsValid == false).ToList();
        }

        public List<MappingInOut> MappingInoutInvalid(DateTime DateStart, DateTime DateEnd, int uid)
        {
            return MappingInoutLeave(DateStart, DateEnd, uid).Where(m => m.IsValid == false).ToList();
        }

        /// <summary>
        /// xử lý toàn bộ nhân viên và toàn bộ ngày trong tháng
        /// </summary>
        /// <param name="MonthYear"></param>
        /// <returns></returns>
        public List<MappingInOut> MappingInoutLeave(DateTime DateStart, DateTime DateEnd)
        {
            return CalculateMappingInoutLeave(DateStart, DateEnd, null);
        }
        public List<MappingInOut> MappingInoutLeave(DateTime DateStart, DateTime DateEnd, int uid)
        {
            return CalculateMappingInoutLeave(DateStart, DateEnd, uid);
        }

        public void SaveGenerateInout(DateTime dateFrom, DateTime? DateTo)
        {
            InitContext(out context);
            if (DateTo == null)
            {
                DateTo = DateTime.Today;
            }
            DateTo = DateTo.Value.AddDays(1).AddSeconds(-1);
            //del old data
            var lstInoutDelete = context.InOuts.Where(m => m.Date >= dateFrom && m.Date <= DateTo).ToList();
            context.InOuts.RemoveRange(lstInoutDelete);
            //create new data

            var lstInoutRaw = context.DataInOutRaws.Where(m => m.Time >= dateFrom && m.Time <= DateTo).Select(m => new { m.Uid, m.Time });
            var lstUid = lstInoutRaw.Select(m => m.Uid).Distinct();
            var listInoutSave = new List<InOut>();
            for (DateTime date = dateFrom; date <= DateTo; date = date.AddDays(1))
            {
                foreach (var uid in lstUid)
                {
                    var lstInDayByUser = lstInoutRaw.Where(m => m.Uid == uid && m.Time.Date == date).OrderBy(m => m.Time);
                    if (lstInDayByUser == null || lstInDayByUser.Count() == 0)
                    {
                        continue;
                    }
                    InOut inOutObject = new InOut();
                    inOutObject.Intime = lstInDayByUser.FirstOrDefault().Time;
                    if (lstInDayByUser.Count() > 1)
                    {
                        inOutObject.OutTime = lstInDayByUser.LastOrDefault().Time;
                    }
                    inOutObject.Uid = uid;
                    listInoutSave.Add(inOutObject);
                }
            }
            if (listInoutSave.Count > 0)
            {
                context.InOuts.AddRange(listInoutSave);
            }
            context.SaveChanges();
            DisposeContext(context);
        }

        public void SaveGenerateInout(int uid, DateTime dateFrom, DateTime? DateTo)
        {
            InitContext(out context);
            if (DateTo == null)
            {
                DateTo = DateTime.Today;
            }
            DateTo = DateTo.Value.AddDays(1).AddSeconds(-1);
            //del old data
            var lstInoutDelete = context.InOuts.Where(m => m.Uid == uid && m.Date >= dateFrom && m.Date <= DateTo).ToList();
            context.InOuts.RemoveRange(lstInoutDelete);
            //create new data

            var lstInoutRaw = context.DataInOutRaws.Where(m => m.Uid == uid && m.Time >= dateFrom && m.Time <= DateTo).Select(m => new { m.Uid, m.Time });
            var listInoutSave = new List<InOut>();
            for (DateTime date = dateFrom; date <= DateTo; date = date.AddDays(1))
            {
                var lstInDayByUser = lstInoutRaw.Where(m => m.Time.Date == date).OrderBy(m => m.Time);
                if (lstInDayByUser == null || lstInDayByUser.Count() == 0)
                {
                    continue;
                }
                InOut inOutObject = new InOut();
                inOutObject.Intime = lstInDayByUser.FirstOrDefault().Time;
                if (lstInDayByUser.Count() > 1)
                {
                    inOutObject.OutTime = lstInDayByUser.LastOrDefault().Time;
                }
                inOutObject.Uid = uid;
                listInoutSave.Add(inOutObject);
            }
            if (listInoutSave.Count > 0)
            {
                context.InOuts.AddRange(listInoutSave);
            }
            context.SaveChanges();
            DisposeContext(context);
        }

        #region private method
        private List<MappingInOut> CalculateMappingInoutLeave(DateTime DateStart, DateTime DateEnd, int? uid)
        {
            if (DateStart.Year != DateEnd.Year || DateStart.Month != DateEnd.Month)
            {
                throw new Exception("Vui lòng nhập từ và đến trong 1 tháng");
            }
            InitContext(out context);
            List<MappingInOut> lstResult = new List<MappingInOut>();
            //get all userid
            var queryUser = context.UserInfoes.Where(m => m.DateResign == null || (m.DateResign != null && m.DateResign.Value >= DateStart)).Select(m => new { m.Uid, m.DateResign, m.FullName });
            if (uid != null)
            {
                queryUser = queryUser.Where(m => m.Uid == uid.Value);
            }
            var lstUser = queryUser.ToList();


            //van de thai san
            //van de nghi luon
            //van de ngay nghi cong ty 
            //thieu out
            // thieu in lan out
            // khong du gio in 
            // khong du gio out
            var lstdayoff = context.MasterLeaveDayCompanies.Where(m => m.Date >= DateStart && m.Date <= DateEnd).Select(m => m.Date).ToList();
            var lstInout = context.InOuts.Where(m => m.Date >= DateStart && m.Date <= DateEnd).Select(m => new RepoInOut { Uid = m.Uid, Date = m.Date, Intime = m.Intime, OutTime = m.OutTime });
            var lstLeave = context.RegisterLeaves.Where(m => m.DateRegister >= DateStart && m.DateRegister <= DateEnd).Select(m => new RepoLeave()
            {
                Uid = m.Uid,
                DateRegister = m.DateRegister,
                DateStart = m.DateStart,
                DateEnd = m.DateEnd,
                LeaveCode = m.MasterLeaveType.LeaveCode,
                leaveName = m.MasterLeaveType.Name,
                RegisterHour = m.RegisterHour,
                leaveStatus = m.Status
            });

            for (DateTime date = DateStart; date <= DateEnd; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || lstdayoff.Any(m => m.Date == date))
                {
                    continue;
                }

                DateTime beginShiftLate = date.AddHours(8).AddMinutes(Common.minuteLatePermit);
                DateTime beginShift = date.AddHours(8);
                DateTime endShift = date.AddHours(17).AddMinutes(15).AddMinutes(-Common.minuteEarlyPermit);


                foreach (var user in lstUser)
                {
                    if (user.DateResign != null && user.DateResign > date)
                    {
                        continue;
                    }
                    var inout = lstInout.Where(m => m.Uid == user.Uid && m.Date == date).FirstOrDefault();
                    var leave = lstLeave.Where(m => m.Uid == user.Uid && (m.DateRegister == date || (m.DateStart <= date && m.DateEnd >= date))).FirstOrDefault();
                    MappingInOut mapping = new MappingInOut();
                    mapping.Uid = user.Uid;
                    mapping.FullName = user.FullName;
                    mapping.Date = date;
                    mapping.Intime = inout != null ? inout.Intime.ToString("HH:mm") : string.Empty;
                    mapping.Outtime = inout != null ? (inout.OutTime != null ? inout.OutTime.Value.ToString("HH:mm") : string.Empty) : string.Empty;
                    mapping.LeaveStart = leave != null ? leave.DateStart.ToString("HH:mm") : string.Empty;
                    mapping.LeaveEnd = leave != null ? leave.DateEnd.ToString("HH:mm") : string.Empty;
                    mapping.LeaveType = leave != null ? leave.leaveName : string.Empty;
                    bool isValid = true;
                    double timeDiff = 0;
                    CalculateValid(inout, leave, beginShiftLate, beginShift, endShift, out isValid, out timeDiff);
                    mapping.IsValid = isValid;
                    mapping.TimeDiff = timeDiff;
                    lstResult.Add(mapping);
                }
            }


            DisposeContext(context);
            return lstResult;
        }
        private void CalculateValid(RepoInOut inout, RepoLeave leave, DateTime beginShiftLate, DateTime beginShift, DateTime endShift, out bool isValid, out double timeDiff)
        {
            isValid = true;
            timeDiff = 0;
            //ko co in out
            if (inout == null || inout.OutTime == null)
            {
                if (leave == null) //ko co leave
                {
                    isValid = false;
                    timeDiff = 8;
                }
                else if (leave.leaveStatus != (int)Common.StatusLeave.E_Approve) // leave chua duyet
                {
                    isValid = false;
                    timeDiff = 8;
                }
                else if (leave.LeaveCode == Common.TypeLeave.E_Materity.ToString()) // loaij thai san
                {
                    isValid = true;
                    timeDiff = 0;
                }
                else if (leave.RegisterHour < 8) // dang ky leave duoi 8 tieng
                {
                    isValid = false;
                    int diffBegin = (leave.DateStart - beginShiftLate).Minutes;
                    diffBegin = diffBegin < 0 ? 0 : diffBegin;
                    int diffEnd = (endShift - leave.DateEnd).Minutes;
                    diffEnd = diffEnd < 0 ? 0 : diffEnd;
                    timeDiff = diffBegin + diffEnd;
                }
            }
            else // co in out
            {
                if (inout.Intime > beginShiftLate || inout.OutTime < endShift) // co tre som
                {
                    //khong co leave
                    if (leave == null)
                    {
                        isValid = false;
                        int diffBegin = (inout.Intime - beginShiftLate).Minutes;
                        diffBegin = diffBegin < 0 ? 0 : diffBegin;
                        int diffEnd = (endShift - inout.OutTime.Value).Minutes;
                        diffEnd = diffEnd < 0 ? 0 : diffEnd;
                        timeDiff = diffBegin + diffEnd;
                    }
                    //leave chua duyet
                    else if (leave.leaveStatus != (int)Common.StatusLeave.E_Approve)
                    {
                        isValid = false;
                        int diffBegin = (inout.Intime - beginShiftLate).Minutes;
                        diffBegin = diffBegin < 0 ? 0 : diffBegin;
                        int diffEnd = (endShift - inout.OutTime.Value).Minutes;
                        diffEnd = diffEnd < 0 ? 0 : diffEnd;
                        timeDiff = diffBegin + diffEnd;
                    }
                    //leave loai thai san
                    else if (leave.LeaveCode == Common.TypeLeave.E_Materity.ToString())
                    {

                    }
                    else
                    {
                        //leave map ko dung inout
                        //dau ca
                        if (inout.Intime > beginShiftLate)
                        {
                            // map nhau
                            if (beginShift <= leave.DateEnd && inout.Intime >= leave.DateStart)
                            {
                                var minutes = ((inout.Intime < leave.DateStart ? inout.Intime : leave.DateStart) - beginShift).TotalMinutes;
                                minutes = minutes < 0 ? 0 : minutes;
                                timeDiff = minutes;
                                if (timeDiff > 0)
                                {
                                    isValid = false;
                                }
                            }
                            // khong map nhau
                            else
                            {
                                isValid = false;
                                timeDiff = (inout.Intime - beginShift).TotalMinutes;
                            }

                        }
                        // ve som
                        if (inout.OutTime < endShift)
                        {
                            //map nhau
                            if (inout.OutTime <= leave.DateEnd && endShift >= leave.DateStart)
                            {
                                var minutes = (endShift - (leave.DateEnd > inout.OutTime ? leave.DateEnd : inout.OutTime.Value)).TotalMinutes;
                                minutes = minutes < 0 ? 0 : minutes;
                                if (minutes > 0)
                                {
                                    timeDiff = minutes;
                                    isValid = false;
                                }

                            }
                            // khong map nhau
                            else
                            {
                                isValid = false;
                                timeDiff = (endShift - inout.OutTime.Value).TotalMinutes;
                            }
                        }
                    }
                }
            }
        }

        private class RepoInOut
        {
            public int Uid { get; set; }
            public DateTime Date { get; set; }
            public DateTime Intime { get; set; }
            public DateTime? OutTime { get; set; }
        }
        private class RepoLeave
        {
            public int Uid { get; set; }
            public DateTime DateRegister { get; set; }
            public DateTime DateStart { get; set; }
            public DateTime DateEnd { get; set; }
            public string LeaveCode { get; set; }
            public string leaveName { get; set; }
            public double? RegisterHour { get; set; }
            public byte? leaveStatus { get; set; }
        }
        #endregion

    }
}