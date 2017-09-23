using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class LeaveTotalMonthly : Base, ILeaveTotalMonthly
    {
        LeaveManagementEntities context;
        ICommonLeaveBase commonLeaveBase;

        public LeaveTotalMonthly()
        {
            commonLeaveBase = new CommonLeaveBase();
        }

        public void GetLastTotalMonthly(int year)
        {
            throw new NotImplementedException();
        }

        public void GetLastTotalMonthly(int year, int uid)
        {
            throw new NotImplementedException();
        }

        public void SaveLastTotalMonthly(DateTime MonthTo)
        {
            throw new NotImplementedException();
        }

        public void SaveLastTotalMonthly(DateTime MonthTo, int uid)
        {
            throw new NotImplementedException();
        }

        public void ReCalculateTotalMonthly(DateTime MonthTo)
        {
            throw new NotImplementedException();
        }

        public void ReCalculateTotalMonthly(DateTime MonthTo, int uid)
        {
            throw new NotImplementedException();
        }

        public void CalculateTotalMonthly(DateTime MonthYear)
        {
            throw new NotImplementedException();
        }

        public void CalculateTotalMonthly(DateTime MonthYear, int uid)
        {
            throw new NotImplementedException();
        }
    }
}