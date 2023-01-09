using System.Diagnostics;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using mss_project.DatabaseStuff;
using Org.BouncyCastle.Bcpg.Sig;
using Owin;
using mss_project.Helpers;

[assembly: OwinStartupAttribute(typeof(mss_project.Startup))]
namespace mss_project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            Debug.WriteLine("Before Seeding");
            DataSeeder.getInstance().SeedData().Wait();
			Debug.WriteLine("After Seeding");
		}
    }
}
