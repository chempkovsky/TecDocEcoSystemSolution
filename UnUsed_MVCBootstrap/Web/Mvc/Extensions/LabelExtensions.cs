// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Extensions.LabelExtensions
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCBootstrap.Web.Mvc.Extensions
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
