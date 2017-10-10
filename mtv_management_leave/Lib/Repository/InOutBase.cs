using System;
using System.Collections.Generic;
using System.Linq;
using mtv_management_leave.Lib.Interface;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class InOutBase : Base, IInOut
    {
        LeaveManagementContext context;

        public InOutBase()
        {
        }

        public void DeleteInOut(List<int> lstId)
        {
            InitContext(out context);
            var lstDelete = context.InOuts.Where(m => lstId.Contains(m.Id)).ToList();
            context.InOuts.RemoveRange(lstDelete);
            context.SaveChanges();
            DisposeContext(context);
        }

        public void DeleteInOut(DateTime dateStart, DateTime dateEnd, List<int> lstUid)
        {
            InitContext(out context);
            var lstDelete = context.InOuts.Where(m => m.Date >= dateStart && m.Date <= dateEnd && lstUid.Contains(m.Uid)).ToList();
            context.InOuts.RemoveRange(lstDelete);
            context.SaveChanges();
            DisposeContext(context);
        }

        /// <summary>
        /// only error data
        /// </summary>
        /// <param name="DateStart"></param>
        /// <param name="DateEnd"></param>
        /// <returns></returns>
        public List<RepoMappingInOut> MappingInoutInValid(DateTime DateStart, DateTime DateEnd)
        {
            return MappingInoutLeave(DateStart, DateEnd).Where(m => m.IsValid == false).ToList();
        }

        /// <summary>
        /// only error Data
        /// </summary>
        /// <param name="DateStart"></param>
        /// <param name="DateEnd"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<RepoMappingInOut> MappingInoutInvalid(DateTime DateStart, DateTime DateEnd, List<int> lstUid)
        {
            return MappingInoutLeave(DateStart, DateEnd, lstUid).Where(m => m.IsValid == false).ToList();
        }

        /// <summary>
        /// xử lý toàn bộ nhân viên và toàn bộ ngày trong tháng
        /// </summary>
        /// <param name="MonthYear"></param>
        /// <returns></returns>
        public List<RepoMappingInOut> MappingInoutLeave(DateTime DateStart, DateTime DateEnd)
        {
            return CalculateMappingInoutLeave(DateStart, DateEnd, null);
        }
        public List<RepoMappingInOut> MappingInoutLeave(DateTime DateStart, DateTime DateEnd, List<int> lstUid)
        {
            return CalculateMappingInoutLeave(DateStart, DateEnd, lstUid);
        }

        public void SaveGenerateInout(DateTime dateFrom, DateTime? DateTo)
        {
            SaveDataGenerateInout(null, dateFrom, DateTo);
        }
        public void SaveGenerateInout(int uid, DateTime dateFrom, DateTime? DateTo)
        {
            SaveDataGenerateInout(uid, dateFrom, DateTo);
        }

        public void UpdateOrCreateInout(InOut obj)
        {
            InitContext(out context);
            var inoutDB = context.InOuts.Where(m => m.Uid == obj.Uid && m.Date == obj.Date).FirstOrDefault();
            if (inoutDB != null)
            {
                inoutDB.Intime = obj.Intime;
                inoutDB.OutTime = obj.OutTime;
            }
            else
            {
                context.InOuts.Add(obj);
            }
            context.SaveChanges();
            DisposeContext(context);

        }

        #region private method
        private List<RepoMappingInOut> CalculateMappingInoutLeave(DateTime DateStart, DateTime DateEnd, List<int> lstUid)
        {
            DateEnd = DateEnd.Date.AddDays(1).AddSeconds(-1);
            if (DateStart.Year != DateEnd.Year || DateStart.Month != DateEnd.Month)
            {
                throw new Exception("Please search data in 1 month");
            }
            InitContext(out context);
            List<RepoMappingInOut> lstResult = new List<RepoMappingInOut>();
            //get all userid
            var queryUser = context.Users.Where(m => m.DateResign == null || (m.DateResign != null && m.DateResign.Value >= DateStart)).Select(m => new { m.Id, m.DateResign, m.FullName });
            if (lstUid != null && lstUid.Count > 0)
            {
                queryUser = queryUser.Where(m => lstUid.Contains(m.Id));
            }
            var lstUser = queryUser.ToList();
            var lstUserId = lstUser.Select(m => m.Id).ToList();


            //van de thai san
            //van de nghi luon
            //van de ngay nghi cong ty 
            //thieu out
            // thieu in lan out
            // khong du gio in 
            // khong du gio out
            var lstdayoff = context.MasterLeaveDayCompanies.Where(m => m.Date >= DateStart && m.Date <= DateEnd).Select(m => m.Date).ToList();
            var lstInout = context.InOuts.Where(m => m.Date >= DateStart && m.Date <= DateEnd && lstUserId.Contains(m.Uid)).Select(m => new RepoInOut { Uid = m.Uid, Date = m.Date, Intime = m.Intime, OutTime = m.OutTime }).ToList();
            var lstLeave = context.RegisterLeaves.Where(m => m.DateStart <= DateEnd && m.DateEnd >= DateStart&& lstUserId.Contains(m.Uid)).Select(m => new RepoLeave()
            {
                Uid = m.Uid,
                DateRegister = m.DateRegister,
                DateStart = m.DateStart,
                DateEnd = m.DateEnd,
                LeaveCode = m.MasterLeaveType.LeaveCode,
                leaveName = m.MasterLeaveType.Name,
                RegisterHour = m.RegisterHour,
                leaveStatus = m.Status
            }).ToList();

            for (DateTime date = DateStart; date <= DateEnd; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || lstdayoff.Any(m => m.Date == date))
                {
                    continue;
                }

                DateTime beginShiftLate = date.AddHours(8).AddMinutes(Common.minuteLatePermit);
                DateTime beginShift = date.AddHours(8);
                DateTime endShift = date.AddHours(17);
                DateTime endShiftEarly = date.AddHours(17).AddMinutes(-Common.minuteEarlyPermit);


                foreach (var user in lstUser)
                {
                    if (user.DateResign != null && user.DateResign < date)
                    {
                        continue;
                    }
                    var inout = lstInout.Where(m => m.Uid == user.Id && m.Date == date).FirstOrDefault();
                    var lstleaveInDay = lstLeave.Where(m => m.Uid == user.Id && ((m.DateStart.Date <= date && m.DateEnd.Date >= date))).OrderBy(m => m.DateStart).ToList();
                    RepoMappingInOut mapping = new RepoMappingInOut();
                    mapping.Uid = user.Id;
                    mapping.FullName = user.FullName;
                    mapping.Date = date;
                    mapping.Intime = inout != null ? inout.Intime.ToString("HH:mm") : string.Empty;
                    mapping.Outtime = inout != null ? (inout.OutTime != null ? inout.OutTime.Value.ToString("HH:mm") : string.Empty) : string.Empty;

                    if (lstleaveInDay.Count == 0)
                    {
                        mapping.LeaveStart1 = string.Empty;
                        mapping.LeaveEnd1 = string.Empty;
                        mapping.LeaveType1 = string.Empty;
                    }
                    else if (lstleaveInDay.Count > 0)
                    {
                        var firstLeave = lstleaveInDay.FirstOrDefault();
                        mapping.LeaveStart1 = firstLeave.DateStart.ToString("yyyy-MM-dd HH:mm");
                        mapping.LeaveEnd1 = firstLeave.DateEnd.ToString("yyyy-MM-dd HH:mm");
                        mapping.LeaveType1 = firstLeave.leaveName;
                        mapping.LeaveStatus1 = firstLeave.leaveStatus.ToString();
                        if (lstleaveInDay.Count > 1)
                        {
                            var lastLeave = lstleaveInDay.LastOrDefault();
                            mapping.LeaveStart2 = lastLeave.DateStart.ToString("yyyy-MM-dd HH:mm");
                            mapping.LeaveEnd2 = lastLeave.DateEnd.ToString("yyyy-MM-dd HH:mm");
                            mapping.LeaveType2 = lastLeave.leaveName;
                            mapping.LeaveStatus2 = lastLeave.leaveStatus.ToString();
                        }
                    }


                    bool isValid = true;
                    double timeDiff = 0;
                    CalculateValid(inout, lstleaveInDay, beginShiftLate, beginShift, endShift, endShiftEarly, out isValid, out timeDiff);
                    mapping.IsValid = isValid;
                    mapping.TimeDiff = timeDiff;
                    lstResult.Add(mapping);
                }
            }


            DisposeContext(context);
            return lstResult;
        }
        private void CalculateValid(RepoInOut inout, List<RepoLeave> lstleave, DateTime beginShiftLate, DateTime beginShift, DateTime endShift, DateTime endShiftEarly, out bool isValid, out double timeDiff)
        {
            isValid = true;
            timeDiff = 0;
            var lstleaveApprove = lstleave.Where(m => m.leaveStatus == Common.StatusLeave.E_Approve).ToList();

            //ko co in out
            if (inout == null || inout.OutTime == null)
            {
                if (lstleaveApprove == null || lstleaveApprove.Count == 0) //ko co leave
                {
                    isValid = false;
                    timeDiff = 8;
                }
                else if (!lstleaveApprove.Any(m => m.leaveStatus == Common.StatusLeave.E_Approve && m.RegisterHour >= 8)) // leave chua duyet
                {
                    isValid = false;
                    timeDiff = 8;
                }
                else if (lstleaveApprove.Any(m => m.LeaveCode == Common.TypeLeave.E_Materity.ToString())) // loaij thai san
                {
                    isValid = true;
                    timeDiff = 0;
                }
                else if (lstleaveApprove.Where(m => m.leaveStatus == Common.StatusLeave.E_Approve).ToList().Sum(m => m.RegisterHour ?? 0) < 8) // dang ky leave duoi 8 tieng
                {
                    isValid = false;
                    timeDiff = (8 - lstleaveApprove.Where(m => m.leaveStatus == Common.StatusLeave.E_Approve).ToList().Sum(m => m.RegisterHour ?? 0)) * 60;
                }
            }
            else // co in out
            {
                if (inout.Intime > beginShiftLate || inout.OutTime < endShiftEarly) // co tre som
                {
                    //khong co leave
                    if (lstleaveApprove == null || lstleaveApprove.Count == 0)
                    {
                        isValid = false;
                        int diffBegin = (inout.Intime - beginShiftLate).Minutes;
                        diffBegin = diffBegin < 0 ? 0 : diffBegin;
                        int diffEnd = (endShift - inout.OutTime.Value).Minutes;
                        diffEnd = diffEnd < 0 ? 0 : diffEnd;
                        timeDiff = diffBegin + diffEnd;
                    }
                    //leave chua duyet
                    else if (lstleaveApprove.Any(m => m.LeaveCode == Common.TypeLeave.E_Materity.ToString()))
                    {

                    }
                    else
                    {
                        //leave map ko dung inout
                        //dau ca
                        if (inout.Intime > beginShiftLate)
                        {
                            var firstLeave = lstleaveApprove.FirstOrDefault();

                            if (firstLeave.DateStart > inout.Intime)
                            {
                                isValid = false;
                                timeDiff += (inout.Intime - beginShift).TotalMinutes;
                            }
                            else if (firstLeave.DateStart > beginShift || firstLeave.DateEnd < inout.Intime)
                            {
                                isValid = false;
                                timeDiff += (firstLeave.DateStart - beginShift).TotalMinutes < 0 ? 0 : (firstLeave.DateStart - beginShift).TotalMinutes;
                                timeDiff += (inout.Intime - firstLeave.DateEnd).TotalMinutes < 0 ? 0 : (inout.Intime - firstLeave.DateEnd).TotalMinutes;
                            }
                        }
                        // ve som
                        if (inout.OutTime < endShiftEarly)
                        {
                            var lastLeave = lstleaveApprove.LastOrDefault();
                            if (lastLeave.DateEnd < inout.OutTime)
                            {
                                isValid = false;
                                timeDiff += (endShift - inout.OutTime.Value).TotalMinutes;
                            }
                            else if (lastLeave.DateStart > inout.OutTime || lastLeave.DateEnd < endShift)
                            {
                                isValid = false;
                                timeDiff += (lastLeave.DateStart - inout.OutTime.Value).TotalMinutes < 0 ? 0 : (lastLeave.DateStart - inout.OutTime.Value).TotalMinutes;
                                timeDiff += (endShift - lastLeave.DateEnd).TotalMinutes < 0 ? 0 : (endShift - lastLeave.DateEnd).TotalMinutes;
                            }
                        }
                    }
                }
            }
        }

        private void SaveDataGenerateInout(int? uidInut, DateTime dateFrom, DateTime? DateTo)
        {
            InitContext(out context);
            if (DateTo == null)
            {
                DateTo = DateTime.Today;
            }
            DateTo = DateTo.Value.AddDays(1).AddSeconds(-1);
            //del old data
            var lstInoutInDB_Query = context.InOuts.Where(m => m.Date >= dateFrom && m.Date <= DateTo);
            var lstInoutRaw_Query = context.DataInOutRaws.Where(m => m.Uid == uidInut && m.Time >= dateFrom && m.Time <= DateTo).Select(m => new { m.Uid, m.Time });
            if (uidInut != null)
            {
                lstInoutInDB_Query = lstInoutInDB_Query.Where(m => m.Uid == uidInut);
                lstInoutRaw_Query = lstInoutRaw_Query.Where(m => m.Uid == uidInut);
            }

            var lstInoutInDB = lstInoutInDB_Query.ToList();
            var lstInoutDelete = lstInoutInDB.Where(m => m.IsModify == null || m.IsModify == false).ToList();
            var lstInOutModified = lstInoutInDB.Where(m => m.IsModify != null && m.IsModify == true).ToList();
            context.InOuts.RemoveRange(lstInoutDelete);

            var lstInoutRaw = lstInoutRaw_Query.ToList();
            var lstUid = lstInoutRaw.Select(m => m.Uid).Distinct();

            var listInoutSave = new List<InOut>();
            for (DateTime date = dateFrom; date <= DateTo; date = date.AddDays(1))
            {
                foreach (var uid in lstUid)
                {
                    if (lstInOutModified.Any(m => m.Uid == uid && m.Date == date))
                    {
                        continue; // ko tính toán lại dữ liệu đã được modify 
                    }

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
            public Common.StatusLeave leaveStatus { get; set; }
        }
        #endregion

    }
}