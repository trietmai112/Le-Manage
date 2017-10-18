using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Models.Response
{
    public class ResponseUserManagement
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public System.DateTime? DateOfBirth { get; set; }
        public System.DateTime? DateBeginWork { get; set; }
        public System.DateTime? DateBeginProbation { get; set; }
        public System.DateTime? DateResign { get; set; }
        public int? FPId { get; set; }
    }
}
