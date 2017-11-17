using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Models.Response
{
    public class CalendarEventViewReponse
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public bool allDay { get; set; } = true;
        public string className { get; set; } = "bgm-orange";
        public string description { get; set; }
    }
}
