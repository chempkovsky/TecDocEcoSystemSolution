//using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using mvcForum.Web.DIServices;
using Owin;
using System;
using System.Linq;
using System.Web.Mvc;

/*
[assembly: OwinStartupAttribute(typeof(mvcForum.Web.Startup))]
namespace mvcForum.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //var services = new ServiceCollection();
            ConfigureAuth(app);
            //ConfigureServices(services);
            //var resolver = new DefaultDependencyResolver(services.BuildServiceProvider());
            //DependencyResolver.SetResolver(resolver);

            ApplicationConfiguration.Initialize();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
               .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
               .Where(t => typeof(IController).IsAssignableFrom(t)
                  || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));
        }
    }
}
*/