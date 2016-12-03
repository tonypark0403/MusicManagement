using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SenecaMusic.Startup))]
namespace SenecaMusic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
