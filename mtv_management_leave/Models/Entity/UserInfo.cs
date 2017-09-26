using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace mtv_management_leave.Models.Entity
{
    public class UserInfo : IdentityUser<int, UserLogin, UserRole, UserClaim>, Interface.IEntity
    {
        public string FullName { get; set; }
        public System.DateTime? DateOfBirth { get; set; }
        public byte UserPermission { get; set; }
        public System.DateTime? DateBeginWork { get; set; }
        public System.DateTime? DateBeginProbation { get; set; }
        public System.DateTime? DateResign { get; set; }
        public string Password { get; set; }
        public int UserCreated { get; set; }
        public int UserUpdated { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }

    }
    public class Role : IdentityRole<int, UserRole>
    {
        public Role() { }
        public Role(string name) { Name = name; }
    }
    public class UserRole : IdentityUserRole<int> { }
    public class UserClaim : IdentityUserClaim<int> { }
    public class UserLogin : IdentityUserLogin<int> { }
}
