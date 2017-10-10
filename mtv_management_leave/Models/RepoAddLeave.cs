using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;

namespace mtv_management_leave.Models
{
    public class RepoAddLeave
    {
        public int Uid { get; set; }
        public DateTime? DateAdd { get; set; }
        public double? AddLeaveHour { get; set; }
    }
}