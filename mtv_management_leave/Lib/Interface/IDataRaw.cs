using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    interface IDataRaw
    {
        /// <summary>
        /// ham lay du lieu may cham cong va luu vao trong bang du lieu Raw
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="portNumber"></param>
        void SaveDataRaw(string ipAddress, string portNumber, DateTime dateFrom );
    }
}
