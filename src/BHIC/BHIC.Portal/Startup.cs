using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BHIC.Portal.Startup))]
namespace BHIC.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
