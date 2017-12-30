using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    /// <summary>
    /// Summerize tổng leave cho từng tháng
    /// </summary>
    interface IDataBeginYear
    {
        void SaveDataBeginYear(DataBeginYear dataInput);
        void SaveDataBeginYear(DataBeginYear dataInput, List<int> lstUid);
        void CalculateFromLastYear(DateTime MonthYear);
        void CalculateFromLastYear(DateTime MonthYear, List<int> lstUid);
        List<ResponseAvailableBeginYear> GetDataBeginYear(int year);
        List<ResponseAvailableBeginYear> GetDataBeginYear(int year, int uid);
        List<ResponseAvailableBeginYear> GetDataBeginYear(int year, List<int> lstUid);
        void AutoSaveDataBeginYear(string emailUser);
        void AutoSaveDataBeginYear(int uid);
        void deleteDataBeginYear(List<int> lstId);
    }
}
