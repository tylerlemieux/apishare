using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(APIShare.Startup))]
namespace APIShare
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
