// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Extensions.RouteCollectionExtensions
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System.Web.Routing;

namespace MVCBootstrap.Web.Mvc.Extensions
{
  public static class RouteCollectionExtensions
  {
    public static void CreateArea(
      this RouteCollection routes,
      string areaName,
      string controllersNamespace,
      params Route[] routeEntries)
    {
      foreach (Route routeEntry in routeEntries)
      {
        if (routeEntry.Constraints == null)
          routeEntry.Constraints = new RouteValueDictionary();
        if (routeEntry.Defaults == null)
          routeEntry.Defaults = new RouteValueDictionary();
        if (routeEntry.DataTokens == null)
          routeEntry.DataTokens = new RouteValueDictionary();
        routeEntry.Constraints["area"] = (object) areaName;
        routeEntry.Defaults["area"] = (object) areaName;
        routeEntry.DataTokens["namespaces"] = (object) new string[1]
        {
          controllersNamespace
        };
        if (!routes.Contains((RouteBase) routeEntry))
          routes.Add((RouteBase) routeEntry);
      }
    }
  }
}
