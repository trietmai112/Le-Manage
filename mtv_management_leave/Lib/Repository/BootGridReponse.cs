using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Repository
{
    public class BootGridReponse<T>
    {
        public int current { get; set; }
        public int rowCount { get; set; }
        public List<T> rows { get; set; }
        public int total { get; set; }
    }
}
