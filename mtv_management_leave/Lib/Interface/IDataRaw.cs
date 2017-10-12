using System;

namespace mtv_management_leave.Lib.Interface
{
    interface IDataRaw
    {
        /// <summary>
        /// ham lay du lieu may cham cong va luu vao trong bang du lieu Raw
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="portNumber"></param>
        void SaveDataRaw();
        DateTime? getLastData();
    }
}
