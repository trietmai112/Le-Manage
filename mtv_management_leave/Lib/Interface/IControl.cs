using System.Web.Mvc;

namespace mtv_management_leave.Lib.Interface
{
    public interface IControl
    {       
        MvcHtmlString ToHtml();
        MvcHtmlString Wrap(string name, object attributes);
    }
}
