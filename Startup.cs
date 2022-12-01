using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mss_project.Startup))]
namespace mss_project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
