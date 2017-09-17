using mtv_management_leave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    interface IInOut
    {
        /// <summary>
        /// ham tinh toan du lieu in - out
        /// tinh theo tung ngay voi gia tri Min -> In , Max -> Out trong 1 ngay
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="DateTo"></param>
        void SaveGenerateInout(DateTime dateFrom, DateTime? DateTo);

        /// <summary>
        /// ham tinh toan du lieu in - out
        /// tinh theo tung ngay voi gia tri Min -> In , Max -> Out trong 1 ngay
        /// </summary>
        /// <param name="Uid"></param>
        /// <param name="dateFrom"></param>
        /// <param name="DateTo"></param>
        void SaveGenerateInout(int Uid, DateTime dateFrom, DateTime? DateTo);
    }
}
