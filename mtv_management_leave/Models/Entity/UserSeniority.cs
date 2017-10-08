using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mtv_management_leave.Models.Entity
{
    [Table("UserSeniority")]
    public class UserSeniority: Override.BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Uid { get; set; }
        public int? Year { get; set; }
        public int? Month1 { get; set; }
        public int? Month2 { get; set; }
        public int? Month3 { get; set; }
        public int? Month4 { get; set; }
        public int? Month5 { get; set; }
        public int? Month6 { get; set; }
        public int? Month7 { get; set; }
        public int? Month8 { get; set; }
        public int? Month9 { get; set; }
        public int? Month10 { get; set; }
        public int? Month11 { get; set; }
        public int? Month12 { get; set; }
        [ForeignKey("Uid")]
        public virtual UserInfo UserInfo { get; set; }
    }
}
