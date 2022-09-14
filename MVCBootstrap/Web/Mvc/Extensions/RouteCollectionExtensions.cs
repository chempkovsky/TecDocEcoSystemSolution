using System;
using System.Web.Routing;

namespace MVCBootstrap.Web.Mvc.Extensions {

	public static class RouteCollectionExtensions {

		public static void CreateArea(this RouteCollection routes, String areaName, String controllersNamespace, params Route[] routeEntries) {
			// Note: This code is not done by me, but written by Steven Sanderson.
			// Source: http://blog.stevensanderson.com/2008/11/05/app-areas-in-aspnet-mvc-take-2/
			foreach (Route route in routeEntries) {
				if (route.Constraints == null) {
					route.Constraints = new RouteValueDictionary();
				}
				if (route.Defaults == null) {
					route.Defaults = new RouteValueDictionary();
				}
				if (route.DataTokens == null) {
					route.DataTokens = new RouteValueDictionary();
				}

				route.Constraints["area"] = areaName;
				route.Defaults["area"] = areaName;
				route.DataTokens["namespaces"] = new String[] { controllersNamespace };

				// To support "new Route()" in addition to "routes.MapRoute()"
				if (!routes.Contains(route)) {
					routes.Add(route);
				}
			}
		}
	}
}