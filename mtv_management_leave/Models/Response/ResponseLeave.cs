using mtv_management_leave.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mtv_management_leave.Models.Response
{
    public class ResponseLeave
    {
        public int Id { get; set; }
        public int Uid { get; set; }
        public string FullName { get; set; }
        public string LeaveFrom { get; set; }
        public string LeaveTo { get; set; }
        public string LeaveTypeName { get; set; }
        public double? RegisterHour { get; set; }
        public Common.StatusLeave LeaveStatus { get; set; }
    }
}