using mtv_management_leave.Lib.Interface;
using mtv_management_leave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mtv_management_leave.Models.Response;
using WebGrease.Css.Extensions;

namespace mtv_management_leave.Lib.Repository
{
    public class CalendarEventBase: ICalendarEventBase
    {
        private LeaveManagementContext _context;

        public CalendarEventBase(LeaveManagementContext context)
        {
            _context = context;
        }


        /// <summary>
        /// birthday: bgm-blue -> leave: bgm-orange
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IEnumerable<CalendarEventViewReponse> GetEvents(DateTime start, DateTime end)
        {
            int _id = 1;
            var employeeBirthDays = _context.Users
                .ToList()
                .Where(m=> m.DateOfBirth.HasValue
                        && m.DateOfBirth.Value.ToString("MMdd").CompareTo(start.ToString("MMdd")) >= 0
                        && m.DateOfBirth.Value.ToString("MMdd").CompareTo(end.ToString("MMdd")) <= 0)
                .Select(m => new CalendarEventViewReponse
                {
                   start = new DateTime(DateTime.Now.Year, m.DateOfBirth.Value.Month, m.DateOfBirth.Value.Day, 20,0,0),
                   description = "Birthday of " + m.FullName,
                   className = "bgm-blue",
                   title = "Birthday of " + m.FullName  
                }).ToList();                

            var leaveDays = _context.MasterLeaveDayCompanies
                .Where(m => m.Date >= start && m.Date <= end)
                .Select(m => new CalendarEventViewReponse
                {
                    start = m.Date,
                    description = m.Description,
                    title = m.Description
                })
                .ToList();

            var results = employeeBirthDays.Union(leaveDays).ToList();
           
            return results;
        }
    }
}
