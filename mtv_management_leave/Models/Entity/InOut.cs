using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mtv_management_leave.Models.Entity
{
    [Table("InOut")]
    public class InOut: Override.BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Uid { get; set; }
        public System.DateTime Intime { get; set; }
        public System.DateTime? OutTime { get; set; }
        public System.DateTime Date { get; set; }
        [ForeignKey("Uid")]
        public virtual UserInfo UserInfo { get; set; }
    }
}
