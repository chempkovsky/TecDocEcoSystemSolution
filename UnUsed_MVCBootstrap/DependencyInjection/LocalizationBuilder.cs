// MVCBootstrap.DependencyInjection.LocalizationBuilder
using ApplicationBoilerplate.DependencyInjection;
using SimpleLocalisation;
using SimpleLocalisation.Web;
using System.Collections.Generic;
using System.Web;

namespace MVCBootstrap.DependencyInjection
{

    public class LocalizationBuilder : IDependencyBuilder
    {
        public void Configure(IDependencyContainer container)
        {
            TextManager instance = new TextManager(new WebCultureContext(), new XmlFileTextSource(() => HttpContext.Current.Server.MapPath("~/app_data/texts")), new Language[1]
            {
            new Language("en-GB")
            });
            container.RegisterSingleton(instance);
        }

        public void ValidateRequirements(IList<ApplicationRequirement> feedback)
        {
        }
    }

}
