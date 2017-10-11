using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Tests.LibTest;

namespace mtv_management_leave.Tests.LeaveTest
{
    [TestClass]
    public class DataRawTest : BaseTest
    {
        [TestMethod]
        public void SaveDataRaw_NormalCase()
        {
            DataInOutRawDataRaw datar = new DataInOutRawDataRaw();
            DateTime? datetime1 = datar.getLastData();
            datar.SaveDataRaw();
            DateTime? datetime2 = datar.getLastData();

            if (datetime1 == null && datetime2 != null)
            { }
            else if (datetime1 == null && datetime2 == null)
            { }
            else if (datetime1 <= datetime2)
            { }
            else
            {
                Assert.Fail();
            }

        }

    }
}
