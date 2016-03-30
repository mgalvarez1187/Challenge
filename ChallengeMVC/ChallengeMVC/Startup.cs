using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChallengeMVC.Startup))]
namespace ChallengeMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
