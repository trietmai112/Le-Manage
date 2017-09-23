using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;

namespace mtv_management_leave.Models
{
    public class MappingInOutByMonth
    {
        public int Uid { get; set; }
        public string FullName { get; set; }
        public bool IsValid { get; set; }
        /// <summary>
        /// thời gian lệch so với leave
        /// </summary>
        public double TimeDiff{ get; set; }
        public DateTime MonthYear { get; set; }
    }

    /// <summary>
    /// Mapping thời gian inout và leave theo user của tháng tính theo từng leave
    /// </summary>
    public class MappingInOutByUser
    {
        public int Uid { get; set; }
        public string FullName { get; set; }
        public bool IsValid { get; set; }
        /// <summary>
        /// thời gian lệch so với leave
        /// </summary>
        public double TimeDiff { get; set; }
        public DateTime Date { get; set; }

        public DateTime BeginLeave { get; set; }
        public DateTime EndLeave { get; set; }
        public string TypeLeave { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
    }
}