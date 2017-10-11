using mtv_management_leave.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mtv_management_leave.Models.Response
{
    public class ResponseMasterDayOff
    {
        public DateTime DateLeave{ get; set; }
        public string Reason{ get; set; }
    }
}