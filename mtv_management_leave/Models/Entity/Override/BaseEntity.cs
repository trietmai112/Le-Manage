using System;

namespace mtv_management_leave.Models.Entity.Override
{
    public class BaseEntity : Interface.IEntity
    {
        public int UserCreated { get; set; }
        public int UserUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
