using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;

namespace mtv_management_leave.Models
{
    public class RepoRegisterLeave
    {
        public int Uid { get; set; }
        public DateTime DateStart { get; set; }
        public double? RegisterHour { get; set; }
    }
}