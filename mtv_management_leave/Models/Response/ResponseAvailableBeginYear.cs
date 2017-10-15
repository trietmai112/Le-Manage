using mtv_management_leave.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mtv_management_leave.Models.Response
{
    public class ResponseAvailableBeginYear
    {
        public int Id { get; set; }
        public int Uid { get; set; }
        public string FullName{ get; set; }
        public double AnnualLeave { get; set; }
        public System.DateTime DateBegin { get; set; }
    }
}