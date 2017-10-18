using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
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
        private string _onChangeHandler;
        private string _onShowHandler;
        private string[] _controlIds;
        private string _flagAttribute = "mti-date-time";
        private bool _hiddenLabel;
        private MvcHtmlString _textBox;

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
            if(string.IsNullOrEmpty(_name))
                _name = name;
            return this;
        }

        public DateTimePickerControl OnChangeJQuery(string handler)
        {
            _onChangeHandler = handler;
            return this;
        }

        public DateTimePickerControl OnShowJQuery(string handler)
        {
            _onShowHandler = handler;
            return this;
        }

        public DateTimePickerControl HiddenLabel()
        {
            _hiddenLabel = true;
            return this;
        }

        public DateTimePickerControl LinkedTo(params string[] controlIds)
        {
            _controlIds = controlIds;
            return this;
        }

        //public DateTimePickerControl SetPlaceHolder(string placeHolder)
        //{
        //    _placeHolder = placeHolder;
        //    return this;
        //}
        

        public DateTimePickerControl Binding<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression, object attributeHtml = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, (_htmlHelper as HtmlHelper<TModel>).ViewData);
            //SetPlaceHolder(string.IsNullOrEmpty(_placeHolder) ? metadata.GetPlaceHolder(): _placeHolder);
            if (string.IsNullOrEmpty(_lable)) SetLable(metadata.GetDisplayName());
            _validationMvcHtmlString = (_htmlHelper as HtmlHelper<TModel>).ValidationMessageFor(expression);
            if(metadata.Container != null)
                _value = expression.Compile().Invoke((TModel)metadata.Container);
            if(string.IsNullOrEmpty(_name))
                _name = metadata.PropertyName + DateTime.Now.Ticks;
            var dic = HtmlHelper.ObjectToDictionary(attributeHtml);
            if (dic == null) dic = new Dictionary<string, object>();

            var attrClass = dic["class"];
            if (dic.ContainsKey("class"))
                dic.Remove("class");
            dic.Add("class", "form-control input-sm" + (string.IsNullOrEmpty(attrClass.vToString()) ? "" : " "+ attrClass ));
            dic.Add(_flagAttribute, "true");
            dic.Add("id", _name);
            if(dic.ContainsKey("attReadonly"))
            {
                dic.Add("readonly", dic["attReadonly"]);
                dic.Remove("attReadonly");
            }
                        
            _textBox = (_htmlHelper as HtmlHelper<TModel>).TextBoxFor(expression, dic);
            
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
            if(_viewMode != DateTimePickerViewMode.Full)
            {
                switch (_viewMode)
                {
                    case DateTimePickerViewMode.Full:
                        break;
                    case DateTimePickerViewMode.Date:
                        _dateFormat = "MM/DD/YYYY";
                        break;
                    case DateTimePickerViewMode.Time:
                        _dateFormat = "hh:mm A";
                        break;
                    case DateTimePickerViewMode.Year:
                        _dateFormat = "YYYY";
                        break;
                    case DateTimePickerViewMode.Month:
                        _dateFormat = "MM/YYYY";
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
        /*
         <div class='form-group'>
                    {{label}}
                    <div class='input-group'>
                         <span class='input-group-addon date-time-picker-clear'><i class='zmdi zmdi-close'></i></span>
                         <div class='dtp-container'>
                               {{textbox}}
                         </div>
                     </div>
                    {{validate-message}}
                </div>


            <div class="form-group">
                    <label class="col-sm-12 control-label" for="DateStart">Date start:</label>
                    <div class="input-group">
                         <span class="input-group-addon date-time-picker-clear"><i class="zmdi zmdi-close"></i></span>
                         <div class="dtp-container">
                               <input class="form-control date-picker input-sm " data-val="true" data-val-date="The field Date start: must be a date." data-val-required="The Date start: field is required." id="DateStart" name="DateStart" placeholder="" type="text" value="">
                         </div>
                     </div>
                    <span class="field-validation-valid" data-valmsg-for="DateStart" data-valmsg-replace="true"></span>
                </div>
             */

        public MvcHtmlString ToHtml()
        {
            if (string.IsNullOrEmpty(_name)) _name = Guid.NewGuid().ToString("N");
            var div = new TagBuilder("div")
                .vAddCssClass("form-group")
                .vAppendText(
                    _hiddenLabel ? null :
                    new TagBuilder("label").vAddCssClass("col-sm-12 control-label").vSetInnerText(_lableBold ? $"<b>{_lable}</b>" : _lable)
                )
                .vAppendText(
                    new TagBuilder("div").vAddCssClass("input-group")
                    .vAppendText("<span class='input-group-addon date-time-picker-clear'><i class='zmdi zmdi-close'></i></span>")
                    .vAppendText(                        
                        new TagBuilder("div").vAddCssClass("dtp-container")
                            .vAppendText(_textBox.ToString())
                        )
                )
                .vAppendText(_validationMvcHtmlString.ToString())
                .vAppendText(
                    ScriptTag()
                );
            return new MvcHtmlString(div.ToString());
        }

        private string ScriptRender()
        {
            var script =
                $@"$('#{_name}').datetimepicker({{";
            script += $"format: '{_dateFormat}'";
            if (_value != null && _value is DateTime) script += $", defaultDate: '{((DateTime)_value).ToString()}'";
            if (_viewMode != DateTimePickerViewMode.Full)
            {
                var mode = _viewMode == DateTimePickerViewMode.Year ? "years" :
                    _viewMode == DateTimePickerViewMode.Month ? "months" : "days";
                script += $@", viewMode: '{mode}'";
            }
            if (_viewMode == DateTimePickerViewMode.Time) script += $", stepping: 15";
            script += $", useCurrent: false";
            script += "});";
            return script;
        }

        private string ScriptOnChangeEvent()
        {
            var script =
                $@"$('#{_name}').on('dp.change', function (e) {{";
            if (!string.IsNullOrEmpty(_onChangeHandler))
            {
                if (_onChangeHandler.Contains("(") && _onChangeHandler.Contains(")"))
                    _onChangeHandler.vReplace("", "(", ")");
                script += $"{_onChangeHandler}($(this));";
            }
            if(!_controlIds.vEmpty())
            {
                foreach (var id in _controlIds)
                {
                    script += $"if($('#{id}').is('[{_flagAttribute}]')){{";
                    script += $"$('#{id}').data('DateTimePicker').date($(this).val());";
                    script += "}else{";
                    script += $"$('#{id}').val($(this).val());";
                    script += "}";
                }
            }
            script += "});";
            return script;
        }

        private string ScriptOnShowEvent()
        {
            var script =
                $"$('#{_name}').on('dp.show', function (e) {{";
            if (!string.IsNullOrEmpty(_onShowHandler))
            {
                if (_onShowHandler.Contains("(") && _onShowHandler.Contains(")"))
                    _onShowHandler.vReplace("", "(", ")");
                script += $"{_onShowHandler}($(this));";
            }
            script += "});";
            return script;
        }

        private string ScriptTag()
        {
            var script = @"<script>";
            script += "$(document).ready(function(e){";
            script += ScriptRender();            
            if (!string.IsNullOrEmpty(_onChangeHandler) || !_controlIds.vEmpty())
                script += ScriptOnChangeEvent();
            if (!string.IsNullOrEmpty(_onShowHandler))
                script += ScriptOnShowEvent();
            script += "});";
            script += "</script>";
            
            return script;
        }
    }
}
