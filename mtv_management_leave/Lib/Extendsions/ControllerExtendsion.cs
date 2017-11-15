using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


public class ControllerExtendsion: Controller
{
    public JsonResult BadRequest(Exception ex)
    {
        Response.StatusCode = 400;
        Response.TrySkipIisCustomErrors = true;
        Response.Headers.Add("Content-type", "application/json");
        return Json( new { status = 400, message = ex.Message }, JsonRequestBehavior.AllowGet );
    }
}

