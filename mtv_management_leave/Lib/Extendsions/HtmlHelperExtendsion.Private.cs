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

        private static string RenderBootGridHtml(BootGridTableOption bootGridTableOption, List<BootGridRowOption> rowOptions)
        {
            var tableHtml = $"<table id='{bootGridTableOption.GuiId}' class='table table-condensed table-hover table-striped {bootGridTableOption.ClassName}'>";
            tableHtml += $"     <thead>";
            tableHtml += "          <tr>";
            foreach(var row in rowOptions)
            {
                var thTag = new TagBuilder("th");
                thTag.Attributes.Add("data-column-id", row.MappingFrom);
                thTag.Attributes.Add("data-sortable", row.Sortable.ToString());
                if (row.Identify)
                {
                    thTag.Attributes.Add("data-type", "numeric");
                    thTag.Attributes.Add("data-identifier", row.Identify.ToString());
                }
                if (!string.IsNullOrEmpty(row.OrderBy)) thTag.Attributes.Add("data-order", row.OrderBy);
                thTag.SetInnerText(row.ColumnHeaderName);
                if (!string.IsNullOrEmpty(row.FormatJqueryHandler)) thTag.Attributes.Add("data-formatter", row.FormatJqueryHandler);

                tableHtml += thTag.ToString();
            }
            tableHtml += "          </tr>";
            tableHtml += $"     </thead>";
            tableHtml += $" </table>";

            return tableHtml;
        }

        private static string RenderBootGridScript(BootGridTableOption bootGridTableOption, List<BootGridRowOption> rowOptions)
        {
            var scriptHtml = $"<script type='text/javascript'>";
            scriptHtml += "     $(document).ready(function() {";
            scriptHtml += $"        var grid = $('#{bootGridTableOption.GuiId}').bootgrid({"{"}";
            scriptHtml += $"                ajax: true,";
            if(bootGridTableOption.AjaxParameters != null && bootGridTableOption.AjaxParameters.Count > 0)
            {
                scriptHtml += "             post: function() {";
                scriptHtml += "                     return {";
                int i = 0;
                foreach(var item in bootGridTableOption.AjaxParameters)
                {
                    scriptHtml += $"{item.Key}: $('{item.Value}').val()" + (i < bootGridTableOption.AjaxParameters.Count - 1 ? ",":"");
                }
                scriptHtml += "                             };";
                scriptHtml += "             },";
            }
            scriptHtml += "                 templates: { header: '' },";
            scriptHtml += "                 ajaxSettings:{";
            scriptHtml += $"                        method: '{bootGridTableOption.AjaxSettings.RequestType}',";
            scriptHtml += $"                        cache: {bootGridTableOption.AjaxSettings.Cache.ToString().ToLower()}";
            scriptHtml += "                 },";
            ///input code ajax setting here [TODO]
            scriptHtml += $"                url: '{bootGridTableOption.AjaxUrl}',";
            scriptHtml += "                 formatters: {";
            /// input code format here [TODO]
            scriptHtml += "                 }";
            scriptHtml += "                 }).on('loaded.rs.jquery.bootgrid', function() {";
            ///input code register function when gird load complete    
            scriptHtml += "                 });";
            scriptHtml += "     });";
            scriptHtml += "</script>";
            
            return scriptHtml;
        }
    }
}
