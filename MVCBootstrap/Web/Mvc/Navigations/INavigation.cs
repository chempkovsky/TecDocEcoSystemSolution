using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Navigations {

	public interface INavigation {
		IEnumerable<NavigationItem> GetNavigation(HtmlHelper html);
		Boolean Initialized { get; }
		String Name { get; }
		void Initialize(UrlHelper url);
	}
}