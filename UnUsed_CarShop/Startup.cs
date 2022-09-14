using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarShop.Startup))]
namespace CarShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
