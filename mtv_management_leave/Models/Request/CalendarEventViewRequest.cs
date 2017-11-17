using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Models.Request
{
    public class CalendarEventViewRequest
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
