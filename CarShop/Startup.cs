// CarShop.Startup
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;



namespace CarShop
{

    public class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Account/Login")
            });
            app.UseExternalSignInCookie("ExternalCookie");
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

}

/*
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

*/