using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            DateTime dateEnd = new DateTime(2000, 1, 4);

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
