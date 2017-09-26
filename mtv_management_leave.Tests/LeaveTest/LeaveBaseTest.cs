using Microsoft.VisualStudio.TestTools.UnitTesting;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Tests.LeaveTest
{
    [TestClass]
    public class LeaveBaseTest
    {
        [TestMethod]
        public void RegisterLeaveTest()
        {
            //System.Web.HttpContext.Current.User =
            RegisterLeave rleave = new RegisterLeave();
            rleave.Id = 1;
            rleave.DateStart = DateTime.Today;
            rleave.DateEnd = DateTime.Today;
            rleave.DateRegister = DateTime.Today;
            rleave.LeaveTypeId = 1;
            rleave.RegisterHour = 1;
            rleave.Uid = 1;
            rleave.UserCreated = 2;
            rleave.UserUpdated = 2;
            rleave.DateCreated = DateTime.Today.AddDays(-2);
            rleave.DateUpdated = DateTime.Today.AddDays(-2);
            LeaveBase leave = new LeaveBase();
            leave.RegisterLeave(rleave);
            Assert.IsTrue(true);
        }
        public void GetLeaveRemainTest()
        {

            LeaveBase leave = new LeaveBase();
            var remain =   leave.GetLeaveRemain(1, DateTime.Today);
        }
    }
}
