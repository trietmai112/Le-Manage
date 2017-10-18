using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace mtv_management_leave.Lib.Extendsions
{
    public static class HttpContextExtendsion
    {
        public static T GetFromNInject<T>(this HttpContext context)
        {
            var obj = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IKernel)) as IKernel;
            return obj.Get<T>();
        }
    }
}
