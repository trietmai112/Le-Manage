using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mtv_management_leave.Models.Entity
{
    [Table("LeaveMonthly")]
    public class LeaveMonthly: Override.BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Uid { get; set; }
        public System.DateTime Month { get; set; }
        public bool? LeaveAvailable { get; set; }
        public bool? LeaveUsed { get; set; }
        public bool? LeaveRemain { get; set; }
        public bool? LeaveNonPaid { get; set; }
        public bool? IsMaterityLeave { get; set; }

        [ForeignKey("Uid")]
        public virtual UserInfo UserInfo { get; set; }
    }
}
