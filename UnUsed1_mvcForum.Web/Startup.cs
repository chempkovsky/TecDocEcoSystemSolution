using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mvcForum.Web.Startup))]
namespace mvcForum.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
