// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Extensions.TagBuilderExtensions
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Extensions
{
  public static class TagBuilderExtensions
  {
    public static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder)
    {
      return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
    }
  }
}
