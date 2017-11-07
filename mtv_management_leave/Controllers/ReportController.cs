using System.Linq;
using System.Web.Mvc;

namespace mtv_management_leave.Controllers
{
    public class ReportController : Controller
    {

        [HttpGet]
        public virtual ActionResult Download(string file)
        {

            var filename = file.Split(new char[] { '\\' }, System.StringSplitOptions.RemoveEmptyEntries).Last();

            //string fullPath = file.Replace(filename, string.Empty);
            //filename = filename.Replace(".xlsx", string.Empty);
            return File(file, "application/vnd.ms-excel", filename);
        }
    }
}
