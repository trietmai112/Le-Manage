using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mtv_management_leave.Lib
{
    public static class Common
    {
        public enum DateTimePickerViewMode
        {
            Full,
            Date,
            Time,
            Year,
            Month,
            Day
        }

        public enum TypeLeave
        {
            E_AnnualLeave = 1,
            E_BussinessLeave = 2,
            E_Materity = 3,
            E_NonPaid = 4
            
        }
        public enum StatusLeave
        {
            E_Approve = 1,
            E_Register = 2,
            E_Reject = 3
        }

        public enum AddLeaveType
        {
            E_Overtime = 1,
            E_Pregnant = 2,
            E_FatherBaby = 3,
            E_Compensating = 4
        }

        public static int minuteLatePermit = 45;
        public static int minuteEarlyPermit = 15;

        public static string ConvertLeaveStatusToString(int leaveStatus)
        {
            string result = string.Empty;
            switch (leaveStatus)
            {
                case (int) StatusLeave.E_Approve: result = "Approved"; break;
                case (int) StatusLeave.E_Register: result = "Register"; break;
                case (int) StatusLeave.E_Reject: result =  "DisApproved"; break;
                default: result = "Register"; break;
            }
            return result;
        }

        public static string ConvertLeaveStatusToString(string leaveStatus)
        {
            string result = string.Empty;
            string E_Approve = StatusLeave.E_Approve.ToString();
            string E_Register = StatusLeave.E_Register.ToString();
            string E_Reject = StatusLeave.E_Reject.ToString();
            if(leaveStatus == E_Approve)
            {
                result = "Approved";
            }else if(leaveStatus == E_Register)
            {
                result = "Register";
            }
            else if(leaveStatus == E_Reject)
            {
                result = "DisApproved";
            }
            else
            {
                result = "Register";
            }
            return result;
        }
        public static string ConvertLeaveTypeToString(string leaveType)
        {
            string result = string.Empty;
            if (leaveType == TypeLeave.E_AnnualLeave.ToString())
                result = "Annual";
            else if (leaveType == TypeLeave.E_BussinessLeave.ToString())
                result = "Bussiness";
            else if (leaveType == TypeLeave.E_Materity.ToString())
                result = "Materity";
            else if (leaveType == TypeLeave.E_NonPaid.ToString())
                result = "NonPaid";
            return result;
        }
    }
}