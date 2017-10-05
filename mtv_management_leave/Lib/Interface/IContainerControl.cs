using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtv_management_leave.Lib.Interface
{
    public interface IContainerControl: IControl
    {
        List<string> Controls { get; set; }

        IContainerControl AddControls(params object[] controls);
    }
}
