using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace mtv_management_leave.Models.AvailableLeave
{
    public class SearchRequest
    {
        public DateTime Year { get; set; } = new DateTime(DateTime.Today.Year, 1, 1);
        public List<int> Uids { get; set; }
    }
}
