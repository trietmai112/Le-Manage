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
    public class LeaveTotalMonthlyTest : BaseTest
    {
        [TestMethod]
        public void SaveLastTotalMonthly_NormalCase()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);

            DataBeginYearBase beginYear = new DataBeginYearBase();
            beginYear.SaveDataBeginYear(new DataBeginYear() { AnnualLeave = 10, DateBegin = new DateTime(2000, 1, 1), Uid = 1 });

            UserSeniorityBase userSeniorityBase = new UserSeniorityBase();
            userSeniorityBase.SaveUserSeniority(new UserSeniority() { Uid = 1, Month1 = 0, Month2 = 3, Month3 = 3, Month4 = 3, Month5 = 3, Month6 = 3, Month7 = 3, Month8 = 3, Month9 = 3, Month10 = 3, Month11 = 3, Month12 = 3, Year = dateValid.Year });

            AddLeaveBase addLeaveBase = new AddLeaveBase();
            addLeaveBase.DeleteAddLeaveBonus(new DateTime(2000, 1, 1), new DateTime(2000, 1, 10));
            addLeaveBase.SaveAddLeaveBonus(new AddLeave() { AddLeaveHour = 8, DateAdd = new DateTime(2000, 1, 10), Reason = "test", Uid = 1, });

            LeaveTotalMonthlyBase totalMonthlyBase = new LeaveTotalMonthlyBase();

            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddDays(3).AddHours(17));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(new DateTime(rleave.DateEnd.Year, 1, 1), rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                var lstLeaveInsert = leave.GetLeave(new DateTime(rleave.DateEnd.Year, 1, 1), rleave.DateEnd, new List<int>() { rleave.Uid }).Select(m=>m.Id).ToList();
                leave.ApproveLeave(lstLeaveInsert);
                totalMonthlyBase.SaveLastTotalMonthly(new DateTime(dateValid.Year, 1, 1), new List<int>() { 1 });
                var data = totalMonthlyBase.GetLastTotalMonthly(new DateTime(dateValid.Year, 1, 1), new List<int>() { 1 }).FirstOrDefault();

                Assert.IsTrue(data.LeaveAvailable == 18);
                Assert.IsTrue(data.LeaveNonPaid == null);
                Assert.IsTrue(data.LeaveRemain == -14);
                Assert.IsTrue(data.LeaveUsed == 32);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ReCalculateTotalMonthlyAllYear_NormalCase()
        {
            DateTime dateValid = new DateTime(2000, 1, 3);

            DataBeginYearBase beginYear = new DataBeginYearBase();
            beginYear.SaveDataBeginYear(new DataBeginYear() { AnnualLeave = 10, DateBegin = new DateTime(2000, 1, 1), Uid = 1 });

            UserSeniorityBase userSeniorityBase = new UserSeniorityBase();
            userSeniorityBase.SaveUserSeniority(new UserSeniority() { Uid = 1, Month1 = 0, Month2 = 3, Month3 = 3, Month4 = 3, Month5 = 3, Month6 = 3, Month7 = 3, Month8 = 3, Month9 = 3, Month10 = 3, Month11 = 3, Month12 = 3, Year = dateValid.Year });

            AddLeaveBase addLeaveBase = new AddLeaveBase();
            addLeaveBase.DeleteAddLeaveBonus(new DateTime(2000, 1, 1), new DateTime(2000, 1, 10));
            addLeaveBase.SaveAddLeaveBonus(new AddLeave() { AddLeaveHour = 8, DateAdd = new DateTime(2000, 1, 10), Reason = "test", Uid = 1, });

            LeaveTotalMonthlyBase totalMonthlyBase = new LeaveTotalMonthlyBase();

            var rleave = createLeave(dateValid.AddHours(8), dateValid.AddDays(3).AddHours(17));
            LeaveBase leave = new LeaveBase();
            try
            {
                leave.DeleteLeaveWithoutValidate(new DateTime(rleave.DateEnd.Year, 1, 1), rleave.DateEnd, new List<int>() { rleave.Uid });
                leave.RegisterLeave(rleave);
                var lstLeaveInsert = leave.GetLeave(new DateTime(rleave.DateEnd.Year, 1, 1), rleave.DateEnd, new List<int>() { rleave.Uid }).Select(m => m.Id).ToList();
                leave.ApproveLeave(lstLeaveInsert);
                totalMonthlyBase.ReCalculateTotalMonthlyAllYear(new DateTime(dateValid.Year, 10, 1), new List<int>() { 1 });
                var data = totalMonthlyBase.GetLastTotalMonthly(new DateTime(dateValid.Year, 10, 1), new List<int>() { 1 }).FirstOrDefault();

                Assert.IsTrue(data.LeaveAvailable == -11);
                Assert.IsTrue(data.LeaveNonPaid == null);
                Assert.IsTrue(data.LeaveRemain == -11);
                Assert.IsTrue(data.LeaveUsed == 0);
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
