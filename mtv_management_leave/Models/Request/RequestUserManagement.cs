using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Models.Request
{
    public class RequestUserManagement : BootGridRequest
    {
        public IEnumerable<int> Roles { get; set; }
        public string SearchString { get; set; }
    }
}
