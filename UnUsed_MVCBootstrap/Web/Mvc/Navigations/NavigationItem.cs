// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Navigations.NavigationItem
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace MVCBootstrap.Web.Mvc.Navigations
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
