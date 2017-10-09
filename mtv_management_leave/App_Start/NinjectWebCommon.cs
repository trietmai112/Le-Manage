[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(mtv_management_leave.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(mtv_management_leave.App_Start.NinjectWebCommon), "Stop")]

namespace mtv_management_leave.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Web.Http;
    using Ninject.Web.WebApi;
    using mtv_management_leave.Models.Entity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Ninject.Activation;
    using Microsoft.AspNet.Identity.Owin;
    using mtv_management_leave.Models;
    using System.Data.Entity;
    using Microsoft.Owin.Security;
    using mtv_management_leave.Lib.Repository;
    using AutoMapper;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();                
                kernel.Bind<LeaveManagementContext>().ToSelf().InRequestScope();
                kernel.Bind<DbContext>().To<LeaveManagementContext>().InRequestScope();
                kernel.Bind<IUserStore<UserInfo, int>>().To<UserStore<UserInfo, Role, int, UserLogin, UserRole, UserClaim>>().InRequestScope();
                kernel.Bind<IAuthenticationManager>().ToMethod(context =>
                {
                    var contextBase = new HttpContextWrapper(HttpContext.Current);
                    return contextBase.GetOwinContext().Authentication;
                });
                kernel.Bind<ApplicationUserManager>().ToSelf().InRequestScope();
                kernel.Bind<ApplicationSignInManager>().ToSelf().InRequestScope();
                kernel.Bind<InOutBase>().ToSelf().InRequestScope();
                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }        
    }
}
