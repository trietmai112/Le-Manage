using System;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public string PasswordResetToken { get; set; }
        public int UserCreated { get; set; }
        public int UserUpdated { get; set; }
        public System.DateTime DateCreated { get; set; } = DateTime.Now;
        public System.DateTime DateUpdated { get; set; } = DateTime.Now;


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserInfo, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class Role : IdentityRole<int, UserRole>
    {
        public bool IsShow { get; set; }
        public Role() { }
        public Role(string name) { Name = name; }
    }
    public class UserRole : IdentityUserRole<int> {     
    }
    public class UserClaim : IdentityUserClaim<int> { }
    public class UserLogin : IdentityUserLogin<int> { }
}
