/* MVCThemes
 * Copyright (C) 2011-2012 Steen F. Tøttrup
 * http://creativeminds.dk/
 */

using System;
using System.Web.Mvc;

namespace MVCThemes.Interfaces {

	public interface IThemeProvider {
		void SetTheme(ActionExecutingContext filterContext);
		String GetTheme(ControllerContext controllerContext);
		String GetTheme();
	}
}