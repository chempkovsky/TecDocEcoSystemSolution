using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

using MVCBootstrap.Web.Mvc.Navigations;
using SimpleLocalisation;

namespace MVCBootstrap.Web.Mvc.Extensions {

	public static class HTMLExtensions {

		public static MvcHtmlString AlgebraCaptcha(this HtmlHelper html, String prefix) {
			TagBuilder tag = new TagBuilder("img");
			tag.Attributes.Add("src", String.Format("/algebracaptcha/read?prefix={0}", prefix));
			tag.Attributes.Add("alt", "Algebra Captcha");

			return tag.ToMvcHtmlString();
		}

		[Obsolete("The Navigation method should be used instead.")]
		public static IEnumerable<NavigationItem> GetNavigation(this HtmlHelper html, UrlHelper url, String name) {
			return html.Navigation(url, name);
		}

		/// <summary>
		/// Extension method for getting the list of visible/accessible navigation items for the given navigation.
		/// </summary>
		/// <param name="html">The HtmlHelper.</param>
		/// <param name="url">The UrlHelper.</param>
		/// <param name="name">The name of the requested navigation.</param>
		/// <returns>The list of visible/accessible navigation items.</returns>
		public static IEnumerable<NavigationItem> Navigation(this HtmlHelper html, UrlHelper url, String name) {
			// Get the navigation with the matching name!
			INavigation navigation = DependencyResolver.Current.GetServices<INavigation>().Where(n => n.Name == name).FirstOrDefault();
			// Found one?
			if (navigation == null) {
				// No navigation found, let's "fail" gracefully!
				return new List<NavigationItem>();
			}

			// Has the navigation been initialized?
			if (!navigation.Initialized) {
				// No, let's do it!
				navigation.Initialize(url);
			}

			// Return the list of navigation items!
			return navigation.GetNavigation(html);
		}

		public static MvcHtmlString Link(this HtmlHelper html, String title, String url, Object htmlAttributes) {
			TagBuilder tag = new TagBuilder("a");
			tag.MergeAttribute("href", url);
			tag.SetInnerText(title);
			RouteValueDictionary attr = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
			tag.MergeAttributes<String, Object>(attr);

			return tag.ToMvcHtmlString();
		}

		/// <summary>
		/// Extension method for checking the current action against a given action.
		/// </summary>
		/// <param name="html">The HtmlHelper.</param>
		/// <param name="action">An action name.</param>
		/// <returns>If the given action is the current action, true is returned</returns>
		public static Boolean IsCurrent(this HtmlHelper html, String action) {
			return String.Equals(html.ViewContext.Controller.ControllerContext.RouteData.GetRequiredString("action"), action, StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Extension method for checking the current action against a given action and controller.
		/// </summary>
		/// <param name="html">The HtmlHelper.</param>
		/// <param name="action">An action name.</param>
		/// <param name="controller">A controller name.</param>
		/// <returns>If the given action and controller is the current action and controller, true is returned</returns>
		public static Boolean IsCurrent(this HtmlHelper html, String action, String controller) {
			return String.Equals(html.ViewContext.Controller.ControllerContext.RouteData.GetRequiredString("action"), action, StringComparison.InvariantCultureIgnoreCase) &&
					String.Equals(html.ViewContext.Controller.ControllerContext.RouteData.GetRequiredString("controller"), controller, StringComparison.InvariantCultureIgnoreCase);
		}

		public static String IdFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) {
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
			String fieldname = ExpressionHelper.GetExpressionText(expression);
			fieldname = metadata.DisplayName ?? metadata.PropertyName ?? fieldname.Split(new Char[] { '.' }).Last<String>();

			return TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldname));
		}

		public static String DisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) {
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
			String fieldname = ExpressionHelper.GetExpressionText(expression);

			fieldname = metadata.DisplayName ?? metadata.PropertyName ?? fieldname.Split(new Char[] { '.' }).Last<String>();
			if (String.IsNullOrEmpty(fieldname)) {
				return String.Empty;
			}
			return fieldname;
		}

		public static MvcHtmlString RouteLink(this HtmlHelper html, String linkText, String routeName, RouteValueDictionary routeValues, String anchor) {
			if (String.IsNullOrEmpty(linkText)) {
				throw new ArgumentNullException("linkText");
			}
			TagBuilder tag = new TagBuilder("a");
			tag.SetInnerText(linkText);
			String urlWithoutAnchor = UrlHelper.GenerateUrl(routeName, null, null, null, null, null, routeValues, html.RouteCollection, html.ViewContext.RequestContext, false);

			tag.Attributes.Add("href", String.Format("{0}#{1}", urlWithoutAnchor ,anchor));
			return tag.ToMvcHtmlString();
		}

		/// <summary>
		/// Get the localized string with the given key.
		/// </summary>
		/// <param name="html">The HtmlHelper.</param>
		/// <param name="key">The key of the localized string.</param>
		/// <returns>A localized string.</returns>
		public static String LocalizedString(this HtmlHelper html, String key) {
			return html.LocalizedString(key, String.Empty, null);
		}

		/// <summary>
		/// Get the localized string with the given key and namespace.
		/// </summary>
		/// <param name="html">The HtmlHelper.</param>
		/// <param name="key">The key of the localized string.</param>
		/// <param name="namespace">The namespace.</param>
		/// <returns>A localized string.</returns>
		public static String LocalizedString(this HtmlHelper html, String key, String @namespace) {
			return html.LocalizedString(key, @namespace, null);
		}

		/// <summary>
		/// Get the localized string with the given key, namespace and values.
		/// </summary>
		/// <param name="html">The HtmlHelper.</param>
		/// <param name="key">The key of the localized string.</param>
		/// <param name="namespace">The namespace.</param>
		/// <param name="values">The values to insert into the string.</param>
		/// <returns>A localized string.</returns>
		public static String LocalizedString(this HtmlHelper html, String key, String @namespace, Object values) {
			TextManager text = DependencyResolver.Current.GetService<TextManager>();
			if (text != null) {
				return text.Get(key, values: values, ns: @namespace);
			}
			return String.Empty;
		}

		public static String LocalizedString<TNamespace>(this HtmlHelper html, String key) {
			return html.LocalizedString<TNamespace>(key, null);
		}

		public static String LocalizedString<TNamespace>(this HtmlHelper html, String key, Object values) {
			TextManager text = DependencyResolver.Current.GetService<TextManager>();
			if (text != null) {
				return text.Get<TNamespace>(key, values: values);
			}
			return String.Empty;
		}

		/// <summary>
		/// Get the localized html string with the given key.
		/// </summary>
		/// <param name="html">The HtmlHelper.</param>
		/// <param name="key">The key of the localized html string.</param>
		/// <returns>A localized html string.</returns>
		public static MvcHtmlString LocalizedHtmlString(this HtmlHelper html, String key) {
			return MvcHtmlString.Create(html.LocalizedString(key));
		}

		/// <summary>
		/// Get the localized html string with the given key and namespace.
		/// </summary>
		/// <param name="html">The HtmlHelper.</param>
		/// <param name="key">The key of the localized html string.</param>
		/// <param name="namespace">The namespace.</param>
		/// <returns>A localized html string.</returns>
		public static MvcHtmlString LocalizedHtmlString(this HtmlHelper html, String key, String @namespace) {
			return MvcHtmlString.Create(html.LocalizedString(key, @namespace));
		}

		/// <summary>
		/// Get the localized html string with the given key, namespace and values.
		/// </summary>
		/// <param name="html">The HtmlHelper.</param>
		/// <param name="key">The key of the localized html string.</param>
		/// <param name="namespace">The namespace.</param>
		/// <param name="values">The values to insert into the string.</param>
		/// <returns>A localized html string.</returns>
		public static MvcHtmlString LocalizedHtmlString(this HtmlHelper html, String key, String @namespace, Object values) {
			return MvcHtmlString.Create(html.LocalizedString(key, @namespace, values));
		}
	}
}