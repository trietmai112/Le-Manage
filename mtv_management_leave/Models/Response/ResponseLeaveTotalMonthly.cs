using mtv_management_leave.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mtv_management_leave.Models.Response
{
    public class ResponseLeaveTotalMonthly
    {
        public int Id { get; set; }
        public int Uid { get; set; }
        public DateTime Month { get; set; }
        public double? LeaveAvailable { get; set; }
        public double? LeaveUsed { get; set; }
        public double? LeaveRemain { get; set; }
        public double? LeaveNonPaid { get; set; }
        public string FullName { get; set; }
    }
}