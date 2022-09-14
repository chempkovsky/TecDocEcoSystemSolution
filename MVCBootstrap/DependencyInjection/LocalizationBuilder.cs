using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

using ApplicationBoilerplate.DependencyInjection;
using SimpleLocalisation;
using SimpleLocalisation.Web;
using System.Configuration;

namespace MVCBootstrap.DependencyInjection {

	/// <summary>
	/// Dependency builder that handles wiring of the localization framework.
	/// </summary>
	public class LocalizationBuilder : IDependencyBuilder {

		public LocalizationBuilder() {
		}

		public void Configure(IDependencyContainer container) {
			TextManager manager = new TextManager(
				new WebCultureContext(),
				new XmlFileTextSource(() => HttpContext.Current.Server.MapPath("~/app_data/texts")),
				new Language[] { new Language("en-GB") });
			// Let's make it fall back to english if everything else fails!
			//manager.FallbackLanguages = new List<Language> { new LanguageInfo { Key = "en-GB" } };

			// The text/localisation manager runs in singleton mode.
			container.RegisterSingleton<TextManager>(manager);
		}

		public void ValidateRequirements(IList<ApplicationRequirement> feedback) { }
	}
}