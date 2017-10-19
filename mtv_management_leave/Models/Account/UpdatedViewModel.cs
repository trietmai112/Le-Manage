using mtv_management_leave.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Models.Account
{
    public class UpdatedViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        [Required, Display(Name = "Full name"), PlaceHolder("please input your full name")]
        public string FullName { get; set; }

        [Display(Name = "Phone number"), PlaceHolder("please input your phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Birthday"), PlaceHolder("please input your birthday")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Roles")]
        public List<int> RoleIds { get; set; }
        [Display(Name = "Finger-Print number"), PlaceHolder("finger print number")]
        public int? FPId { get; set; }

        [Display(Name = "Date begin work")]
        public System.DateTime? DateBeginWork { get; set; }
        [Display(Name = "Date begin probation")]
        public System.DateTime? DateBeginProbation { get; set; }
        [Display(Name = "Date resign")]
        public System.DateTime? DateResign { get; set; }
    }
}
