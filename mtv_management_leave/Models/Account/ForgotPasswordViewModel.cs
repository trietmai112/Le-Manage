using mtv_management_leave.Attributes;
using System.ComponentModel.DataAnnotations;

namespace mtv_management_leave.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required, Display(Name = "E-mail"), PlaceHolder("please input your email")]
        public string Email { get; set; }
    }
}
