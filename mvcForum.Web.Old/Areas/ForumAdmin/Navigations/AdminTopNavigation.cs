// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAdmin.Navigations.AdminTopNavigation
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Web.Navigations;
using System.Collections.Generic;

namespace mvcForum.Web.Areas.ForumAdmin.Navigations
{
  public class AdminTopNavigation : NavigationBase
  {
    private List<NavigationItem> pages = new List<NavigationItem>()
    {
      new NavigationItem()
      {
        Controller = "Home",
        Action = "Index",
        Area = "forumadmin",
        Visible = true,
        Visibility = PageVisibility.Authenticated,
        Groups = new string[2]
        {
          "Solution Administrator",
          "Board Administrator"
        }
      },
      new NavigationItem()
      {
        Controller = "Group",
        Action = "Index",
        Area = "forumadmin",
        Visible = true,
        Visibility = PageVisibility.Authenticated,
        Groups = new string[1]{ "Solution Administrator" }
      },
      new NavigationItem()
      {
        Controller = "User",
        Action = "Index",
        Area = "forumadmin",
        Visible = true,
        Visibility = PageVisibility.Authenticated,
        Groups = new string[1]{ "Solution Administrator" }
      },
      new NavigationItem()
      {
        Controller = "Settings",
        Action = "Index",
        Area = "forumadmin",
        Visible = true,
        Visibility = PageVisibility.Authenticated,
        Groups = new string[1]{ "Solution Administrator" }
      },
      new NavigationItem()
      {
        Controller = "Search",
        Action = "Index",
        Area = "forumadmin",
        Visible = true,
        Visibility = PageVisibility.Authenticated,
        Groups = new string[1]{ "Solution Administrator" }
      },
      new NavigationItem()
      {
        Controller = "AntiSpam",
        Action = "Index",
        Area = "forumadmin",
        Visible = true,
        Visibility = PageVisibility.Authenticated,
        Groups = new string[1]{ "Solution Administrator" }
      }
    };

    protected override IEnumerable<NavigationItem> Items
    {
      get
      {
        return (IEnumerable<NavigationItem>) this.pages;
      }
    }

    public override string Name
    {
      get
      {
        return nameof (AdminTopNavigation);
      }
    }
  }
}
