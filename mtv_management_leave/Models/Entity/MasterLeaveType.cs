using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mtv_management_leave.Models.Entity
{
    [Table("MasterLeaveType")]
    public class MasterLeaveType: Override.BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsPaidLeave { get; set; }
        public bool? IsBussinessLeave { get; set; }
        public string LeaveCode { get; set; }
        public string Description { get; set; }

        public virtual ICollection<AddLeave> AddLeaves { get; set; }
        public virtual ICollection<RegisterLeave> RegisterLeaves { get; set; }
    }
}
