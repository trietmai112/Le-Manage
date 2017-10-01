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
        public double? LeaveAvailable { get; set; }
        public double? LeaveUsed { get; set; }
        public double? LeaveRemain { get; set; }
        public double? LeaveNonPaid { get; set; }

        [ForeignKey("Uid")]
        public virtual UserInfo UserInfo { get; set; }
    }
}
