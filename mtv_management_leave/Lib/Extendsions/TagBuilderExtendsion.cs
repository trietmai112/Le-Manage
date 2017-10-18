using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace mtv_management_leave.Lib.Extendsions
{
    public static class TagBuilderExtendsion
    {
        public static TagBuilder vAddCssClass(this TagBuilder tag, string value)
        {
            tag.AddCssClass(value);
            return tag;
        }

        public static TagBuilder vGenerateId(this TagBuilder tag, string value)
        {
            if (string.IsNullOrEmpty(value)) value = Guid.NewGuid().ToString("N");
            tag.GenerateId(value);
            tag.MergeAttribute("name", value);
            return tag;
        }

        public static TagBuilder vMergeAttribute(this TagBuilder tag,string key, string value)
        {
            tag.MergeAttribute(key, value);
            return tag;
        }

        public static TagBuilder vMergeAttributes<TKey, TValue>(this TagBuilder tag, IDictionary<TKey, TValue> attributes)
        {
            if(attributes != null)
                tag.MergeAttributes(attributes);
            return tag;
        }

        public static TagBuilder vSetInnerText(this TagBuilder tag, string text)
        {
            if(text != null)
                tag.SetInnerText(text);
            return tag;
        }

        public static TagBuilder vSetInnerText(this TagBuilder tag, TagBuilder control)
        {
            if(control != null)
                tag.SetInnerText(control.ToString());
            return tag;
        }

        public static TagBuilder vAppendText(this TagBuilder tag, TagBuilder control)
        {
            if(control != null)
                tag.InnerHtml += control.ToString();
            return tag;
        }

        public static TagBuilder vAppendText(this TagBuilder tag, string text)
        {
            if(text != null)
                tag.InnerHtml += text;
            return tag;
        }
    }
}
