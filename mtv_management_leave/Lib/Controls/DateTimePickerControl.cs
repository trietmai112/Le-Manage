using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using static mtv_management_leave.Lib.Common;

namespace mtv_management_leave.Lib.Controls
{
    public class DateTimePickerControl : IControl
    {
        private HtmlHelper _htmlHelper;
        private MvcHtmlString _validationMvcHtmlString;

        private string _lable { get; set; }
        private bool _lableBold { get; set; }
        private DateTimePickerViewMode _viewMode { get; set; } = DateTimePickerViewMode.Full;
        private string _dateFormat { get; set; } = "MM/DD/YYYY hh:mm:ss A";
        private object  _value { get; set; }
        private string  _name { get; set; }
        private string _placeHolder { get; set; }
        public DateTimePickerControl(HtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public DateTimePickerControl SetName(string name)
        {
            _name = name;
            return this;
        }

        public DateTimePickerControl SetPlaceHolder(string placeHolder)
        {
            _placeHolder = placeHolder;
            return this;
        }

        public DateTimePickerControl Binding<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, (_htmlHelper as HtmlHelper<TModel>).ViewData);
            SetPlaceHolder(string.IsNullOrEmpty(_placeHolder) ? metadata.GetPlaceHolder(): _placeHolder);
            if (string.IsNullOrEmpty(_lable)) SetLable(metadata.GetDisplayName());
            _validationMvcHtmlString = (_htmlHelper as HtmlHelper<TModel>).ValidationMessageFor(expression);
            _value = expression.Compile().Invoke((TModel)metadata.Container);
            return this;
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
            if(_viewMode != DateTimePickerViewMode.Full && _viewMode != DateTimePickerViewMode.SideBySide)
            {
                switch (_viewMode)
                {
                    case DateTimePickerViewMode.SideBySide:
                        break;
                    case DateTimePickerViewMode.Full:
                        break;
                    case DateTimePickerViewMode.Date:
                        _dateFormat = "MM/DD/YYYY";
                        break;
                    case DateTimePickerViewMode.Time:
                        _dateFormat = "hh:mm:ss A";
                        break;
                    case DateTimePickerViewMode.Year:
                        _dateFormat = "YYYY";
                        break;
                    case DateTimePickerViewMode.Month:
                        _dateFormat = "MM";
                        break;
                    case DateTimePickerViewMode.Day:
                        _dateFormat = "DD";
                        break;
                    default:
                        break;
                }
            }
            return this;
        }

        public MvcHtmlString ToHtml()
        {
            throw new NotImplementedException();
        }

        private string ScriptTag()
        {
            TagBuilder tag = new TagBuilder("script");
            var script = "";
            return tag.ToString();
        }
    }
}
