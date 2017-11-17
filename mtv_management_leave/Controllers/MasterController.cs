using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Repository;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Request;
using System.Web.Mvc;

namespace mtv_management_leave.Controllers
{
    public class MasterController:Controller
    {
        private CalendarEventBase _calendarEvent;

        public MasterController(CalendarEventBase calendarEvent)
        {
            _calendarEvent = calendarEvent;
        }
        public ActionResult LeftSidebar()
        {
            return PartialView("_LeftSidebar", model: User.GetRoleName());
        }

        [HttpPost]
        public JsonResult GetCalendarEvent(CalendarEventViewRequest model)
        {
            var result = _calendarEvent.GetEvents(model.Start, model.End); 
            return Json(result);
        }
    }
}
