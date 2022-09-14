// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Navigations.NavigationBase
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core.Interfaces.Services;
using mvcForum.Web.Interfaces;
using SimpleLocalisation;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Navigations
{
  public abstract class NavigationBase : INavigation
  {
    private TextManager text;

    public virtual string Area
    {
      get
      {
        return string.Empty;
      }
    }

    public virtual bool Initialized { get; set; }

    public abstract string Name { get; }

    protected abstract IEnumerable<NavigationItem> Items { get; }

    protected virtual string GetTitle(NavigationItem item)
    {
      if (this.TextManager == null)
        return item.Title;
      TextManager textManager = this.TextManager;
      string str = string.Format("Navigation.{0}", (object) this.Name);
      string key = string.Format("{2}{0}.{1}", (object) item.Controller, (object) item.Action, string.IsNullOrWhiteSpace(item.Area) ? (object) "" : (object) string.Format("{0}.", (object) item.Area));
      string ns = str;
      return textManager.Get(key, (object) null, ns);
    }

    private TextManager TextManager
    {
      get
      {
        if (this.text == null)
          this.text = DependencyResolver.Current.GetService<TextManager>();
        return this.text;
      }
    }

    public virtual void Initialize(UrlHelper url)
    {
      foreach (NavigationItem navigationItem in this.Items)
        this.InitializeItem(url, navigationItem);
      this.Initialized = true;
    }

    protected virtual void InitializeItem(UrlHelper url, NavigationItem item)
    {
      item.URL = !string.IsNullOrWhiteSpace(this.Area) || !string.IsNullOrWhiteSpace(item.Area) ? url.Action(item.Action, item.Controller, (object) new
      {
        area = (string.IsNullOrWhiteSpace(item.Area) ? this.Area : item.Area)
      }) : url.Action(item.Action, item.Controller, (object) new
      {
        area = string.Empty
      });
      item.Title = this.GetTitle(item);
      item.Selected = false;
      item.Visible = false;
      if (!item.SubPages.Any<NavigationItem>())
        return;
      foreach (NavigationItem subPage in item.SubPages)
        this.InitializeItem(url, subPage);
    }

    public virtual IEnumerable<NavigationItem> GetNavigation(
      HtmlHelper html)
    {
      bool isAuthenticated = html.ViewContext.HttpContext.User.Identity.IsAuthenticated;
      List<NavigationItem> output = new List<NavigationItem>();
      string empty1 = string.Empty;
      if (html.ViewContext.RouteData.Values.ContainsKey("controller"))
        empty1 = (string) html.ViewContext.RouteData.Values["controller"];
      string empty2 = string.Empty;
      if (html.ViewContext.RouteData.Values.ContainsKey("action"))
        empty2 = (string) html.ViewContext.RouteData.Values["action"];
      string area = string.Empty;
      if (html.ViewContext.RouteData.DataTokens.ContainsKey("area"))
        area = (string) html.ViewContext.RouteData.DataTokens["area"];
      this.HandlePageList(isAuthenticated, empty1, empty2, area, this.Items, output);
      return (IEnumerable<NavigationItem>) output;
    }

    protected virtual void HandlePageList(
      bool signedIn,
      string controller,
      string action,
      string area,
      IEnumerable<NavigationItem> pages,
      List<NavigationItem> output)
    {
      IWebUserProvider service1 = DependencyResolver.Current.GetService<IWebUserProvider>();
      IMembershipService service2 = DependencyResolver.Current.GetService<IMembershipService>();
      string accountName = string.Empty;
      if (service1.Authenticated)
        accountName = service1.ActiveUser.Name;
      foreach (NavigationItem page in pages)
      {
        bool flag = false;
        if (page.Visibility == PageVisibility.Always || page.Visibility == PageVisibility.Anonymous && !signedIn || page.Visibility == PageVisibility.Authenticated && signedIn)
        {
          if (page.Visibility == PageVisibility.Authenticated && signedIn)
          {
            if (page.Groups != null && page.Groups.Length > 0)
            {
              foreach (string group in page.Groups)
              {
                flag = service2.IsAccountInRole(accountName, group);
                if (flag)
                  break;
              }
            }
            else
              flag = true;
          }
          else
            flag = true;
        }
        if (flag)
        {
          NavigationItem navigationItem = new NavigationItem()
          {
            Title = page.Title,
            Controller = page.Controller,
            Action = page.Action,
            Area = page.Area,
            URL = page.URL,
            Selected = this.IsActive(page, controller, action, area),
            AdditionalData = page.AdditionalData
          };
          output.Add(navigationItem);
          if (page.SubPages.Any<NavigationItem>())
            this.HandlePageList(signedIn, controller, action, area, (IEnumerable<NavigationItem>) page.SubPages, navigationItem.SubPages);
        }
      }
    }

    protected virtual bool IsActive(
      NavigationItem item,
      string controller,
      string action,
      string area)
    {
      if ((string.IsNullOrWhiteSpace(item.Area) && string.IsNullOrWhiteSpace(area) || !string.IsNullOrWhiteSpace(item.Area) && item.Area.ToLower() == area.ToLower()) && item.Controller.ToLower() == controller.ToLower())
        return item.Action.ToLower() == action.ToLower();
      return false;
    }
  }
}
