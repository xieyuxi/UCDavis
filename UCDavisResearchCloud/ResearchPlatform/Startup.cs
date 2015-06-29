using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ResearchPlatform.Startup))]
namespace ResearchPlatform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
