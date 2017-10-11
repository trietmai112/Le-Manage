using System.Collections.Generic;
using mtv_management_leave.Models;

namespace mtv_management_leave.Lib.Interface
{
    interface IMasterLeaveType
    {
        List<RepoLeaveType> GetLeaveType();
        List<RepoLeaveType> GetLeaveType(List<int> lstId);
        List<RepoLeaveType> GetLeaveType(List<string> lstCode);
    }
}
