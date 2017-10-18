using System;

namespace mtv_management_leave.Models.Response
{
    public class ResponseMasterDayOff
    {
        public int Id { get; set; }
        public DateTime DateLeave { get; set; }
        public string Reason { get; set; }
    }
}