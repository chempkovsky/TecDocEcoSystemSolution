// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Navigations.NavigationItem
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace mvcForum.Web.Navigations
{
  public class NavigationItem
  {
    private bool selected;

    public NavigationItem()
    {
      this.Groups = new string[0];
      this.SubPages = new List<NavigationItem>();
    }

    public string Action { get; set; }

    public string Controller { get; set; }

    public string Area { get; set; }

    public List<NavigationItem> SubPages { get; set; }

    public string Title { get; set; }

    public string URL { get; set; }

    public bool Visible { get; set; }

    public PageVisibility Visibility { get; set; }

    public string[] Groups { get; set; }

    public object AdditionalData { get; set; }

    public bool Selected
    {
      get
      {
        if (this.selected)
          return true;
        if (this.SubPages != null)
          return this.SubPages.Any<NavigationItem>((Func<NavigationItem, bool>) (p => p.Selected));
        return false;
      }
      set
      {
        this.selected = value;
      }
    }
  }
}
