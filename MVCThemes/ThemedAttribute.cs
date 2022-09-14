/* MVCThemes
 * Copyright (C) 2011-2012 Steen F. Tøttrup
 * http://creativeminds.dk/
 */

using System;
using System.Web.Mvc;

using MVCThemes.Interfaces;

namespace MVCThemes {

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ThemedAttribute : ActionFilterAttribute {

		public ThemedAttribute() : base() { }

		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			base.OnActionExecuting(filterContext);

			if (filterContext == null) {
				throw new ArgumentNullException("filterContext");
			}

			IThemeProvider provider = DependencyResolver.Current.GetService<IThemeProvider>();
			// Can we locate the theme provider?
			if (provider != null) {
				// Yes, let's set the theme!
				provider.SetTheme(filterContext);
			}
		}
	}
}