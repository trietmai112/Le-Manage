using System;
using System.Collections.Generic;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

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
        void SaveGenerateInout(int uid, DateTime dateFrom, DateTime? DateTo);

        List<RepoMappingInOut> MappingInoutLeave(DateTime DateStart, DateTime DateEnd);
        List<RepoMappingInOut> MappingInoutLeave(DateTime DateStart, DateTime DateEnd, List<int> lstUid);
        List<RepoMappingInOut> MappingInoutInValid(DateTime DateStart, DateTime DateEnd);
        List<RepoMappingInOut> MappingInoutInvalid(DateTime DateStart, DateTime DateEnd, List<int> lstUid);

        void UpdateOrCreateInout(InOut obj);
        void DeleteInOut(List<int> lstId);
        void DeleteInOut(DateTime dateStart, DateTime dateEnd, List<int> lstUid);


    }
}
