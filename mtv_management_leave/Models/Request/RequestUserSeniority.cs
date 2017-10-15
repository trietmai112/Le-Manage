using mtv_management_leave.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mtv_management_leave.Models.Request
{
    public class RequestUserSeniority
    {
        public List<int> Uids{ get; set; }
        public DateTime DateYear { get; set; }
    }
}