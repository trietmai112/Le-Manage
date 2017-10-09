using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace mtv_management_leave.ModelBinder
{
    public class DateTimeBinder: IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            DateTime dateTime;

            var isDate = DateTime.TryParse(value.AttemptedValue, new DateTimeFormatInfo(), DateTimeStyles.AllowWhiteSpaces, out dateTime);

            if (!isDate)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, new FormatException());
                return DateTime.UtcNow;
            }

            return dateTime;
        }
    }
}
