/* MVCThemes
 * Copyright (C) 2011-2012 Steen F. Tøttrup
 * http://creativeminds.dk/
 */

using System;
using System.Web.Mvc;

using MVCThemes.Interfaces;

namespace MVCThemes.Extensions {

	public static class UrlHelperExtensions {

		/// <summary>
		/// Extension method for getting the base path of the views folder,
		/// taking into account the current theme and area if any.
		/// </summary>
		/// <returns>The base path to the views folder</returns>
		public static String ThemeViewBasePath(this UrlHelper url) {
			String area = String.Empty;
			// Is the request is to a controller in an area?
			if (url.RequestContext.RouteData.Values.ContainsKey("area")) {
				// Get it!
				area = url.RequestContext.RouteData.Values["area"] as String;
			}

			String rootPath = url.ThemeRootPath();
			// Let's return the path to the views folder, we'll remember the area if any.
			return String.Format("{0}{1}Views/", rootPath, String.IsNullOrWhiteSpace(area) ? "" : String.Format("areas/{0}/", area));
		}

		/// <summary>
		/// Method for getting the current theme, if any.
		/// </summary>
		/// <returns>The selected theme.</returns>
		private static String GetTheme() {
			// Get the theme provider!
			IThemeProvider provider = DependencyResolver.Current.GetService<IThemeProvider>();
			// Did we get one?
			if (provider != null) {
				// Yes, let's get the theme then!
				return provider.GetTheme();
			}
			return String.Empty;
		}

		/// <summary>
		/// Extension method for getting the path to the content folder of the theme selected.
		/// If no theme is selected, the content folder in the root folder is returned.
		/// </summary>
		/// <returns></returns>
		public static String ContentPath(this UrlHelper url) {
			return String.Format("{0}{1}", url.ThemeRootPath(), "content/");
		}

		/// <summary>
		/// Extension method for getting the root path of the theme. If no theme is selected, the root
		/// of the site is returned.
		/// </summary>
		/// <param name="url"></param>
		/// <returns>The root of the selected theme.</returns>
		public static String ThemeRootPath(this UrlHelper url) {
			String theme = GetTheme();
			// Is any theme selected?
			if (String.IsNullOrEmpty(theme)) {
				// No theme, let's return the root of the site!
				return String.Format("~/");
			}
			else {
				return String.Format("~/themes/{0}/", theme);
			}
		}
	}
}