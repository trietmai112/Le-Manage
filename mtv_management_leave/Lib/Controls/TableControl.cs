using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Interface;
using mtv_management_leave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace mtv_management_leave.Lib.Controls
{
    public class TableControl : IControl
    {
        private string Name { get; set; }
        private string AjaxUrl { get; set; }
        private AjaxSetting AjaxSetting { get; set; } = new AjaxSetting
        {
            Cache = false,
            RequestType = "POST"
        };
        private string ClassName { get; set; }
        private Dictionary<string, string> AjaxParameters { get; set; } = new Dictionary<string, string>();

        private List<BootGridColumnOption> Rows = new List<BootGridColumnOption>();

        public TableControl SetName(string name)
        {
            if (string.IsNullOrEmpty(name)) name = Guid.NewGuid().ToString("N");
            Name = name;
            return this;
        }

        public TableControl SetAjaxUrl(string url)
        {
            AjaxUrl = url;
            return this;
        }

        public TableControl SetRequestType(string type)
        {
            AjaxSetting.RequestType = type;
            return this;
        }

        public TableControl SetClassName(string className)
        {
            ClassName = className;
            return this;
        }

        public TableControl AddAjaxParameter(string varibleName, string selectorControl)
        {
            AjaxParameters.Add(varibleName, selectorControl);
            return this;
        }

        public TableControl AddAjaxParameter(Type model)
        {
            var dic = model.vGetDictionary();
            foreach (var d in dic)
            {
                AjaxParameters.Add(d.Key.ToLower(), d.Key);
            }
            return this;
        }

        public TableControl AddColumn(BootGridColumnOption column)
        {
            Rows.Add(column);
            return this;
        }

        public TableControl AddColumnRange(params BootGridColumnOption[] columns)
        {
            Rows.AddRange(columns);
            return this;
        }

        private string RenderBootGridScript()
        {
            var scriptHtml = $"<script type='text/javascript'>";
            scriptHtml += "     $(document).ready(function() {";
            scriptHtml += $"        var grid = $('#{this.Name}').bootgrid({"{"}";
            scriptHtml += $"                ajax: true,";
            if (this.AjaxParameters != null && this.AjaxParameters.Count > 0)
            {
                scriptHtml += "             post: function() {";
                scriptHtml += "                     return {";
                int j = 0;
                foreach (var item in this.AjaxParameters)
                {
                    scriptHtml += $"{item.Key}: $('#{item.Value}').val()" + (j < this.AjaxParameters.Count - 1 ? "," : "");
                    j++;
                }
                scriptHtml += "                             };";
                scriptHtml += "             },";
            }
            scriptHtml += "                 templates: { header: '' },";
            scriptHtml += "                 ajaxSettings:{";
            scriptHtml += $"                        method: '{this.AjaxSetting.RequestType}',";
            scriptHtml += $"                        cache: {this.AjaxSetting.Cache.ToString().ToLower()}";
            scriptHtml += "                 },";
            scriptHtml += $"                url: '{this.AjaxUrl}',";
            scriptHtml += $"                selection: true,";
            scriptHtml += $"                rowSelect: true,";
            scriptHtml += $"                keepSelection: true,";
            scriptHtml += $"                multiSelect: true,";
            scriptHtml += "                 formatters: {";

            //format data on row
            var rowsFormat = Rows.Where(m => !string.IsNullOrEmpty(m.FormatJqueryHandler));
            int i = 0;
            foreach (var row in rowsFormat)
            {
                scriptHtml += $"                \"{row.MappingFrom}\": function(column, row) {"{"}";
                scriptHtml += $"                    return {row.FormatJqueryHandler}(column, row);";
                scriptHtml += "                     }" + (i < rowsFormat.Count() ? "," : "");
                i++;
            }
            scriptHtml += "                 }";
            scriptHtml += "                 }).on('loaded.rs.jquery.bootgrid', function() {";
            ///input code register function when gird load complete    
            scriptHtml += "                 });";
            scriptHtml += "     });";
            scriptHtml += "</script>";

            return scriptHtml;
        }

        public MvcHtmlString ToHtml()
        {
            var trTag = new TagBuilder("tr")
                .vGenerateId($"table_header_{this.Name}");

            foreach (var row in this.Rows)
            {
                var thTag = new TagBuilder("th")
                    .vMergeAttribute("data-column-id", row.MappingFrom)
                    .vMergeAttribute("data-sortable", row.Sortable.ToString().ToLower());
                if (row.Identify)
                {
                    thTag.Attributes.Add("data-type", "numeric");
                    thTag.Attributes.Add("data-identifier", row.Identify.vToString().ToLower());
                }
                if (!string.IsNullOrEmpty(row.OrderBy)) thTag.Attributes.Add("data-order", row.OrderBy);
                thTag.SetInnerText(row.ColumnHeaderName);
                if (!string.IsNullOrEmpty(row.FormatJqueryHandler)) thTag.Attributes.Add("data-formatter", row.MappingFrom);

                trTag.vAppendText(thTag);
            }
            var theadTag = new TagBuilder("thead").vAppendText(trTag);

            var tableTag = new TagBuilder("table")
                .vAddCssClass("table table-condensed table-hover table-striped " + ClassName)
                .vGenerateId(this.Name).vAppendText(theadTag);

            var divTag = new TagBuilder("div")
                .vAddCssClass("table-responsive ")
                .vGenerateId($"div_{this.Name}").vAppendText(tableTag)
                .vAppendText(new StringBuilder(this.RenderBootGridScript()).Replace("  ", "").ToString())
                .vAppendText(

                    $@"<script> $(document).ajaxError(function(event, jqxhr, settings, thrownError) {{
                        if (settings.url == '{this.AjaxUrl}')
                        {{
                            if (jqxhr.responseText == null || jqxhr.responseText == '')
                            {{
                                swal('Ajax error', thrownError, 'error');
                            }}
                            else
                            {{
                                var doc = $(jqxhr.responseText);
                                var message = doc[1].innerText;
                                swal('Ajax error', message, 'error');
                            }}
                        }}
                    }});</script>");
            return new MvcHtmlString(divTag.ToString());
        }
    }
}
