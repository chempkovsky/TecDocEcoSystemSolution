// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Extensions.Mvc3.LabelExtensions
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web.Extensions.Mvc3
{
  public static class LabelExtensions
  {
    public static MvcHtmlString LabelFor<TModel, TValue>(
      this HtmlHelper<TModel> html,
      Expression<Func<TModel, TValue>> expression,
      object htmlAttributes)
    {
      ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
      string expressionText = ExpressionHelper.GetExpressionText((LambdaExpression) expression);
      string str = modelMetadata.PropertyName;
      if (str == null)
        str = ((IEnumerable<string>) expressionText.Split('.')).Last<string>();
      string originalId = str;
      string innerText = modelMetadata.DisplayName ?? originalId;
      if (string.IsNullOrEmpty(originalId))
        return MvcHtmlString.Empty;
      TagBuilder tagBuilder = new TagBuilder("label");
      tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(originalId));
      tagBuilder.SetInnerText(innerText);
      RouteValueDictionary htmlAttributes1 = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
      tagBuilder.MergeAttributes<string, object>((IDictionary<string, object>) htmlAttributes1);
      return tagBuilder.ToMvcHtmlString();
    }
  }
}
