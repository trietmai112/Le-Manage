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
        public void InitContext(LeaveManagementEntities context)
        {
            context = new LeaveManagementEntities();
        }
        public void DisposeContext (LeaveManagementEntities context)
        {
            context.Dispose();
        }
    }
}