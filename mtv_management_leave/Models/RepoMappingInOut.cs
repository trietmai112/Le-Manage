﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;

namespace mtv_management_leave.Models
{
    public class RepoMappingInOut
    {
        public string GuiId { get; set; } = Guid.NewGuid().ToString("N");
        public int Uid { get; set; }
        public string FullName { get; set; }
        public bool IsValid { get; set; }
        /// <summary>
        /// thời gian lệch so với leave
        /// </summary>
        public double TimeDiff { get; set; }
        public DateTime Date { get; set; }
        public string Intime { get; set; }
        public DateTime? IntimeByDateTime { get; set; }
        public string Outtime { get; set; }
        public DateTime? OuttimeByDateTime { get; set; }
        public string LeaveStart1 { get; set; }
        public string LeaveEnd1 { get; set; }
        public string LeaveStatus1 { get; set; }
        public string LeaveType1 { get; set; }
        public string LeaveStart2 { get; set; }
        public string LeaveEnd2 { get; set; }
        public string LeaveStatus2 { get; set; }
        public string LeaveType2 { get; set; }

        public double TimeShift { get; set; }
        public double TimeWork { get; set; }
        public double TimeLeave { get; set; }

    }
}