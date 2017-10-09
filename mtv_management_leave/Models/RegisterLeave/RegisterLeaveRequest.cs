using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Models.RegisterLeave
{
    public class RegisterLeaveRequest
    {
        public int Uid { get; set; }
        public bool IsToday { get; set; } = true;
        public System.DateTime DateStart { get; set; } = DateTime.Now;
        public System.DateTime DateEnd { get; set; } = DateTime.Now;
        public double? RegisterHour { get; set; }
        public string Reason { get; set; }
        [Display(Name = "Leave type")]
        public int LeaveTypeId { get; set; }
        public string Error { get; set; }
    }
}
