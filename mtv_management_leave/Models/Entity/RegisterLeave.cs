using mtv_management_leave.Lib;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mtv_management_leave.Models.Entity
{
    [Table("RegisterLeave")]
    public class RegisterLeave: Override.BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Uid { get; set; }
        public int LeaveTypeId { get; set; }
        public System.DateTime DateStart { get; set; }
        public System.DateTime DateEnd { get; set; }
        public double? RegisterHour { get; set; }
        public string Reason { get; set; }
        public System.DateTime DateRegister { get; set; }
        public Common.StatusLeave Status { get; set; }
        public int? UserApprove { get; set; }
        public DateTime? DateApprove { get; set; }

        [ForeignKey("LeaveTypeId")]
        public virtual MasterLeaveType MasterLeaveType { get; set; }
        [ForeignKey("Uid")]
        public virtual UserInfo UserInfo { get; set; }
    }
}
