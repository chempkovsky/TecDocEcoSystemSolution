// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Extensions.UrlHelperExtensions
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCThemes.Interfaces;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web.Extensions
{
  public static class UrlHelperExtensions
  {
    private static string GetTheme()
    {
      IThemeProvider service = DependencyResolver.Current.GetService<IThemeProvider>();
      if (service != null)
        return service.GetTheme();
      return string.Empty;
    }

    public static string GetThemeBaseUrl(this UrlHelper url)
    {
      string theme = UrlHelperExtensions.GetTheme();
      if (!string.IsNullOrWhiteSpace(theme))
        return string.Format("~/themes/{0}/", (object) theme);
      return "~/";
    }

    public static string Forum(this UrlHelper url, int forumId, string forumName)
    {
      return UrlHelper.GenerateUrl("ShowForum", (string) null, (string) null, (string) null, (string) null, (string) null, new RouteValueDictionary()
      {
        {
          "id",
          (object) forumId
        },
        {
          "title",
          (object) forumName.ToSlug()
        }
      }, url.RouteCollection, url.RequestContext, false);
    }
  }
}
