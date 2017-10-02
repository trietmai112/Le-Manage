using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mtv_management_leave.Lib
{
    public static class Common
    {
        public enum TypeLeave
        {
            E_CompanyStrip = 1,
            E_AnnualLeave = 2,
            E_SickLeave = 3,
            E_Materity = 4,
            E_NonPaid = 5,
            E_Other = 6
        }
        public enum StatusLeave
        {
            E_Approve = 1,
            E_Register = 2,
            E_Reject = 3
        }

        public static int minuteLatePermit = 45;
        public static int minuteEarlyPermit = 15;
    }
}