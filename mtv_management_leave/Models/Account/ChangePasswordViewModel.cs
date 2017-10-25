using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Models.Account
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "New password"), Required]
        public string Password { get; set; }
        [Display(Name = "Confirm new password"), Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
