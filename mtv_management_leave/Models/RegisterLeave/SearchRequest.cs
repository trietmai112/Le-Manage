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
        [Display(Name = "Date start:")]
        public DateTime DateStart { get; set; } = DateTime.Now;
        [Display(Name = "Date end:")]
        public DateTime DateEnd { get; set; } = DateTime.Now;
        public string Uids { get; set; }
        public int? Uid { get; set; }
    }
}
