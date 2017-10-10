using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    interface IDayOffCompany
    {
        List<MasterLeaveDayCompany> GetLeaveDayCompany(int year);
        List<MasterLeaveDayCompany> GetLeaveDayCompany(DateTime dateStart, DateTime DateEnd);
        void SaveLeaveDayCompany(MasterLeaveDayCompany InputLeaveDayCompany);
        void DeleteLeaveDayCompany(int id);
        void DeleteLeaveDayCompany(DateTime date);
        void DeleteLeaveDayCompany(DateTime dateStart,DateTime dateEnd);

    }
}
