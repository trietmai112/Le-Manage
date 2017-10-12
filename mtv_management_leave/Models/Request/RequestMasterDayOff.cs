using mtv_management_leave.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mtv_management_leave.Models.Request
{
    public class RequestMasterDayOff
    {
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}