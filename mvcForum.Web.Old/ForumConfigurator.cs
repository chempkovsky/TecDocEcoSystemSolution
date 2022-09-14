// mvcForum.Web.ForumConfigurator
using ApplicationBoilerplate.DependencyInjection;
using BoC.Web.Mvc.PrecompiledViews;
using MVCBootstrap.Web.Mvc;
using mvcForum.Web.Filters;
using MVCThemes;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace mvcForum.Web
{

    public class ForumConfigurator : IDependencyBuilder
    {
        public void Configure(IDependencyContainer container)
        {
            HostingEnvironment.RegisterVirtualPathProvider(new CompiledVirtualPathProvider());
            ApplicationPartRegistry.Register(GetType().Assembly);
            IEnumerable<IViewEngine> source = from x in ViewEngines.Engines
                                              where x is RazorViewEngine
                                              select x;
            if (source.Any())
            {
                ViewEngines.Engines.Remove(source.First());
            }
            ViewEngines.Engines.Add(new ThemeViewEngine());
            if (!(ModelBinders.Binders.DefaultBinder is FlagEnumerationModelBinder))
            {
                ModelBinders.Binders.DefaultBinder = new FlagEnumerationModelBinder();
            }
            if (!(from x in GlobalFilters.Filters
                  where x.Instance is LastVisitedFilterAttribute
                  select x).Any())
            {
                GlobalFilters.Filters.Add(new LastVisitedFilterAttribute());
            }
        }

        public void ValidateRequirements(IList<ApplicationRequirement> feedback)
        {
        }
    }

}