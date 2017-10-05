using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace mtv_management_leave.Lib.Controls
{
    public class TableHeaderControl: ContainerBaseControl
    {
        public override MvcHtmlString ToHtml()
        {
            var divChildTag = new TagBuilder("div").vAddCssClass("col-sm-12 actionBar");
            foreach(var control in Controls)
            {
                divChildTag.vAppendText(control);
            }

            var divTag = new TagBuilder("div").vAddCssClass("row").vAppendText(divChildTag);
            return new MvcHtmlString(divTag.ToString());
        }
    }
}
