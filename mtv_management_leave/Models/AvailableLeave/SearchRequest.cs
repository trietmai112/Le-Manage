using System.Collections.Generic;

namespace mtv_management_leave.Models.AvailableLeave
{
    public class SearchRequest
    {
        public int? Year { get; set; }
        public List<int> Uids { get; set; }
    }
}
