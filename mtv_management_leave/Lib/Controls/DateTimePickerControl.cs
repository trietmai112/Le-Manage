using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static mtv_management_leave.Lib.Common;

namespace mtv_management_leave.Lib.Controls
{
    public class DateTimePickerControl : IControl
    {
        private string _lable { get; set; }
        private bool _lableBold { get; set; }
        private DateTimePickerViewMode _viewMode { get; set; } = DateTimePickerViewMode.Full;
        private string _dateFormat { get; set; } = "MM/DD/YYYY h:mm:ss A";
        private string  _value { get; set; }
        private string  _name { get; set; }
        public DateTimePickerControl(string name)
        {
            if (string.IsNullOrEmpty(name)) name = Guid.NewGuid().ToString("N");
            _name = name;
        }
        
        public DateTimePickerControl SetLable(string lable, bool bold = false)
        {
            _lable = lable;
            _lableBold = bold;
            return this;
        }

        public DateTimePickerControl SetViewMode(DateTimePickerViewMode mode)
        {
            _viewMode = mode;
            return this;
        }

        public MvcHtmlString ToHtml()
        {
            throw new NotImplementedException();
        }
    }
}
