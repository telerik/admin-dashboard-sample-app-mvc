using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdminDashboardMVC.Startup))]
namespace AdminDashboardMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
