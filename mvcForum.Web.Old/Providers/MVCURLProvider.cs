// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Providers.MVCURLProvider
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Web.Interfaces;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web.Providers
{
  public class MVCURLProvider : IURLProvider
  {
    public string RouteUrl(string routeName, object routeValues)
    {
      return new UrlHelper(new RequestContext()).RouteUrl(routeName, routeValues);
    }
  }
}
