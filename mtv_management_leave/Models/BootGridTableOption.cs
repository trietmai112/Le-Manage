using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Models
{
    public class BootGridTableOption
    {
        /// <summary>
        /// Id
        /// </summary>
        public string GuiId { get; set; }

        /// <summary>
        /// class
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// set url will call when get data
        /// </summary>
        public string AjaxUrl { get; set; }

        public AjaxSetting AjaxSettings { get; set; } = new AjaxSetting();

        /// <summary>
        /// id/class of controls will put more value when call ajax
        /// ex: Dictionanry: 1. Variable name     2. Id/Class of control
        /// </summary>
        public Dictionary<string, string> AjaxParameters { get; set; }
    }

    public class BootGridColumnOption
    {
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
        public bool Cache { get; set; } = false;
    }
}
