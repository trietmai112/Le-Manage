using mtv_management_leave.Lib.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace mtv_management_leave.Controllers
{
    
    public class ValueController: ApiController
    {
        [HttpGet, Route("api/value/value")]
        public IHttpActionResult Value()
        { 
            return Json( new Lib.Repository.BootGridReponse<object>
            {
                current = 1,
                rowCount = 0,
                rows = new List<object>()
            });
        }
    }
}
