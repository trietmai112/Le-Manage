using System;
using System.Collections.Generic;
using System.Linq;
using mtv_management_leave.Lib.Interface;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class DataRawBase : Base, IDataRaw
    {

        LeaveManagementContext context;
        private zkemkeeper.CZKEMClass driver = new zkemkeeper.CZKEMClass();

        public DateTime? getLastData()
        {
            InitContext(out context);
            DateTime? lastTime = null;
            var lastDataRaw = context.DataInOutRaws.OrderByDescending(t => t.Time).FirstOrDefault();
            if (lastDataRaw != null)
            {
                lastTime = lastDataRaw.Time;
            }
            DisposeContext(context);
            return lastTime;
        }

        public void SaveDataRaw()
        {
            DateTime? lastTime = getLastData();
            InitContext(out context);
            int dwMachineNumber = 1;
            string dwEnrollNumber;
            string name;
            string password;
            int privilege;
            bool enabled;
            int dwVerifyMode;
            int dwInOutMode;
            int year = 2017;
            int month = 9;
            int day = 27;
            int h;
            int m;
            int s;
            int dwWorkCode = 0;

            // {
            //fp.Disconnect();
            Connection();

            var lstEmpInfor = context.EmployeeInfos.Select(t => new { t.Id, t.FPId }).ToList();
            Console.WriteLine("====== History tracker =============================================");
            List<DataInOutRaw> lstdataRaw = new List<DataInOutRaw>();
            try
            {
                while (driver.SSR_GetGeneralLogData(dwMachineNumber, out dwEnrollNumber, out dwVerifyMode, out dwInOutMode, out year, out month, out day, out h, out m, out s, ref dwWorkCode))
                {
                    DateTime timePrint = new DateTime(year, month, day, h, m, s);
                    if (lastTime != null && timePrint <= lastTime)
                    {
                        continue;
                    }
                    else
                    {
                        var fpID = int.Parse(dwEnrollNumber);
                        var uid = lstEmpInfor.Where(t => t.FPId == fpID).Select(t => t.Id).FirstOrDefault();
                        DataInOutRaw dataR = new DataInOutRaw();
                        dataR.Uid = uid;
                        dataR.Time = timePrint;
                        lstdataRaw.Add(dataR);
                    }
                }
            }
            catch (Exception e)
            {

            }
            Console.WriteLine("====== End History tracker =============================================");
            Disconnect();
            if (lstdataRaw.Count > 0)
            {
                context.DataInOutRaws.AddRange(lstdataRaw);
            }
            context.SaveChanges();
            DisposeContext(context);

        }

        #region private code
        private void Connection()
        {
            var isConnect = false;
            int port = 4370;
            do
            {
                isConnect = driver.Connect_Net("10.10.10.20", port);
                if (isConnect) Console.WriteLine("Open connect complete!");
                else
                {
                    Console.WriteLine("Can not connect to device, try...");
                }
            } while (!isConnect);
        }
        private void Disconnect()
        {
            driver.Disconnect();
            Console.WriteLine("Disconnection...!");
        }

        #endregion
    }
}