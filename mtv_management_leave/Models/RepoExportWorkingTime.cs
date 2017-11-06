using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;

namespace mtv_management_leave.Models
{
    public class RepoExportWorkingTime
    {
        public int Uid { get; set; }
        public string FullName { get; set; }
        /// <summary>
        /// thời gian lệch so với leave
        /// </summary>
        public double TotalTimeShift { get; set; }
        public double TotalTimeWork { get; set; }
        public double TotalTimeLateEarly { get; set; }
        public double TotalTimeLeave { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}