using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sipro_2017.Startup))]
namespace Sipro_2017
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
