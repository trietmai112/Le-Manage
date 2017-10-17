using System;
using System.ComponentModel.DataAnnotations;

namespace mtv_management_leave.Models.RegisterLeave
{
    public class RegisterLeaveRequest
    {
        public int Uid { get; set; }
        public bool IsToday { get; set; } = true;
        public System.DateTime DateStart { get; set; } = DateTime.Today.AddHours(8);
        public System.DateTime DateEnd { get; set; } = DateTime.Today.AddHours(17);
        public double? RegisterHour { get; set; }
        public string Reason { get; set; }
        [Display(Name = "Leave type")]
        public int LeaveTypeId { get; set; }
        public string Error { get; set; }
    }
}
