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
        public byte? Month1 { get; set; }
        public byte? Month2 { get; set; }
        public byte? Month3 { get; set; }
        public byte? Month4 { get; set; }
        public byte? Month5 { get; set; }
        public byte? Month6 { get; set; }
        public byte? Month7 { get; set; }
        public byte? Month8 { get; set; }
        public byte? Month9 { get; set; }
        public byte? Month10 { get; set; }
        public byte? Month11 { get; set; }
        public byte? Month12 { get; set; }
        [ForeignKey("Uid")]
        public virtual UserInfo UserInfo { get; set; }
    }
}
