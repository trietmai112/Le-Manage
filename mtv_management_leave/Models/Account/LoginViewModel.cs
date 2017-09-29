using mtv_management_leave.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mtv_management_leave.Models.Account
{
    public class LoginViewModel
    {
        [PlaceHolder("please input your email"), DisplayName("E-mail"), Required]
        public string Email { get; set; }
        [PlaceHolder("please input your password"), DisplayName("Password"), Required]
        public string Password { get; set; }
        [DisplayName("Remember me!")]
        public bool RememberMe { get; set; }
    }
}
