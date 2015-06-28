using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Platform.Startup))]
namespace Platform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
