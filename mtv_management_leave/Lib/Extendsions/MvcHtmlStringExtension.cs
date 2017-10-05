using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace mtv_management_leave.Lib.Extendsions
{
    public static class MvcHtmlStringExtension
    {
        public static MvcHtmlString Wrap(this MvcHtmlString mvcHtml, string name, object attribute)
        {
            var tag = new TagBuilder(name).vMergeAttributes(attribute.vGetDictionary())
                .vAppendText(mvcHtml.ToHtmlString());
            return new MvcHtmlString(tag.ToString());
        }
    }
}
