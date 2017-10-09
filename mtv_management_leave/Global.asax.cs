using AutoMapper;
using mtv_management_leave.Models.Account;
using mtv_management_leave.Models.Entity;
using mtv_management_leave.Models.RegisterLeave;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace mtv_management_leave
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap(typeof(Models.Account.RegisterViewModel), typeof(Models.Entity.UserInfo))
                         .AfterMap((sObject, dObject) => {
                             if((sObject is RegisterViewModel) && (dObject is UserInfo))
                             {
                                 var des = dObject as UserInfo;
                                 var source = sObject as RegisterViewModel;
                                 des.UserName = source.Email;
                             }
                         })
                        .ReverseMap();
                cfg.CreateMap<Models.RegisterLeave.RegisterLeaveRequest, Models.Entity.RegisterLeave>()
                        .AfterMap((sObject, dObject) => {
                            if(sObject is RegisterLeaveRequest && dObject is RegisterLeave )
                            {
                                var source = sObject as RegisterLeaveRequest;
                                var des = dObject as RegisterLeave;
                                if (source.IsToday) des.RegisterHour = (des.DateEnd - des.DateStart).TotalHours;
                                des.DateRegister = DateTime.Now;
                                des.DateUpdated = DateTime.Now;
                            }
                        })
                        .ReverseMap();
            });

            System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime?), new mtv_management_leave.ModelBinder.NullableDateTimeBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime), new mtv_management_leave.ModelBinder.DateTimeBinder());

        }
    }
}
