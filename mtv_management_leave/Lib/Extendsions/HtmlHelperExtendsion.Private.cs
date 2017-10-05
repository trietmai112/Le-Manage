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
        private readonly static string _labelDivClass = "col-sm-3 control-label";
        private readonly static string _textboxTemplate =
            @"<div class='form-group'>
                {label}
                <div class='col-sm-9'>
                    <div class='fg-line'>
                        {textbox}                        
                    </div>     
                    {validate-message}
                </div>
              </div>";

        private readonly static string _selectTemplate =
            @"<div class='form-group'>
                {label}
                <div class='col-sm-9'>
                    <div class='fg-line'>
                        {select}                        
                    </div>  
                    {validate-message}
                </div>";


        private readonly static string _checkBoxTemplate =
            @"<div class='form-group'>
                <div class='{offset}'>
                    <div class='checkbox'>
                        <label>
                            {checkbox}
                            <i class='input-helper'></i>
                            {label}
                        </label>
                    </div>
                    {checkbox-hidden}
                </div>
            </div>";

        private readonly static string _dateTimePickerTemplate =
            @"<div class='input-group form-group'>
                {label}
                <div class='col-sm-9'>
                    <div class='dtp-container fg-line'>
                        {textbox}
                    </div>
                    {validate-message}
                </div>
              </div>";    

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
    }
}
