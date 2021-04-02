using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheOneTag.WebAPI.Startup))]
namespace TheOneTag.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
