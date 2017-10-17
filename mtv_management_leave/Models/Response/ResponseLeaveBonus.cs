using mtv_management_leave.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mtv_management_leave.Models.Response
{
    public class ResponseLeaveBonus
    {
        public int Id { get; set; }
        public int Uid { get; set; }
        public string FullName { get; set; }
        public double? AddLeaveHour { get; set; }
        public DateTime? DateAdd { get; set; }
        public string Reason { get; set; }
    }
}