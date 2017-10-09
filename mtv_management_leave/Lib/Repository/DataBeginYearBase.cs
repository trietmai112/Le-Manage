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
        ICommonLeaveBase commonLeave;
        public DataBeginYearBase()
        {
            commonLeave = new CommonLeaveBase();

        }

        public void CalculateFromLastYear(DateTime MonthYear)
        {
            privateCalculateDataBeginYear(MonthYear, null);
        }

        public void CalculateFromLastYear(DateTime MonthYear, List<int> lstUid)
        {
            privateCalculateDataBeginYear(MonthYear, lstUid);
        }

        public void SaveDataBeginYear(DataBeginYear dataInput)
        {
            InitContext(out context);
            var dataBeginDB = context.DataBeginYears.Where(m => m.Uid == dataInput.Uid && m.DateBegin.Year == dataInput.DateBegin.Year).FirstOrDefault();
            if (dataBeginDB != null)
            {
                dataBeginDB.AnnualLeave = dataInput.AnnualLeave;
                dataBeginDB.DateBegin = dataInput.DateBegin;
                dataBeginDB.Uid = dataInput.Uid;
            }
            else
            {
                context.DataBeginYears.Add(dataInput);
            }
            context.SaveChanges();
            DisposeContext(context);
        }

        public void SaveDataBeginYear(DataBeginYear dataInput, List<int> lstUid)
        {
            InitContext(out context);
            var dataBeginDB = context.DataBeginYears.Where(m => lstUid.Contains(m.Uid) && m.DateBegin.Year == dataInput.DateBegin.Year).ToList();
            List<DataBeginYear> dataNew = new List<DataBeginYear>();
            foreach (var uid in lstUid)
            {
                var dataAlready = dataBeginDB.Where(m => m.Uid == uid).FirstOrDefault();
                if(dataAlready!= null)
                {
                    dataAlready.DateBegin = dataInput.DateBegin;
                    dataAlready.AnnualLeave = dataInput.AnnualLeave;
                }
                else
                {
                    DataBeginYear dataInputByUser = new DataBeginYear();
                    dataInputByUser.Uid = uid;
                    dataInputByUser.DateBegin = dataInput.DateBegin;
                    dataInputByUser.AnnualLeave = dataInput.AnnualLeave;
                    context.DataBeginYears.Add(dataInputByUser);
                }

            }
            context.SaveChanges();
            DisposeContext(context);
        }

        public List<DataBeginYear> GetDataBeginYear(int year)
        {
            return privateGetDataBeginYear(year, null);
        }

        public List<DataBeginYear> GetDataBeginYear(int year, int uid)
        {
            return privateGetDataBeginYear(year, new List<int>() { uid });
        }

        public List<DataBeginYear> getDataBeginyear(int year, List<int> lstUid)
        {
            return privateGetDataBeginYear(year, lstUid);
        }

        public void deleteDataBeginYear(List<int> lstId)
        {
            InitContext(out context);
            var dataDel =  context.DataBeginYears.Where(m => lstId.Contains(m.Id)).ToList();
            context.DataBeginYears.RemoveRange(dataDel);
            DisposeContext(context);
        }

        #region private Method
        private List<DataBeginYear> privateGetDataBeginYear(int year, List<int> lstUid)
        {
            List<DataBeginYear> lstResult = new List<DataBeginYear>();
            InitContext(out context);
            var Query = context.DataBeginYears.Where(m => m.DateBegin.Year == year);
            if (lstUid != null && lstUid.Count > 0)
            {
                Query = Query.Where(m => lstUid.Contains(m.Uid));
            }
            lstResult = Query.ToList();
            DisposeContext(context);
            return lstResult;
        }
        private void privateCalculateDataBeginYear(DateTime MonthYear, List<int> lstUid)
        {
            InitContext(out context);
            var lstTotalLeave = commonLeave.GetTotalLeaveMonthly(context, new DateTime(MonthYear.Year - 1, 12, 1), lstUid);
            var dataBeginDB = context.DataBeginYears.Where(m => lstUid.Contains(m.Uid) && m.DateBegin.Year == MonthYear.Year).ToList();
            foreach (var item in lstTotalLeave)
            {
                var data = dataBeginDB.Where(m => m.Uid == item.Uid).FirstOrDefault();
                if (data != null)
                {
                    data.AnnualLeave = item.LeaveRemain ?? 0;
                }
                else
                {
                    var dataBeginyear = new DataBeginYear();
                    dataBeginyear.Uid = item.Uid;
                    dataBeginyear.AnnualLeave = item.LeaveRemain ?? 0;
                    dataBeginyear.DateBegin = new DateTime(MonthYear.Year, 1, 1);
                    context.DataBeginYears.Add(dataBeginyear);
                }
            }
            DisposeContext(context);
        }

        


        #endregion

    }
}