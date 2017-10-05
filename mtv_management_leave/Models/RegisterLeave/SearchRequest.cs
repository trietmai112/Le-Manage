using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace mtv_management_leave.Models.RegisterLeave
{
    public class SearchRequest
    {
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int? Uid { get; set; }
    }
}
