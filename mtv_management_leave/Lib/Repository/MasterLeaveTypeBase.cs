using System.Collections.Generic;
using System.Linq;
using mtv_management_leave.Lib.Interface;
using mtv_management_leave.Models;

namespace mtv_management_leave.Lib.Repository
{
    public class MasterLeaveTypeBase : Base, IMasterLeaveType
    {
        LeaveManagementContext context;

        public MasterLeaveTypeBase()
        {
        }

        public List<RepoLeaveType> GetLeaveType()
        {
            return PrivateGetLeaveType(null, null);
        }

        public List<RepoLeaveType> GetLeaveType(List<string> lstCode)
        {
            return PrivateGetLeaveType(null, lstCode);
        }

        public List<RepoLeaveType> GetLeaveType(List<int> lstId)
        {
            return PrivateGetLeaveType(lstId, null);
        }

        private List<RepoLeaveType> PrivateGetLeaveType(List<int> lstId, List<string> lstCode)
        {
            InitContext(out context);
            var query = context.MasterLeaveTypes.AsQueryable();
            if (lstId != null)
            {
                query = query.Where(m => lstId.Contains(m.Id));
            }
            if (lstCode != null && lstCode.Count > 0)
            {
                query = query.Where(m => lstCode.Contains(m.LeaveCode));
            }
            var lstResult = query.Select(m => new RepoLeaveType() { Id = m.Id, TypeCode = m.LeaveCode, TypeName = m.Name }).ToList();
            DisposeContext(context);
            return lstResult;
        }

    }
}