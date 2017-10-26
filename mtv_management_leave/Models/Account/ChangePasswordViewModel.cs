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
        [Required]
        public int? Id { get; set; }
        [Display(Name = "Current password"), Required]
        public string Password { get; set; }
        [Display(Name = "New password"), Required]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm new password"), Required, Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
