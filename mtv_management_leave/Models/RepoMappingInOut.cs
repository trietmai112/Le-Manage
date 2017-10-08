using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;

namespace mtv_management_leave.Models
{
    public class RepoMappingInOut
    {
        public int Uid { get; set; }
        public string FullName { get; set; }
        public bool IsValid { get; set; }
        /// <summary>
        /// thời gian lệch so với leave
        /// </summary>
        public double TimeDiff { get; set; }
        public DateTime Date { get; set; }
        public string Intime { get; set; }
        public string Outtime { get; set; }
        public string LeaveStart1 { get; set; }
        public string LeaveEnd1 { get; set; }
        public string LeaveStatus1 { get; set; }
        public string LeaveType1 { get; set; }
        public string LeaveStart2 { get; set; }
        public string LeaveEnd2 { get; set; }
        public string LeaveStatus2 { get; set; }
        public string LeaveType2 { get; set; }

    }
}