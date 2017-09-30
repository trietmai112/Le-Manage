using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;

namespace mtv_management_leave.Models
{
    public class MappingInOut
    {
        public int Uid { get; set; }
        public string FullName { get; set; }
        public bool IsValid { get; set; }
        /// <summary>
        /// thời gian lệch so với leave
        /// </summary>
        public double TimeDiff{ get; set; }
        public DateTime Date { get; set; }
        public string Intime { get; set; }
        public string Outtime { get; set; }
        public string LeaveStart { get; set; }
        public string LeaveEnd { get; set; }
        public string LeaveType { get; set; }

    }
}