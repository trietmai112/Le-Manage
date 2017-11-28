using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTV_WindowsService.DbContext
{
    public class DataInOutRaw
    {
        public int Id { get; set; }
        public int Uid { get; set; }
        public System.DateTime Time { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UserCreated { get; set; }
        public int UserUpdated { get; set; }
    }
}
