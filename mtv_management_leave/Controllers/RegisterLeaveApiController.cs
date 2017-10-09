using mtv_management_leave.Lib.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace mtv_management_leave.Controllers
{
    
    public class RegisterLeaveApiController: ApiController
    {
        private InOutBase _inOutBase;

        public RegisterLeaveApiController(InOutBase inOutBase)
        {
            _inOutBase = inOutBase;
        }

        [HttpPost, AllowAnonymous]
        public IHttpActionResult ToList(Models.RegisterLeave.SearchRequest model)
        {
            var result = model.Uid.HasValue ?
                _inOutBase.MappingInoutLeave(model.DateStart, model.DateEnd, model.Uid.Value) :
                _inOutBase.MappingInoutLeave(model.DateStart, model.DateEnd);
            var resultJson = Json( new Lib.Repository.BootGridReponse<Models.MappingInOut>
            {
                current = 1,
                rowCount = 0,
                rows = result
            });
            return resultJson;
        }
    }
}
