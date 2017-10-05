using mtv_management_leave.Lib.Extendsions;
using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace mtv_management_leave.Lib.Controls
{
    public class ContainerBaseControl : IContainerControl
    {
        public List<string> Controls { get; set; } = new List<string>();

        public virtual MvcHtmlString ToHtml()
        {
            return new MvcHtmlString("");
        }

        public virtual IContainerControl AddControls(params object[] controls)
        {
            foreach (var control in controls)
            {
                if (control is MvcHtmlString)
                {
                    Controls.Add((control as MvcHtmlString).ToHtmlString());
                }
                else if (control is IHtmlString)
                {
                    Controls.Add((control as MvcHtmlString).ToHtmlString());
                }
                else if(control is IControl)
                {
                    Controls.Add((control as IControl).ToHtml().ToHtmlString());
                }
                else if(control is string)
                {
                    Controls.Add(control as String);
                }
            }
            return this;
        }

        public MvcHtmlString Wrap(string name, object attributes)
        {
            var divTag = new TagBuilder(name).vMergeAttributes(attributes.vGetDictionary());
            return new MvcHtmlString(divTag.vAppendText(this.ToHtml().ToHtmlString()).ToString());
        }
    }
}
