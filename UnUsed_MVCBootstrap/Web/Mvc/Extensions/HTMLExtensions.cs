// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Extensions.HTMLExtensions
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using MVCBootstrap.Web.Mvc.Navigations;
using SimpleLocalisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCBootstrap.Web.Mvc.Extensions
{
  public static class HTMLExtensions
  {
    public static MvcHtmlString AlgebraCaptcha(this HtmlHelper html, string prefix)
    {
      return new TagBuilder("img")
      {
        Attributes = {
          {
            "src",
            string.Format("/algebracaptcha/read?prefix={0}", (object) prefix)
          },
          {
            "alt",
            "Algebra Captcha"
          }
        }
      }.ToMvcHtmlString();
    }

    [Obsolete("The Navigation method should be used instead.")]
    public static IEnumerable<NavigationItem> GetNavigation(
      this HtmlHelper html,
      UrlHelper url,
      string name)
    {
      return html.Navigation(url, name);
    }

    public static IEnumerable<NavigationItem> Navigation(
      this HtmlHelper html,
      UrlHelper url,
      string name)
    {
      INavigation navigation = DependencyResolver.Current.GetServices<INavigation>().Where<INavigation>((Func<INavigation, bool>) (n => n.Name == name)).FirstOrDefault<INavigation>();
      if (navigation == null)
        return (IEnumerable<NavigationItem>) new List<NavigationItem>();
      if (!navigation.Initialized)
        navigation.Initialize(url);
      return navigation.GetNavigation(html);
    }

    public static MvcHtmlString Link(
      this HtmlHelper html,
      string title,
      string url,
      object htmlAttributes)
    {
      TagBuilder tagBuilder = new TagBuilder("a");
      tagBuilder.MergeAttribute("href", url);
      tagBuilder.SetInnerText(title);
      RouteValueDictionary htmlAttributes1 = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
      tagBuilder.MergeAttributes<string, object>((IDictionary<string, object>) htmlAttributes1);
      return tagBuilder.ToMvcHtmlString();
    }

    public static bool IsCurrent(this HtmlHelper html, string action)
    {
      return string.Equals(html.ViewContext.Controller.ControllerContext.RouteData.GetRequiredString(nameof (action)), action, StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool IsCurrent(this HtmlHelper html, string action, string controller)
    {
      if (string.Equals(html.ViewContext.Controller.ControllerContext.RouteData.GetRequiredString(nameof (action)), action, StringComparison.InvariantCultureIgnoreCase))
        return string.Equals(html.ViewContext.Controller.ControllerContext.RouteData.GetRequiredString(nameof (controller)), controller, StringComparison.InvariantCultureIgnoreCase);
      return false;
    }

    public static string IdFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression)
    {
      ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
      string expressionText = ExpressionHelper.GetExpressionText((LambdaExpression) expression);
      string str = modelMetadata.DisplayName;
      if (str == null)
      {
        string propertyName = modelMetadata.PropertyName;
        if (propertyName == null)
          str = ((IEnumerable<string>) expressionText.Split('.')).Last<string>();
        else
          str = propertyName;
      }
      string partialFieldName = str;
      return TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(partialFieldName));
    }

    public static string DisplayNameFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression)
    {
      ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
      string expressionText = ExpressionHelper.GetExpressionText((LambdaExpression) expression);
      string str1 = modelMetadata.DisplayName;
      if (str1 == null)
      {
        string propertyName = modelMetadata.PropertyName;
        if (propertyName == null)
          str1 = ((IEnumerable<string>) expressionText.Split('.')).Last<string>();
        else
          str1 = propertyName;
      }
      string str2 = str1;
      if (string.IsNullOrEmpty(str2))
        return string.Empty;
      return str2;
    }

    public static MvcHtmlString RouteLink(
      this HtmlHelper html,
      string linkText,
      string routeName,
      RouteValueDictionary routeValues,
      string anchor)
    {
      if (string.IsNullOrEmpty(linkText))
        throw new ArgumentNullException(nameof (linkText));
      TagBuilder tagBuilder = new TagBuilder("a");
      tagBuilder.SetInnerText(linkText);
      string url = UrlHelper.GenerateUrl(routeName, (string) null, (string) null, (string) null, (string) null, (string) null, routeValues, html.RouteCollection, html.ViewContext.RequestContext, false);
      tagBuilder.Attributes.Add("href", string.Format("{0}#{1}", (object) url, (object) anchor));
      return tagBuilder.ToMvcHtmlString();
    }

    public static string LocalizedString(this HtmlHelper html, string key)
    {
      return html.LocalizedString(key, string.Empty, (object) null);
    }

    public static string LocalizedString(this HtmlHelper html, string key, string @namespace)
    {
      return html.LocalizedString(key, @namespace, (object) null);
    }

    public static string LocalizedString(
      this HtmlHelper html,
      string key,
      string @namespace,
      object values)
    {
      TextManager service = DependencyResolver.Current.GetService<TextManager>();
      if (service == null)
        return string.Empty;
      TextManager textManager = service;
      object obj = values;
      string str = @namespace;
      string key1 = key;
      object values1 = obj;
      string ns = str;
      return textManager.Get(key1, values1, ns);
    }

    public static string LocalizedString<TNamespace>(this HtmlHelper html, string key)
    {
      return html.LocalizedString<TNamespace>(key, (object) null);
    }

    public static string LocalizedString<TNamespace>(
      this HtmlHelper html,
      string key,
      object values)
    {
      TextManager service = DependencyResolver.Current.GetService<TextManager>();
      if (service == null)
        return string.Empty;
      TextManager textManager = service;
      object obj = values;
      string key1 = key;
      object values1 = obj;
      return textManager.Get<TNamespace>(key1, values1);
    }

    public static MvcHtmlString LocalizedHtmlString(this HtmlHelper html, string key)
    {
      return MvcHtmlString.Create(html.LocalizedString(key));
    }

    public static MvcHtmlString LocalizedHtmlString(
      this HtmlHelper html,
      string key,
      string @namespace)
    {
      return MvcHtmlString.Create(html.LocalizedString(key, @namespace));
    }

    public static MvcHtmlString LocalizedHtmlString(
      this HtmlHelper html,
      string key,
      string @namespace,
      object values)
    {
      return MvcHtmlString.Create(html.LocalizedString(key, @namespace, values));
    }
  }
}
