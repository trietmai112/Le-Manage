using mtv_management_leave.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace mtv_management_leave.Models.Account
{
    public class RegisterViewModel
    {
        [Required, EmailAddress, Display(Name = "E-mail"), PlaceHolder("please input your email")]
        public string Email { get; set; }

        [Required, 
            StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6),
            DataType(DataType.Password), 
            Display(Name = "Password"),
            PlaceHolder("please input your password")]
        public string Password { get; set; }

        [DataType(DataType.Password), 
            Display(Name = "Confirm password"), 
            Compare("Password", ErrorMessage = "The password and confirmation password do not match."),
            PlaceHolder("please input confirm password")]
        public string ConfirmPassword { get; set; }

        [Required, PlaceHolder("please input your full name")]
        public string FullName { get; set; }

        [PlaceHolder("please input your phone number")]
        public string PhoneNumber { get; set; }

        [PlaceHolder("please input your birthday")]
        public DateTime DateOfBirth { get; set; }
    }
}
