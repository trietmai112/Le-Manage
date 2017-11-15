using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using mtv_management_leave.Lib;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models;
using mtv_management_leave.Models.InOut;

namespace mtv_management_leave.Controllers
{
    [Authorize(Roles = "Super admin, Admin")]
    public class InOutManagementController : ControllerExtendsion
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
                        if (!string.IsNullOrEmpty(severResponse))
                        {
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
                return BadRequest(e); 
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
                return BadRequest(e);
            }
        }

        [HttpPost]
        public JsonResult AutoCreateLeave(SearchRequest model)
        {
            try
            {
                if (model.Uids != null && model.Uids.Count() == 1 && model.Uids[0] == 0)
                {
                    model.Uids = null;
                }
                var result = _inoutBase.MappingInoutInvalid(model.DateStart, model.DateEnd, model.Uids);
                LeaveBase leaveBase = new LeaveBase();
                leaveBase.AutoCreateLeave(result);
                return Json(new { Status = 0, Message = "Action complete" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public ActionResult ReportLateEarly(SearchRequest model)
        {


            try
            {
                if (model.DateStart == DateTime.MinValue || model.DateEnd == DateTime.MinValue)
                {
                    throw new Exception("Please input Start and End!");
                }
                if(model.DateStart.Year != model.DateEnd.Year || model.DateStart.Month!= model.DateEnd.Month)
                {
                    throw new Exception("Please export in month!");
                }

                if(model.Uids!= null && model.Uids.Count== 1 && model.Uids[0] == 0)
                {
                    model.Uids = null;
                }

                var path = Excel(model.DateStart, model.DateEnd, model.Uids);
                return Content(path);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        private string Excel(DateTime dateStart, DateTime dateEnd, List<int> lstUid)
        {
            string path = Path.Combine(Server.MapPath("~/ReportFiles/"), $@"{DateTime.Now.Ticks}.xlsx");
            var result = _inoutBase.ExportWorkingTime(dateStart, dateEnd, lstUid);
            ExcelCommon common = new ExcelCommon();
            common.CreateNewShet("WorkingTime");
            common.AddHeader<RepoExportWorkingTime>();

            common.AddRecords(result);
            common.Save(path);
            //var memory = new MemoryStream(System.IO.File.ReadAllBytes(path));
            //return File(memory, "Application/x-msexcel", DateTime.Now.Ticks + ".xlsx");

            return path;

            //var historyList = historyLogic.CsvOutput(param);
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("shift-jis");
            //Response.Clear();
            //Response.Write("記事ID,タイトル,日付,IOS,Android,PC,妊娠したくない,妊娠したい,月経期,卵胞期,黄体期,黄体期後期,わからん");
            //Response.Write(Environment.NewLine);

            //foreach (var history in historyList)
            //{
            //    Response.Write(history.ToString());
            //    Response.Write(Environment.NewLine);
            //}
            //Response.AddHeader("content-disposition", "attachment; filename*=utf-8''" + HttpUtility.UrlEncode("記事広告アクセス履歴.csv", System.Text.Encoding.UTF8));
            //Response.ContentType = "text/csv";
            //Response.AddHeader("Pragma", "public");
            //Response.Flush();
            //Response.End();
            //return View();

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
