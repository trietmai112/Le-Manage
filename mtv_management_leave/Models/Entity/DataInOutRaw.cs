using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mtv_management_leave.Models.Entity
{
    [Table("DataInOutRaw")]
    public class DataInOutRaw: Override.BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Uid { get; set; }
        public System.DateTime Time { get; set; }
    }
}
