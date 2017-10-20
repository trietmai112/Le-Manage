using mtv_management_leave.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Models.Request
{
    public class RequestUserManagement : BootGridRequest
    {
        [Display(Name = "Roles")]
        public IEnumerable<int> Roles { get; set; }
        [Display(Name = "Search"), PlaceHolder("serch by name/phone/email")]
        public string SearchString { get; set; }
    }
}
