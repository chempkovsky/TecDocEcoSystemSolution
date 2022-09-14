/* MVCThemes
 * Copyright (C) 2011-2012 Steen F. Tøttrup
 * http://creativeminds.dk/
 */

using System;
using System.Collections;
using System.Web;
using System.Web.Mvc;

using MVCThemes.Interfaces;

namespace MVCThemes {

	public class QuerystringThemeProvider : IThemeProvider {
		private const String themeKey = "MVCTheme_key";

		public void SetTheme(ActionExecutingContext filterContext) {
			String theme = filterContext.HttpContext.Request.QueryString["theme"];
			if (!String.IsNullOrWhiteSpace(theme)) {
				filterContext.HttpContext.Items.Add(QuerystringThemeProvider.themeKey, theme);
			}
		}

		private String GetTheme(IDictionary items) {
			Object theme = items[QuerystringThemeProvider.themeKey];
			if (theme != null && !String.IsNullOrWhiteSpace((String)theme)) {
				return (String)theme;
			}

			return String.Empty;
		}

		public String GetTheme(ControllerContext controllerContext) {
			return this.GetTheme(controllerContext.HttpContext.Items);
		}

		public string GetTheme() {
			return this.GetTheme(HttpContext.Current.Items);
		}
	}
}