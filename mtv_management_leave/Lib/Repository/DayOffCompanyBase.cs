
using System;
using System.Collections.Generic;
using System.Linq;
using mtv_management_leave.Lib.Interface;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class DayOffCompanyBase : Base, IDayOffCompany
    {
        LeaveManagementContext context;

        public DayOffCompanyBase()
        {
        }

        public void DeleteLeaveDayCompany(DateTime date)
        {
            InitContext(out context);
            var lstDelete = context.MasterLeaveDayCompanies.Where(m => m.Date == date).ToList();
            context.MasterLeaveDayCompanies.RemoveRange(lstDelete);
            context.SaveChanges();
            DisposeContext(context);
        }

        public void DeleteLeaveDayCompany(int id)
        {
            InitContext(out context);
            var lstDelete = context.MasterLeaveDayCompanies.Where(m => m.Id == id).ToList();
            context.MasterLeaveDayCompanies.RemoveRange(lstDelete);
            context.SaveChanges();
            DisposeContext(context);
        }

        public void DeleteLeaveDayCompany(List<int> ids)
        {
            InitContext(out context);
            var lstDelete = context.MasterLeaveDayCompanies.Where(m => ids.Contains(m.Id)).ToList();
            context.MasterLeaveDayCompanies.RemoveRange(lstDelete);
            context.SaveChanges();
            DisposeContext(context);
        }

        public void DeleteLeaveDayCompany(DateTime dateStart, DateTime dateEnd)
        {
            InitContext(out context);
            var lstDelete = context.MasterLeaveDayCompanies.Where(m => m.Date >= dateStart && m.Date <= dateEnd).ToList();
            context.MasterLeaveDayCompanies.RemoveRange(lstDelete);
            context.SaveChanges();
            DisposeContext(context);
        }

        public List<MasterLeaveDayCompany> GetLeaveDayCompany(int year)
        {
            InitContext(out context);
            DateTime BeginYear = new DateTime(year, 1, 1);
            DateTime EndYear = BeginYear.AddYears(1).AddMinutes(-1);
            var lstResult = context.MasterLeaveDayCompanies.Where(m => m.Date >= BeginYear && m.Date <= EndYear).ToList();
            DisposeContext(context);
            return lstResult;
        }

        public List<MasterLeaveDayCompany> GetLeaveDayCompany(DateTime dateStart, DateTime DateEnd)
        {
            if (dateStart > DateEnd)
            {
                throw new Exception("Please select Date Start less or equal with Date End!");
            }
            InitContext(out context);
            var lstResult = context.MasterLeaveDayCompanies.Where(m => m.Date >= dateStart && m.Date <= DateEnd).ToList();
            DisposeContext(context);
            return lstResult;
        }

        public void SaveLeaveDayCompany(MasterLeaveDayCompany InputLeaveDayCompany)
        {
            InitContext(out context);
            DateTime date = InputLeaveDayCompany.Date;
            var lstResult = context.MasterLeaveDayCompanies.Where(m => m.Date == date).FirstOrDefault();
            if (lstResult == null)
            {
                context.MasterLeaveDayCompanies.Add(InputLeaveDayCompany);
            }
            else
            {
                throw new Exception("Data Duplicate!");
            }
            context.SaveChanges();
            DisposeContext(context);
        }
    }
}