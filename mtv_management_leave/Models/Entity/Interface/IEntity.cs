namespace mtv_management_leave.Models.Entity.Interface
{
    public interface IEntity
    {
        int UserCreated { get; set; }
        int UserUpdated { get; set; }
        System.DateTime DateCreated { get; set; }
        System.DateTime DateUpdated { get; set; }
    }
}
