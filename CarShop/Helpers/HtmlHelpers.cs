// CarShop.Helpers.HtmlHelpers
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarShop.Helpers
{

    public static class HtmlHelpers
    {
        public static MvcHtmlString ExternalLink(this HtmlHelper htmlHelper, string url, object innerHtml, object htmlAttributes = null, object dataAttributes = null)
        {
            TagBuilder tagBuilder = new TagBuilder("a");
            tagBuilder.MergeAttribute("href", url);
            tagBuilder.InnerHtml = innerHtml.ToString();
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes), replaceExisting: true);
            if (dataAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(dataAttributes);
                foreach (KeyValuePair<string, object> item in routeValueDictionary)
                {
                    tagBuilder.MergeAttribute("data-" + item.Key, item.Value.ToString());
                }
            }
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}