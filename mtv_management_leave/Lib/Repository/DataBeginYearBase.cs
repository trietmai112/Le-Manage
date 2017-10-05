using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class DataBeginYearBase : Base, IDataBeginYear
    {
        LeaveManagementContext context;

        public DataBeginYearBase()
        {
        }

        public void CalculateFromLastYear(DateTime MonthYear)
        {
            throw new NotImplementedException();
        }

        public void CalculateFromLastYear(DateTime MonthYear, int uid)
        {
            throw new NotImplementedException();
        }

        public void SaveDataBeginYear(DataBeginYear dataInput)
        {
            InitContext(out context);
            var dataBeginDB = context.DataBeginYears.Where(m => m.Uid == dataInput.Uid && m.DateBegin.Year == dataInput.DateBegin.Year).FirstOrDefault();
            if(dataBeginDB!= null)
            {
                dataBeginDB.AnnualLeave = dataInput.AnnualLeave;
                dataBeginDB.DateBegin = dataInput.DateBegin;
                dataBeginDB.AnnualLeave = dataInput.;
                dataBeginDB.AnnualLeave = dataInput.AnnualLeave;
                dataBeginDB.AnnualLeave = dataInput.AnnualLeave;
                dataBeginDB.AnnualLeave = dataInput.AnnualLeave;
                dataBeginDB.AnnualLeave = dataInput.AnnualLeave;
            }
            else
            {

            }

                DisposeContext(context);
        }
        
    }
}