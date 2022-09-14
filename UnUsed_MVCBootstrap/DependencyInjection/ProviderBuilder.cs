// MVCBootstrap.DependencyInjection.ProviderBuilder
using ApplicationBoilerplate.DependencyInjection;
using MVCBootstrap;
using MVCBootstrap.Web;
using MVCBootstrap.Web.Mvc.Interfaces;
using MVCBootstrap.Web.Mvc.Services;
using System.Collections.Generic;

namespace MVCBootstrap.DependencyInjection
{

    public class ProviderBuilder : IDependencyBuilder
    {
        public void Configure(IDependencyContainer container)
        {
            container.RegisterPerRequest<IUserProvider, WebUserProvider>();
            container.RegisterPerRequest<IMailService, MailService>();
            container.RegisterPerRequest<IFormsAuthenticationService, FormsAuthenticationService>();
            container.RegisterPerRequest<IMembershipService, AccountMembershipService>();
        }

        public void ValidateRequirements(IList<ApplicationRequirement> feedback)
        {
        }
    }

}
