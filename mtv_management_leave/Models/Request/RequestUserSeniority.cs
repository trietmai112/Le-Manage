using System;
using System.Collections.Generic;

namespace mtv_management_leave.Models.Request
{
    public class RequestUserSeniority
    {
        public List<int> Uids { get; set; }
        public DateTime DateYear { get; set; }
        public int Year { get; set; }
    }
}