using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mtv_management_leave.Lib;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Tests.LibTest;

namespace mtv_management_leave.Tests.LeaveTest
{
    [TestClass]
    public class InOutTest : BaseTest
    {
        [TestMethod]
        public void MappingInoutLeave_NormalCase_UserResign()
        {
            //update Profile DateResign
            DateTime dateStart = new DateTime(2000, 1, 3);
            DateTime dateEnd = new DateTime(2000, 1, 4);

            UpdateUserResign(dateEnd);
            //UpdateInout
            InOutBase inoutBase = new InOutBase();
            for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
            {
                inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = date.AddHours(8), OutTime = date.AddHours(17), Date = date });
            }
            //Calculate over dateResign
            var lstresult = inoutBase.MappingInoutLeave(dateStart, dateEnd, new List<int>() { 1 });

            try
            {
                Assert.IsTrue(lstresult.Count == 2);
                Assert.IsTrue(lstresult.FirstOrDefault().IsValid == true);
                Assert.IsTrue(lstresult.LastOrDefault().IsValid == true);
            }
            catch
            {
                Assert.Fail();
            }
            UpdateUserResign(null);

        }

        [TestMethod]
        public void MappingInoutLeave_NormalCase_UserResignEarly()
        {
            //update Profile DateResign
            DateTime dateStart = new DateTime(2000, 1, 3);
            DateTime dateEnd = new DateTime(2000, 1, 4);

            UpdateUserResign(dateEnd);
            //UpdateInout
            InOutBase inoutBase = new InOutBase();
            for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
            {
                inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = date.AddHours(8), OutTime = date.AddHours(17), Date = date });
            }
            try
            {
                //Calculate over dateResign
                var lstresult = inoutBase.MappingInoutLeave(dateStart, dateEnd.AddDays(3), new List<int>() { 1 });
                Assert.IsTrue(lstresult.Count == 2);
            }
            catch
            {
                Assert.Fail();
            }
            UpdateUserResign(null);

        }


        [TestMethod]
        public void MappingInoutLeave_NormalCase_OverDayOff()
        {
            //update Profile DateResign
            DateTime dateStart = new DateTime(2000, 1, 3);
            DateTime dateEnd = new DateTime(2000, 1, 6);

            DayOffCompanyBase dayoff = new DayOffCompanyBase();
            dayoff.SaveLeaveDayCompany(new MasterLeaveDayCompany() { Date = dateStart.AddDays(1), Description = "test" });

            //UpdateInout
            InOutBase inoutBase = new InOutBase();
            for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
            {
                inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = date.AddHours(8), OutTime = date.AddHours(17), Date = date });
            }
            try
            {
                //Calculate over dateResign
                var lstresult = inoutBase.MappingInoutLeave(dateStart, dateEnd, new List<int>() { 1 });
                Assert.IsTrue(lstresult.Count == 3);
            }
            catch
            {
                Assert.Fail();
            }
            dayoff.DeleteLeaveDayCompany(dateStart, dateEnd);

        }

        [TestMethod]
        public void MappingInoutLeave_NormalCase_OverWeekend()
        {
            //update Profile DateResign
            DateTime dateStart = new DateTime(2000, 1, 1);
            DateTime dateEnd = new DateTime(2000, 1, 3);
            //UpdateInout
            InOutBase inoutBase = new InOutBase();
            for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
            {
                inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = date.AddHours(8), OutTime = date.AddHours(17), Date = date });
            }
            try
            {
                //Calculate over dateResign
                var lstresult = inoutBase.MappingInoutLeave(dateStart, dateEnd, new List<int>() { 1 });
                Assert.IsTrue(lstresult.Count == 1);
            }
            catch
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void MappingInoutLeave_NormalCase_LateOverPermit()
        {
            //update Profile DateResign
            DateTime dateStart1 = new DateTime(2000, 1, 3).AddHours(8).AddMinutes(45);
            DateTime dateEnd1 = new DateTime(2000, 1, 3).AddHours(17);
            DateTime dateStart2 = new DateTime(2000, 1, 4).AddHours(8).AddMinutes(46);
            DateTime dateEnd2 = new DateTime(2000, 1, 4).AddHours(17);
            //UpdateInout
            InOutBase inoutBase = new InOutBase();
            inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = dateStart1, OutTime = dateEnd1, Date = dateStart1.Date });
            inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = dateStart2, OutTime = dateEnd2, Date = dateStart2.Date });
            try
            {
                //Calculate over dateResign
                var lstresult = inoutBase.MappingInoutLeave(dateStart1.Date, dateEnd2.Date, new List<int>() { 1 });
                Assert.IsTrue(lstresult.Count == 2);
                Assert.IsTrue(lstresult[0].IsValid == true);
                Assert.IsTrue(lstresult[1].IsValid == false);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void MappingInoutLeave_NormalCase_EarlyOverPermit()
        {
            //update Profile DateResign
            DateTime dateStart1 = new DateTime(2000, 1, 3).AddHours(8);
            DateTime dateEnd1 = new DateTime(2000, 1, 3).AddHours(17).AddMinutes(-15);
            DateTime dateStart2 = new DateTime(2000, 1, 4).AddHours(8);
            DateTime dateEnd2 = new DateTime(2000, 1, 4).AddHours(17).AddMinutes(-16);
            //UpdateInout
            InOutBase inoutBase = new InOutBase();
            inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = dateStart1, OutTime = dateEnd1, Date = dateStart1.Date });
            inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = dateStart2, OutTime = dateEnd2, Date = dateStart2.Date });
            try
            {
                //Calculate over dateResign
                var lstresult = inoutBase.MappingInoutLeave(dateStart1.Date, dateEnd2.Date, new List<int>() { 1 });
                Assert.IsTrue(lstresult.Count == 2);
                Assert.IsTrue(lstresult[0].IsValid == true);
                Assert.IsTrue(lstresult[1].IsValid == false);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void MappingInoutLeave_NormalCase_MissOut()
        {
            //update Profile DateResign
            DateTime dateStart = new DateTime(2000, 1, 3).AddHours(8);
            DateTime dateEnd = new DateTime(2000, 1, 3).AddHours(17).AddMinutes(-15);
            //UpdateInout
            InOutBase inoutBase = new InOutBase();
            inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = dateStart, OutTime = null, Date = dateStart.Date });
            try
            {
                //Calculate over dateResign
                var lstresult = inoutBase.MappingInoutLeave(dateStart.Date, dateEnd.Date, new List<int>() { 1 });
                Assert.IsTrue(lstresult.Count == 1);
                Assert.IsTrue(lstresult[0].IsValid == false);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void MappingInoutLeave_NormalCase_MissInOut()
        {
            //update Profile DateResign
            DateTime dateStart = new DateTime(2000, 1, 3).AddHours(8);
            DateTime dateEnd = new DateTime(2000, 1, 3).AddHours(17).AddMinutes(-15);
            //UpdateInout
            InOutBase inoutBase = new InOutBase();
            inoutBase.DeleteInOut(dateStart.Date, dateEnd.Date, new List<int>() { 1 });
            try
            {
                //Calculate over dateResign
                var lstresult = inoutBase.MappingInoutLeave(dateStart.Date, dateEnd.Date, new List<int>() { 1 });
                Assert.IsTrue(lstresult.Count == 1);
                Assert.IsTrue(lstresult[0].IsValid == false);
            }
            catch
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void MappingInoutLeave_NormalCase_LateWithLeave_NonApprove()
        {
            //update Profile DateResign
            DateTime dateStart = new DateTime(2000, 1, 3).AddHours(8).AddMinutes(46);
            DateTime dateEnd = new DateTime(2000, 1, 3).AddHours(17);
            //UpdateInout
            InOutBase inoutBase = new InOutBase();
            LeaveBase leaveB = new LeaveBase();
            try
            {
                inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = dateStart, OutTime = null, Date = dateStart.Date });

                leaveB.RegisterLeave(new RegisterLeave()
                {
                    DateStart = new DateTime(2000, 1, 3).AddHours(8),
                    DateEnd = new DateTime(2000, 1, 3).AddHours(9),
                    LeaveTypeId = 1,
                    Uid = 1,
                    Status = Common.StatusLeave.E_Register

                });
                //Calculate over dateResign
                var lstresult = inoutBase.MappingInoutLeave(dateStart.Date, dateEnd.Date, new List<int>() { 1 });
                Assert.IsTrue(lstresult.Count == 1);
                Assert.IsTrue(lstresult[0].IsValid == false);
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                leaveB.DeleteLeaveWithoutValidate(dateStart.Date, dateEnd.Date, new List<int>() { 1 });
            }
        }

        [TestMethod]
        public void MappingInoutLeave_NormalCase_LateWithLeave_Approve()
        {
            //update Profile DateResign
            DateTime dateStart = new DateTime(2000, 1, 3).AddHours(8).AddMinutes(46);
            DateTime dateEnd = new DateTime(2000, 1, 3).AddHours(17);
            //UpdateInout
            InOutBase inoutBase = new InOutBase();
            LeaveBase leaveB = new LeaveBase();


            try
            {
                inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = dateStart, OutTime = dateEnd, Date = dateStart.Date });
                leaveB.RegisterLeave(new RegisterLeave()
                {
                    DateStart = new DateTime(2000, 1, 3).AddHours(8),
                    DateEnd = new DateTime(2000, 1, 3).AddHours(9),
                    LeaveTypeId = 1,
                    Uid = 1,
                    Status = Common.StatusLeave.E_Approve

                });
                //Calculate over dateResign
                var lstresult = inoutBase.MappingInoutLeave(dateStart.Date, dateEnd.Date, new List<int>() { 1 });
                Assert.IsTrue(lstresult.Count == 1);
                Assert.IsTrue(lstresult[0].IsValid == true);
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                leaveB.DeleteLeaveWithoutValidate(dateStart.Date, dateEnd.Date, new List<int>() { 1 });
            }
        }
        [TestMethod]
        public void MappingInoutLeave_NormalCase_LateWithLeaveNonMap()
        {
            //update Profile DateResign
            DateTime dateStart = new DateTime(2000, 1, 3).AddHours(8).AddMinutes(46);
            DateTime dateEnd = new DateTime(2000, 1, 3).AddHours(17);
            //UpdateInout
            InOutBase inoutBase = new InOutBase();
            LeaveBase leaveB = new LeaveBase();


            try
            {
                inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = dateStart, OutTime = dateEnd, Date = dateStart.Date });
                leaveB.RegisterLeave(new RegisterLeave()
                {
                    DateStart = new DateTime(2000, 1, 3).AddHours(9),
                    DateEnd = new DateTime(2000, 1, 3).AddHours(10),
                    LeaveTypeId = 1,
                    Uid = 1,
                    Status = Common.StatusLeave.E_Approve

                });
                //Calculate over dateResign
                var lstresult = inoutBase.MappingInoutLeave(dateStart.Date, dateEnd.Date, new List<int>() { 1 });
                Assert.IsTrue(lstresult.Count == 1);
                Assert.IsTrue(lstresult[0].IsValid == false);
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                leaveB.DeleteLeaveWithoutValidate(dateStart.Date, dateEnd.Date, new List<int>() { 1 });
            }
        }

        [TestMethod]
        public void MappingInoutLeave_NormalCase_EarlyWithLeave_Approve()
        {
            //update Profile DateResign
            DateTime dateStart = new DateTime(2000, 1, 3).AddHours(8);
            DateTime dateEnd = new DateTime(2000, 1, 3).AddHours(17).AddMinutes(-30);
            //UpdateInout
            InOutBase inoutBase = new InOutBase();
            LeaveBase leaveB = new LeaveBase();


            try
            {
                inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = dateStart, OutTime = dateEnd, Date = dateStart.Date });
                leaveB.RegisterLeave(new RegisterLeave()
                {
                    DateStart = new DateTime(2000, 1, 3).AddHours(17).AddMinutes(-30),
                    DateEnd = new DateTime(2000, 1, 3).AddHours(17),
                    LeaveTypeId = 1,
                    Uid = 1,
                    Status = Common.StatusLeave.E_Approve

                });
                //Calculate over dateResign
                var lstresult = inoutBase.MappingInoutLeave(dateStart.Date, dateEnd.Date, new List<int>() { 1 });
                Assert.IsTrue(lstresult.Count == 1);
                Assert.IsTrue(lstresult[0].IsValid == true);
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                leaveB.DeleteLeaveWithoutValidate(dateStart.Date, dateEnd.Date, new List<int>() { 1 });
            }
        }

        [TestMethod]
        public void SaveGenerateInout_NormalCase()
        {
            InOutBase iob = new InOutBase();
            try
            {
                iob.SaveGenerateInout(new DateTime(2000, 1, 1), null);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void ExportWorkingTime_NormalCase()
        {
            InOutBase inoutBase = new InOutBase();
            DateTime dateStart = new DateTime(2017, 11, 09);
            DateTime dateEnd = dateStart;
            for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
            {
                inoutBase.UpdateOrCreateInout(new InOut() { Uid = 1, Intime = date.AddHours(9), OutTime = date.AddHours(18), Date = date });
            }
            var data = inoutBase.ExportWorkingTime(dateStart,dateEnd, new List<int>() { 1 });
        }


        private void UpdateUserResign(DateTime? dateResign)
        {
            AccountBase account = new AccountBase();
            account.UpdateUserInfo(new RepoUserUpdateInfo()
            {
                Id = 1,
                FullName = "Test",
                DateBeginProbation = new DateTime(2000, 1, 1),
                DateBeginWork = new DateTime(2000, 1, 1),
                DateResign = dateResign,
                DateOfBirth = new DateTime(2000, 1, 1),
            });
        }
    }
}
