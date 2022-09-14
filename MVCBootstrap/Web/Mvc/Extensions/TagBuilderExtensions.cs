using System;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Extensions {

	public static class TagBuilderExtensions {

		/// <summary>
		/// Extension method for creating a MvcHtmlString from a TagBuilder object.
		/// </summary>
		/// <param name="tagBuilder">The TagBuilder instance.</param>
		/// <returns>The MvcHtmlString output of the given TagBuilder.</returns>
		public static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder) {
			return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
		}
	}
}