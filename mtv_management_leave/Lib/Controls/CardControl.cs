using System;
using System.Web.Mvc;
using mtv_management_leave.Lib.Interface;
using System.Collections.Generic;
using mtv_management_leave.Lib.Extendsions;

namespace mtv_management_leave.Lib.Controls
{
    public class CardControl : IControl
    {
        public string Name { get; private set; }
        public string Header { get; set; }
        public string HeaderSummary { get; set; }
        public string ClassName { get; set; }
        public Dictionary<string, object> Actions { get; set; } = new Dictionary<string, object>();
        public List<MvcHtmlString> Bodys { get; set; } = new List<MvcHtmlString>();

        public CardControl SetName(string name)
        {
            if (string.IsNullOrEmpty(name)) name = Guid.NewGuid().ToString("N");
            Name = name;
            return this;
        }

        public CardControl SetHeader(string header)
        {
            Header = header;
            return this;
        }

        public CardControl SetSummary(string summary)
        {
            HeaderSummary = summary;
            return this;
        }

        public CardControl SetClassName(string className)
        {
            ClassName = className;
            return this;
        }

        public CardControl AddAction(string text, object attribute)
        {
            this.Actions.Add(text, attribute);
            return this;
        }

        public CardControl AddBody(IControl control)
        {
            this.Bodys.Add(control.ToHtml());
            return this;
        }

        public CardControl AddBody(MvcHtmlString control)
        {
            this.Bodys.Add(control);
            return this;
        }

        public MvcHtmlString ToHtml()
        {            
            var header = $"<h2>{this.Header} <small>{this.HeaderSummary}</small></h2>";

            var action = "";
            if (Actions.Count > 0)
            {
                var ulTag = new TagBuilder("ul").vAddCssClass("actions");
                var liOne = new TagBuilder("li").vAddCssClass("dropdown action-show")
                    .vAppendText($"<a href='#' data-toggle='dropdown' aria-expanded='false'><i class='zmdi zmdi-more-vert'></i></a>");
                var ulChildTag = new TagBuilder("ul").vAddCssClass("dropdown-menu dropdown-menu-right");
                foreach (var act in this.Actions)
                {
                    var tag = new TagBuilder("a")                        
                        .vMergeAttributes(act.Value.vGetDictionary())
                        .vSetInnerText(act.Key);
                    if (!tag.Attributes.ContainsKey("href")) tag.vMergeAttribute("href", "#");
                    ulChildTag.vAppendText($"<li>{tag.ToString()}</li>");
                }
                liOne.vAppendText(ulChildTag);
                ulTag.vAppendText(liOne);
                action = ulTag.ToString();
            }
            var divHeaderTag = new TagBuilder("div")
                .vAddCssClass("card-header")
                .vAppendText(header)
                .vAppendText(action);

            var divBody = new TagBuilder("div")
                .vAddCssClass("card-body");

            foreach(var b in Bodys)
            {
                divBody.vAppendText(b.ToHtmlString());
            }

            var divTag = new TagBuilder("div")
                .vAddCssClass("card " + ClassName)
                .vGenerateId(this.Name)
                .vAppendText(divHeaderTag)
                .vAppendText(divBody);

            return new MvcHtmlString(divTag.ToString());
        }       
    }
}
