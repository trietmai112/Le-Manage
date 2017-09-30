using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mtv_management_leave.Models.Entity
{
    [Table("DataBeginYear")]

    public partial class DataBeginYear : Override.BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Uid { get; set; }
        public double AnnualLeave { get; set; }
        public System.DateTime DateBegin { get; set; }
        [ForeignKey("Uid")]
        public virtual UserInfo UserInfo { get; set; }
    }
}
