using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace mtv_management_leave.ModelBinder
{
    public class NullableDateTimeBinder : System.Web.Mvc.IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (string.IsNullOrWhiteSpace(value.AttemptedValue))
            {
                return null;
            }

            DateTime dateTime;

            var isDate = DateTime.TryParse(value.AttemptedValue, new DateTimeFormatInfo(), DateTimeStyles.AllowWhiteSpaces, out dateTime);

            if (!isDate)
            {
                //bindingContext.ModelState.AddModelError(bindingContext.ModelName, new FormatException());
                return null;
            }

            return dateTime;
        }
    }
}
