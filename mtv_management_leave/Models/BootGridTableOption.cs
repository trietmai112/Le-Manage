using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Models
{   
    public class BootGridColumnOption
    {
        public bool IsHidden { get; set; }
        public string FormatJqueryHandler { get; set; }
        public string ColumnHeaderName { get; set; }
        public string MappingFrom { get; set; }
        public bool Identify { get; set; }
        public string OrderBy { get; set; }
        public bool Sortable { get; set; }
    }

    public class AjaxSetting
    {
        public string RequestType { get; set; } = "POST";
        public bool Cache { get; set; } = true;
    }
}
