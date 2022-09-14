using System;
using System.Collections.Generic;

using ApplicationBoilerplate.DependencyInjection;

using MVCBootstrap.Web;
using MVCBootstrap.Web.Mvc.Interfaces;
using MVCBootstrap.Web.Mvc.Services;

namespace MVCBootstrap.DependencyInjection {

	/// <summary>
	/// This dependency builders binds the most common interfaces/services needed when using users on your site.
	/// A <see ref="MVCBootstrap.Web.WebUserProvider">WebUserProvider</see> for getting the current user,
	/// <see ref="MVCBootstrap.Web.FormsAuthenticationservice">FormsAuthenticationservice</see> for handling authentication,
	/// <see ref="MVCBootstrap.Web.AccountMembershipService">AccountMembershipService</see> for ASP.NET membership and a MailService for sending e-mails.
	/// </summary>
	public class ProviderBuilder : IDependencyBuilder {

		public void Configure(IDependencyContainer container) {
			container.RegisterPerRequest<IUserProvider, WebUserProvider>();
			container.RegisterPerRequest<IMailService, MailService>();
			container.RegisterPerRequest<IFormsAuthenticationService, FormsAuthenticationService>();
			container.RegisterPerRequest<IMembershipService, AccountMembershipService>();
		}

		public void ValidateRequirements(IList<ApplicationRequirement> feedback) { }
	}
}