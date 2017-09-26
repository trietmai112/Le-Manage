using mtv_management_leave.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtv_management_leave.Models;
using mtv_management_leave.Models.Entity;

namespace mtv_management_leave.Lib.Repository
{
    public class Base 
    {
        public void InitContext(out LeaveManagementContext context)
        {
            context = new LeaveManagementContext();
        }
        public void DisposeContext (LeaveManagementContext context)
        {
            context.Dispose();
        }
    }
}