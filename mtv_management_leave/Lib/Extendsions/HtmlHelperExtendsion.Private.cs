using mtv_management_leave.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace mtv_management_leave.Lib.Extendsions
{
    public static partial class HtmlHelperExtendsion
    {
        private readonly static string _labelDivClass = "col-sm-12 control-label";
        private readonly static string _textDivClass = "col-sm-12";
        private readonly static string _textboxTemplate =
            $@"<div class='form-group'>
                {{label}}
                <div class='{_textDivClass}'>
                    <div class='fg-line'>
                        {{textbox}}                        
                    </div>     
                    {{validate-message}}
                </div>
              </div>";

        private readonly static string _textareaTemplate =
            $@"<div class='form-group'>
                {{label}}
                <div class='{_textDivClass}'>
                    <div class='fg-line'>
                        {{textarea}}                        
                    </div>     
                    {{validate-message}}
                </div>
              </div>";

        private readonly static string _selectTemplate =
            $@"<div class='form-group'>
                {{label}}
                <div class='{_textDivClass}'>
                    <div class='fg-line'>
                        {{select}}                        
                    </div>  
                    {{validate-message}}
                </div>
            </div>";


        private readonly static string _checkBoxTemplate =
           $@"<div class='form-group'>
                <div class='{{offset}}'>
                    <div class='checkbox'>
                        <label>
                            {{checkbox}}
                            <i class='input-helper'></i>
                            {{label}}
                        </label>
                    </div>
                    {{checkbox-hidden}}
                </div>
            </div>";

        private readonly static string _dateTimePickerTemplate =
            $@"<div class='form-group'>
                    {{label}}
                    <div class='input-group'>
                         <span class='input-group-addon date-time-picker-clear'><i class='zmdi zmdi-close'></i></span>
                         <div class='dtp-container'>
                               {{textbox}}
                         </div>
                     </div>
                    {{validate-message}}
                </div>";

            //$@"<div class='form-group'>
            //    {{label}}
                
            //    <div class='{_textDivClass}'>
            //        <div class='input-group'>
            //            <span class='input-group-addon'><i class='zmdi zmdi-time'></i></span>
            //            <div class='dtp-container fg-line'>
            //                {{textbox}}
            //                <button class='btn btn-link'>x</button>
            //            </div>
            //        </div>
                    
            //        {{validate-message}}
            //    </div>
            //  </div>";


        private static string CombineVaribleToLayout(string layout, Dictionary<string, string> dictionary)
        {
            var htmlString = layout;
            foreach (var item in dictionary)
            {
                htmlString = Regex.Replace(htmlString, item.Key, item.Value);
            }
            return htmlString;
        }

        private static MvcHtmlString CreateLabelMvcString<TModel, TProperty>(HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.LabelFor(expression, new { @class = _labelDivClass });
        }

        private static MvcHtmlString RenderDateTimePicker<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string placeHolder, object attributes, string inputClass, bool showTitle)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);


            var placeHolderText = string.IsNullOrEmpty(placeHolder) ? metadata.GetPlaceHolder() : placeHolder;

            var dic = new Dictionary<string, object>();
            dic.Add("class", inputClass);
            dic.Add("placeholder", placeHolderText);

            var inputHtmlString = htmlHelper.TextBoxFor(expression, dic);
            var labelHtmlString = CreateLabelMvcString(htmlHelper, expression);
            var validateMessageHtmlString = htmlHelper.ValidationMessageFor(expression);

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("{label}", showTitle ? labelHtmlString.ToHtmlString() : "");
            dictionary.Add("{textbox}", inputHtmlString.ToHtmlString());
            dictionary.Add("{validate-message}", validateMessageHtmlString.ToHtmlString());

            return new MvcHtmlString(CombineVaribleToLayout(_dateTimePickerTemplate, dictionary));
        }
    }
}
