using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models;
using mtv_management_leave.Models.InOut;
using System.IO;
using System.IO.Pipes;
using System.Configuration;

namespace mtv_management_leave.Controllers
{
    public class InOutManagementController : Controller
    {
        private InOutBase _inoutBase;
       // private DataRawBase _dataRawBase;

        public InOutManagementController(InOutBase inOutBase)
        {
            _inoutBase = inOutBase;
           // _dataRawBase = new DataRawBase();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetFPdata()
        {
            try
            {
                var pipeClient = new NamedPipeClientStream(".",
                     ConfigurationManager.AppSettings["PipleName"], PipeDirection.InOut, PipeOptions.None);
                
                if (pipeClient.IsConnected != true) { pipeClient.Connect(); }

                StreamReader sr = new StreamReader(pipeClient);
                StreamWriter sw = new StreamWriter(pipeClient);

                string temp;
                temp = sr.ReadLine();

                if (temp == "Waiting")
                {
                    try
                    {
                        sw.WriteLine("getdata");
                        sw.Flush();
                        string severResponse = sr.ReadLine();
                        if(!string.IsNullOrEmpty(severResponse)) { 
                            ///TODO
                        }
                        pipeClient.Close();
                    }
                    catch (Exception ex) { throw ex; }
                }
                return Json(new { Status = 0, Message = "Action complete" });
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Json(new { status = 400, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SumaryInout(SearchRequest model)
        {
            try
            {
                if (model.Uids != null && model.Uids.Count() == 1 && model.Uids[0] == 0)
                {
                    model.Uids = null;
                }
                _inoutBase.SaveGenerateInout(model.DateStart, model.DateEnd, model.Uids);
                return Json(new { Status = 0, Message = "Action complete" });
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
                return Json(new { status = 400, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpPost]
        public JsonResult ToList(SearchRequest model)
        {
            try
            {
                if (model.Uids != null && model.Uids.Count() == 1 && model.Uids[0] == 0)
                {
                    model.Uids = null;
                }

                var result = _inoutBase.MappingInoutLeave(model.DateStart, model.DateEnd, model.Uids);
                if (model.InValidData != null && model.InValidData.Value == true)
                {
                    result = result.Where(m => m.IsValid == false).ToList();
                }
                return Json(new BootGridReponse<RepoMappingInOut>
                {
                    current = 1,
                    total = result.Count,
                    rowCount = -1,
                    rows = result
                });
            }
            catch (Exception e)
            {
                return Json(new { Status = (int)HttpStatusCode.BadRequest, Message = e.Message });
            }
        }
    }
}
