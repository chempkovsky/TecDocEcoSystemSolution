/* MVCThemes
 * Copyright (C) 2011-2012 Steen F. Tøttrup
 * http://creativeminds.dk/
 */

using System;

using MVCThemes.Interfaces;
using System.Web;
using System.IO;

namespace MVCThemes {

	/// <summary>
	/// The simple implementation of a IThemeURLProvider using the base URL: ~/Themes/{Theme}/
	/// </summary>
	public class SimpleThemeURLProvider : IThemeURLProvider {

		/// <summary>
		/// Builds a path to the root of the Views folder where the views for the given theme are located.
		/// </summary>
		/// <param name="theme">The theme.</param>
		/// <returns>The root of the given theme, always ending with a slash.</returns>
		public string GetThemeBaseURL(String theme) {
			// We don't allow empty strings as themes!
			if (!String.IsNullOrWhiteSpace(theme)) {
				String physicalRoot = HttpContext.Current.Server.MapPath("~/themes/");
				// Does the theme folder exists on the filesystem??
				if (Directory.Exists(Path.Combine(physicalRoot, theme))) {
					// Let's build the base URL for the given theme.
					return String.Format("~/themes/{0}/", theme);
				}
			}

			// Default base URL.
			return "~/";
		}
	}
}