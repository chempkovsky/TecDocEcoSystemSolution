// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Navigations.INavigation
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System.Collections.Generic;
using System.Web.Mvc;

namespace mvcForum.Web.Navigations
{
  public interface INavigation
  {
    IEnumerable<NavigationItem> GetNavigation(HtmlHelper html);

    bool Initialized { get; }

    string Name { get; }

    void Initialize(UrlHelper url);
  }
}
