using mtv_management_leave.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mtv_management_leave.Models.Response
{
    public class ResponseChangeInout
    {
        public int Id { get; set; }
        public int Uid { get; set; }
        public string FullName{ get; set; }
        public string Intime { get; set; }
        public string OutTime { get; set; }
        public string IntimeRequest { get; set; }
        public string OutTimeRequest { get; set; }
        public System.DateTime Date { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
    }
}