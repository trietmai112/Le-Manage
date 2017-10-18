using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Models.Request
{
    public class BootGridRequest
    {
        public int current { get; set; } = 1;
        public int rowCount { get; set; } = 20;
        public Dictionary<string,string> sort { get; set; }
    }
}
