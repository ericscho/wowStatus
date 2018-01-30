using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WowStatus.Startup))]
namespace WowStatus
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
