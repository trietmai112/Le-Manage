using mtv_management_leave.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    public interface ICalendarEventBase
    {
        IEnumerable<CalendarEventViewReponse> GetEvents(DateTime start, DateTime end);
    }
}
