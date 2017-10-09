using Microsoft.VisualStudio.TestTools.UnitTesting;
using mtv_management_leave.Lib;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Tests.LibTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Tests.LeaveTest
{
    [TestClass]
    public class LeaveBaseTest : BaseTest
    {
        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_InDay_Normal()
        {
            //System.Web.HttpContext.Current.User =
            DateTime dateValid = new DateTime(1999, 1, 1);
            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddHours(17));
            
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
            Assert.IsTrue(leave.GetLeave(rleave.DateStart, rleave.DateEnd, new List<int>() { 1 }).Count == 1);
        }

        [TestMethod]
        public void RegisterLeave_NormalCase_MaternityLeave()
        {
            DateTime dateValid = new DateTime(1999, 1, 1);
            RegisterLeave rleave = new RegisterLeave();
            rleave.Id = 1;
            rleave.DateStart = dateValid;
            rleave.DateEnd = dateValid.AddMonths(6);
            rleave.DateRegister = dateValid;
            rleave.LeaveTypeId = (int)Common.TypeLeave.E_Materity;
            rleave.RegisterHour = 1;
            rleave.Status = Lib.Common.StatusLeave.E_Register;
            rleave.Uid = 1;
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
            Assert.IsTrue(leave.GetLeave(rleave.DateStart, rleave.DateStart.AddHours(1), new List<int>() { 1 }).Count == 1);
        }

        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_Saturday()
        {
            DateTime dateValid = new DateTime(2000, 1, 1);
            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddHours(17));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == "It's weekend");
            }
        }
        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_Sunday()
        {
            DateTime dateValid = new DateTime(2000, 1, 2);
            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddHours(17));
            
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == "It's weekend");
            }
        }

        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_DayOffCompany()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            DayOffCompanyBase dayOff = new DayOffCompanyBase();
            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddHours(17));
            
            LeaveBase leave = new LeaveBase();
            try
            {
                dayOff.SaveLeaveDayCompany(new MasterLeaveDayCompany() { Date = dateValid, Description = "testing" });
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
            }
            catch (Exception e)
            {
                dayOff.DeleteLeaveDayCompany(dateValid);
                Assert.IsTrue(e.Message == "It's a day off company");
            }
        }


        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_InBreakTime()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid.AddHours(12), dateValid.AddHours(13));

            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                Assert.IsTrue(leave.GetLeave(rleave.DateStart, rleave.DateStart.AddHours(1), new List<int>() { 1 }).FirstOrDefault().RegisterHour == 0);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_Map_StartBreakTime()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid.AddHours(11), dateValid.AddHours(13));

            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                Assert.IsTrue(leave.GetLeave(rleave.DateStart, rleave.DateStart.AddHours(1), new List<int>() { 1 }).FirstOrDefault().RegisterHour == 1);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_Map_EndBreakTime()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid.AddHours(12).AddMinutes(30), dateValid.AddHours(14));

            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                Assert.IsTrue(leave.GetLeave(rleave.DateStart, rleave.DateStart.AddHours(1), new List<int>() { 1 }).FirstOrDefault().RegisterHour == 1);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_OverBreakTime()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddHours(14));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                Assert.IsTrue(leave.GetLeave(rleave.DateStart, rleave.DateStart.AddHours(1), new List<int>() { 1 }).FirstOrDefault().RegisterHour == 5);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_MultiDate()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddDays(3).AddHours(17));

            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                Assert.IsTrue(leave.GetLeave(rleave.DateStart, rleave.DateEnd, new List<int>() { 1 }).Count == 4);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_MultiDate_DifferenceHours()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid, dateValid.AddDays(3).AddHours(20));

            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                Assert.IsTrue(leave.GetLeave(rleave.DateStart, rleave.DateEnd, new List<int>() { 1 }).Count == 4);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_MultiDate_CheckHours()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid, dateValid.AddDays(3).AddHours(20));

            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                var firstInsert = leave.GetLeave(rleave.DateStart, rleave.DateEnd, new List<int>() { 1 }).FirstOrDefault();
                Assert.IsTrue(firstInsert.LeaveFrom == rleave.DateStart.AddHours(8).ToString("yyyy-MM-dd HH:mm"));
                Assert.IsTrue(firstInsert.LeaveTo == rleave.DateStart.AddHours(17).ToString("yyyy-MM-dd HH:mm"));
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_MultiDate_Saturday_Sunday()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid, dateValid.AddDays(7).AddHours(20));

            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                Assert.IsTrue(leave.GetLeave(rleave.DateStart, rleave.DateEnd, new List<int>() { 1 }).Count == 6);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RegisterLeave_NormalCase_AnualLeave_MultiDate_DayOff()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            DayOffCompanyBase dayOff = new DayOffCompanyBase();
            var rleave = createLeave(dateValid, dateValid.AddDays(3).AddHours(20));

            LeaveBase leave = new LeaveBase();
            try
            {
                dayOff.SaveLeaveDayCompany(new MasterLeaveDayCompany() { Date = dateValid, Description = "testing" });
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                Assert.IsTrue(leave.GetLeave(rleave.DateStart, rleave.DateEnd, new List<int>() { 1 }).Count == 3);
                dayOff.DeleteLeaveDayCompany(dateValid);
            }
            catch (Exception e)
            {
                dayOff.DeleteLeaveDayCompany(dateValid);
                Assert.Fail();
            }
        }


        [TestMethod]
        public void RegisterLeave_AbNormalCase_AnualLeave_Duplicate()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid, dateValid.AddDays(3).AddHours(20));

            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                leave.RegisterLeave(rleave);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == "Duplicate Data!");
            }
        }


        [TestMethod]
        public void RegisterLeave_AbNormalCase_AnualLeave_Duplicate_AtStart()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid, dateValid.AddDays(3).AddHours(20));

            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                rleave.DateStart = dateValid.AddHours(7);
                rleave.DateEnd = dateValid.AddHours(8).AddMinutes(1);
                leave.RegisterLeave(rleave);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == "Duplicate Data!");
            }
        }
        [TestMethod]
        public void RegisterLeave_AbNormalCase_AnualLeave_Duplicate_AtEnd()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid, dateValid.AddDays(3).AddHours(20));

            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                rleave.DateStart = dateValid.AddHours(17).AddMinutes(-1);
                rleave.DateEnd = dateValid.AddHours(18);
                leave.RegisterLeave(rleave);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == "Duplicate Data!");
            }
        }


        [TestMethod]
        public void RegisterLeave_AbNormalCase_AnualLeave_OutTimeShift()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid, dateValid.AddHours(20));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == "Please register leave in 8:00 to 17:00!");
            }
        }

        [TestMethod]
        public void RegisterLeave_AbNormalCase_AnualLeave_OutTimeShift_AtBegin()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid, dateValid.AddHours(17));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == "Please register leave in 8:00 to 17:00!");
            }
        }
        [TestMethod]
        public void RegisterLeave_AbNormalCase_AnualLeave_OutTimeShift_AtEnd()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddHours(17).AddSeconds(1));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == "Please register leave in 8:00 to 17:00!");
            }
        }

        [TestMethod]
        public void ApproveLeave_NormalCase_OneLeave()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddHours(17));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                leave.ApproveLeave(rleave.Id);
                Assert.IsTrue(leave.GetLeave(dateValid, dateValid.AddDays(1).AddMinutes(-1)).FirstOrDefault().LeaveStatus == Common.StatusLeave.E_Approve);
            }
            catch (Exception e)
            {
            }
        }
        [TestMethod]
        public void ApproveLeave_NormalCase_MultiLeave()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);
            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddDays(3).AddHours(17));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                var lstLeaveNeedApprove = leave.GetLeave(rleave.DateStart, rleave.DateEnd).Select(m => m.Id).ToList();
                leave.ApproveLeave(lstLeaveNeedApprove);
                var lstLeaveNeedChecked = leave.GetLeave(rleave.DateStart, rleave.DateEnd).ToList();
                foreach (var item in lstLeaveNeedChecked)
                {
                    Assert.IsTrue(item.LeaveStatus == Common.StatusLeave.E_Approve);
                }
            }
            catch (Exception e)
            {
            }
        }
        [TestMethod]
        public void GetLeaveRemain_NormalCase()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);

            DataBeginYearBase beginYear = new DataBeginYearBase();
            beginYear.SaveDataBeginYear(new DataBeginYear() { AnnualLeave = 10, DateBegin = new DateTime(2000, 1, 1), Uid = 1 });

            UserSeniorityBase userSeniorityBase = new UserSeniorityBase();
            userSeniorityBase.SaveUserSeniority(new UserSeniority() { Uid = 1, Month1 = 3, Month2 = 3, Month3 = 3, Month4 = 3, Month5 = 3, Month6 = 3, Month7 = 3, Month8 = 3, Month9 = 3, Month10 = 3, Month11 = 3, Month12 = 3, Year = dateValid.Year });

            AddLeaveBase addLeaveBase = new AddLeaveBase();
            addLeaveBase.SaveAddLeaveBonus(new AddLeave() { AddLeaveHour = 8, DateAdd = new DateTime(2000, 1, 1), Reason = "test", Uid = 1, });



            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddDays(3).AddHours(17));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(new DateTime( rleave.DateEnd.Year,1,1), rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                var LeaveRemain = leave.GetLeaveRemain(rleave.Uid, rleave.DateEnd);

                Assert.IsTrue(LeaveRemain == -11);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetLeaveRemain_NormalCase_AddBonusLater()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);

            DataBeginYearBase beginYear = new DataBeginYearBase();
            beginYear.SaveDataBeginYear(new DataBeginYear() { AnnualLeave = 10, DateBegin = new DateTime(2000, 1, 1), Uid = 1 });

            UserSeniorityBase userSeniorityBase = new UserSeniorityBase();
            userSeniorityBase.SaveUserSeniority(new UserSeniority() { Uid = 1, Month1 = 3, Month2 = 3, Month3 = 3, Month4 = 3, Month5 = 3, Month6 = 3, Month7 = 3, Month8 = 3, Month9 = 3, Month10 = 3, Month11 = 3, Month12 = 3, Year = dateValid.Year });

            AddLeaveBase addLeaveBase = new AddLeaveBase();
            addLeaveBase.DeleteAddLeaveBonus(new DateTime(2000, 1, 1), new DateTime(2000, 1, 10));
            addLeaveBase.SaveAddLeaveBonus(new AddLeave() { AddLeaveHour = 8, DateAdd = new DateTime(2000, 1, 10), Reason = "test", Uid = 1, });



            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddDays(3).AddHours(17));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(new DateTime(rleave.DateEnd.Year, 1, 1), rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                var LeaveRemain = leave.GetLeaveRemain(rleave.Uid, rleave.DateEnd);

                Assert.IsTrue(LeaveRemain == -19);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void GetLeaveRemain_NormalCase_SeniorityLater()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);

            DataBeginYearBase beginYear = new DataBeginYearBase();
            beginYear.SaveDataBeginYear(new DataBeginYear() { AnnualLeave = 10, DateBegin = new DateTime(2000, 1, 1), Uid = 1 });

            UserSeniorityBase userSeniorityBase = new UserSeniorityBase();
            userSeniorityBase.SaveUserSeniority(new UserSeniority() { Uid = 1, Month1 = 0, Month2 = 3, Month3 = 3, Month4 = 3, Month5 = 3, Month6 = 3, Month7 = 3, Month8 = 3, Month9 = 3, Month10 = 3, Month11 = 3, Month12 = 3, Year = dateValid.Year });

            AddLeaveBase addLeaveBase = new AddLeaveBase();
            addLeaveBase.DeleteAddLeaveBonus(new DateTime(2000, 1, 1), new DateTime(2000, 1, 10));
            addLeaveBase.SaveAddLeaveBonus(new AddLeave() { AddLeaveHour = 8, DateAdd = new DateTime(2000, 1, 10), Reason = "test", Uid = 1, });



            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddDays(3).AddHours(17));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(new DateTime(rleave.DateEnd.Year, 1, 1), rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                var LeaveRemain = leave.GetLeaveRemain(rleave.Uid, rleave.DateEnd);

                Assert.IsTrue(LeaveRemain == -22);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetLeaveRemain_NormalCase_RejectStatus()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);

            DataBeginYearBase beginYear = new DataBeginYearBase();
            beginYear.SaveDataBeginYear(new DataBeginYear() { AnnualLeave = 10, DateBegin = new DateTime(2000, 1, 1), Uid = 1 });

            UserSeniorityBase userSeniorityBase = new UserSeniorityBase();
            userSeniorityBase.SaveUserSeniority(new UserSeniority() { Uid = 1, Month1 = 0, Month2 = 3, Month3 = 3, Month4 = 3, Month5 = 3, Month6 = 3, Month7 = 3, Month8 = 3, Month9 = 3, Month10 = 3, Month11 = 3, Month12 = 3, Year = dateValid.Year });

            AddLeaveBase addLeaveBase = new AddLeaveBase();
            addLeaveBase.DeleteAddLeaveBonus(new DateTime(2000, 1, 1), new DateTime(2000, 1, 10));
            addLeaveBase.SaveAddLeaveBonus(new AddLeave() { AddLeaveHour = 8, DateAdd = new DateTime(2000, 1, 10), Reason = "test", Uid = 1, });



            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddDays(3).AddHours(17));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(new DateTime(rleave.DateEnd.Year, 1, 1), rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                var lstleaveRegister = leave.GetLeave(rleave.DateStart, rleave.DateEnd, new List<int>() { rleave.Uid }).Select(m => m.Id).ToList();
                leave.RejectLeave(lstleaveRegister);


                var LeaveRemain = leave.GetLeaveRemain(rleave.Uid, rleave.DateEnd);

                Assert.IsTrue(LeaveRemain == 10);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        private RegisterLeave createLeave(DateTime dateStart, DateTime dateEnd)
        {
            RegisterLeave rleave = new RegisterLeave();
            rleave.Id = 1;
            rleave.DateStart = dateStart;
            rleave.DateEnd = dateEnd;
            rleave.LeaveTypeId = (int)Common.TypeLeave.E_AnnualLeave;
            rleave.RegisterHour = 1;
            rleave.Status = Lib.Common.StatusLeave.E_Register;
            rleave.Uid = 1;
            return rleave;
        }
    }
}
