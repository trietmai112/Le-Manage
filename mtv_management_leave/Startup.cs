using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mtv_management_leave.Startup))]
namespace mtv_management_leave
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
