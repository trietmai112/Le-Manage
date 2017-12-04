using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MTV_WindowsService.DbContext;

namespace MTV_WindowsService.CallerMethods
{
    public class FingerPrintDevice
    {
        private static int dwMachineNumber = 1;
        private static string dwEnrollNumber;
        private static string name;
        private static string password;
        private static int privilege;
        private static bool enabled;
        private static int dwVerifyMode;
        private static int dwInOutMode;
        private static int year = 2017;
        private static int month = 9;
        private static int day = 27;
        private static int h;
        private static int m;
        private static int s;
        private static int dwWorkCode;

        zkemkeeper.CZKEMClass _device = new zkemkeeper.CZKEMClass();
        NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        DataProvider.Gateway context = null;

        public FingerPrintDevice()
        {
            context = new DataProvider.Gateway(ConfigurationManager.ConnectionStrings["LeaveManagementEntities"].ConnectionString);
        }

        private bool ConnectToDevice()
        {
            var ip = ConfigurationManager.AppSettings["FingerPrintIP"];
            var port = 0;
            if (!int.TryParse(ConfigurationManager.AppSettings["FingerPrintPort"], out port))
            {
                port = 4370;
                _logger.Error($"Can not parse {ConfigurationManager.AppSettings["FingerPrintPort"]} to System.Int");
            }
            if (!_device.Connect_Net(ip, port))
            {
                _logger.Error($"Can not connect to finger print with address {ip}:{port}");
            }
            return true;
        }

        private void DisConnectWithDevice()
        {
            _device.Disconnect();
        }

        private DateTime? LastTime()
        {
            DateTime? lastTime = context.Single<DateTime?>("select max(time) from DataInOutRaw");
            return lastTime;
        }

        public void GetTimeTrackerFromDevice()
        {
            var lastTime = LastTime();
            this.ConnectToDevice();
            var lstEmpInfor = context.ToList<EmployeeInfo>("select Id, FPId, DateUpdated from EmployeeInfo");
            List<DataInOutRaw> lstdataRaw = new List<DataInOutRaw>();
            try
            {
                while (_device.SSR_GetGeneralLogData(dwMachineNumber, out dwEnrollNumber, out dwVerifyMode, out dwInOutMode, out year, out month, out day, out h, out m, out s, ref dwWorkCode))
                {
                    DateTime timePrint = new DateTime(year, month, day, h, m, s);
                    if (lastTime != null && timePrint <= lastTime)
                    {
                        continue;
                    }
                    else
                    {
                        var fpID = int.Parse(dwEnrollNumber);
                        var uid = lstEmpInfor.Where(t => t.FPId == fpID).OrderByDescending(m => m.DateUpdated).Select(t => t.Id).FirstOrDefault();
                        DataInOutRaw dataR = new DataInOutRaw();
                        dataR.Uid = uid;
                        dataR.Time = timePrint;
                        dataR.DateCreated = DateTime.Now;
                        dataR.DateUpdated = DateTime.Now;
                        dataR.UserCreated = int.MaxValue;
                        dataR.UserUpdated = int.MaxValue;
                        lstdataRaw.Add(dataR);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error("Occur error when read data from device");
                _logger.Error($"-> Error message: {e.Message}");
                _logger.Error($"-> Error stack trace: {e.StackTrace}");
            }
            DisConnectWithDevice();
            if (lstdataRaw.Count > 0)
            {
                var now = DateTime.Now;
                string sql = "";
                var index = 0;
                var result = 0;
                foreach (var item in lstdataRaw)
                {
                    sql += $@"insert into DataInOutRaw(Time, UserCreated, UserUpdated, DateCreated, DateUpdated, Uid)
                            values('{item.Time}', '{int.MaxValue}', '{int.MaxValue}', '{item.DateCreated}','{item.DateUpdated}', '{item.Uid}');";
                    index += 1;
                    if (index == 100)
                    {

                        result = context.ExecuteNonQuery(sql);
                        _logger.Error("Result: " + result);
                        _logger.Error("context.Message: " + context.Message);
                        sql = "";
                        index = 0;
                    }
                }
                if (!string.IsNullOrEmpty(sql))
                    result = context.ExecuteNonQuery(sql);
                _logger.Error("Last Result: " + result);
                _logger.Error("Last context.Message: " + context.Message);
            }


        }
    }
}
