using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class Base
    {
        public void InitContext(out LeaveManagementContext context)
        {
            context = new LeaveManagementContext();
        }
        public void DisposeContext(LeaveManagementContext context)
        {
            context.Dispose();
        }

        public void validate(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom == DateTime.MinValue || dateTo == DateTime.MinValue)
            {
                throw new Exception("Please select date From and date To");
            }
            if (dateFrom > dateTo)
            {
                throw new Exception("Please select date From less than date To");
            }
            if (dateFrom.Year != dateTo.Year || dateFrom.Month != dateTo.Month)
            {
                throw new Exception("Please select search data in 1 month!");
            }

        }
    }
}