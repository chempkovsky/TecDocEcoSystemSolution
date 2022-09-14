/* MVCThemes
 * Copyright (C) 2011-2012 Steen F. Tøttrup
 * http://creativeminds.dk/
 */

using System;
using System.Web.Mvc;
using System.Web.WebPages;

using MVCThemes.Interfaces;

namespace MVCThemes.Extensions {

	public static class WebBasePageExtensions {

		/// <summary>
		/// Return the base path of the given page, including the current selected theme (if any).
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		[Obsolete("This method does not support Mvc Areas, instead use the extension ThemeViewBasePath on the Urlhelper.")]
		public static String BasePath(this WebPageBase page) {
			// Get the theme provider!
			IThemeProvider provider = DependencyResolver.Current.GetService<IThemeProvider>();
			// Did we get one?
			if (provider != null) {
				// Yes, let's get the theme then!
				String theme = provider.GetTheme();
				// Did we get anything useful ?
				if (!String.IsNullOrWhiteSpace(theme)) {
					// Yeah, let's find a theme URL provider then!
					IThemeURLProvider urlProvider = DependencyResolver.Current.GetService<IThemeURLProvider>();
					// Did we get an URL provider?
					if (urlProvider != null) {
						// Yes, let's get the base URL!
						String basePath = urlProvider.GetThemeBaseURL(theme);
						// Let's build the URL then!
						return String.Format("{0}Views/", basePath);
					}
				}
			}
			// No dice, let's just return the usual views base URL.
			return "~/Views/";

			// TODO: How about areas!??!?!
		}
	}
}
