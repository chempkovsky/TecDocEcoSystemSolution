// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Extensions.TagBuilderExtensions
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System.Web.Mvc;

namespace mvcForum.Web.Extensions
{
  public static class TagBuilderExtensions
  {
    public static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder)
    {
      return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
    }
  }
}
