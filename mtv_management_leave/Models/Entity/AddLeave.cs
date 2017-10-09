using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mtv_management_leave.Models.Entity
{
    [Table("AddLeave")]
    public class AddLeave: Override.BaseEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Uid { get; set; }
        public double? AddLeaveHour { get; set; }
        public DateTime? DateAdd { get; set; }
        public string Reason { get; set; }
        [ForeignKey("Uid")]
        public virtual UserInfo UserInfo { get; set; }
    }
}
