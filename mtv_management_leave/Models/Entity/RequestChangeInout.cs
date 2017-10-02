using mtv_management_leave.Lib;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mtv_management_leave.Models.Entity
{
    [Table("RequestChangeInout")]
    public class RequestChangeInout : Override.BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Uid { get; set; }
        public System.DateTime? Intime { get; set; }
        public System.DateTime? OutTime { get; set; }
        public System.DateTime Date { get; set; }
        public Common.StatusLeave  status { get; set; }
        public string Reason { get; set; }

        [ForeignKey("Uid")]
        public virtual UserInfo UserInfo { get; set; }
    }
}
