using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCBootstrap.Web.Mvc.Extensions {

	public static class LabelExtensions {

		/// <summary>
		/// Extension method for creating a label with additional html attributes.
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="html"></param>
		/// <param name="expression"></param>
		/// <param name="htmlAttributes"></param>
		/// <returns></returns>
		public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes) {
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
			String fieldname = ExpressionHelper.GetExpressionText(expression);

			fieldname = metadata.PropertyName ?? fieldname.Split(new Char[] { '.' }).Last<String>();
			String label = metadata.DisplayName ?? fieldname;
			if (String.IsNullOrEmpty(fieldname)) {
				return MvcHtmlString.Empty;
			}
			TagBuilder tagBuilder = new TagBuilder("label");
			tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(fieldname));
			tagBuilder.SetInnerText(label);
			RouteValueDictionary attr = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
			tagBuilder.MergeAttributes<String, Object>(attr);
			return tagBuilder.ToMvcHtmlString();
		}
	}
}