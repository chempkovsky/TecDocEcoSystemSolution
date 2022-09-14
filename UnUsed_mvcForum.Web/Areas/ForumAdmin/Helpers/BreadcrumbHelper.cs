// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAdmin.Helpers.BreadcrumbHelper
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Web.Areas.ForumAdmin.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAdmin.Helpers
{
  public static class BreadcrumbHelper
  {
    public static List<BreadcrumbNode> BuildPath(mvcForum.Core.Group group, UrlHelper url)
    {
      return new List<BreadcrumbNode>()
      {
        new BreadcrumbNode()
        {
          Title = "Groups",
          Action = url.Action("index", nameof (group), (object) new
          {
            area = "forumadmin"
          })
        },
        new BreadcrumbNode()
        {
          Title = group.Name,
          Action = url.Action("update", nameof (group), (object) new
          {
            id = group.Id,
            area = "forumadmin"
          })
        }
      };
    }

    public static List<BreadcrumbNode> BuildPath(mvcForum.Core.ForumUser user, UrlHelper url)
    {
      return new List<BreadcrumbNode>()
      {
        new BreadcrumbNode()
        {
          Title = "Users",
          Action = url.Action("index", nameof (user), (object) new
          {
            area = "forumadmin"
          })
        },
        new BreadcrumbNode()
        {
          Title = user.Name,
          Action = url.Action("update", nameof (user), (object) new
          {
            id = user.Id,
            area = "forumadmin"
          })
        }
      };
    }

    public static List<BreadcrumbNode> BuildPath(mvcForum.Core.Board board, UrlHelper url)
    {
      return new List<BreadcrumbNode>()
      {
        new BreadcrumbNode()
        {
          Title = "Home",
          Action = url.Action("index", "home", (object) new
          {
            area = "forumadmin"
          })
        },
        new BreadcrumbNode()
        {
          Title = board.Name,
          Action = url.Action("update", nameof (board), (object) new
          {
            id = board.Id,
            area = "forumadmin"
          })
        }
      };
    }

    public static List<BreadcrumbNode> BuildPath(mvcForum.Core.AccessMask mask, UrlHelper url)
    {
      List<BreadcrumbNode> breadcrumbNodeList = BreadcrumbHelper.BuildPath(mask.Board, url);
      breadcrumbNodeList.Add(new BreadcrumbNode()
      {
        Title = mask.Name,
        Action = url.Action("update", "accessmask", (object) new
        {
          id = mask.Id,
          area = "forumadmin"
        })
      });
      return breadcrumbNodeList;
    }

    public static List<BreadcrumbNode> BuildPath(mvcForum.Core.Category category, UrlHelper url)
    {
      List<BreadcrumbNode> breadcrumbNodeList = BreadcrumbHelper.BuildPath(category.Board, url);
      breadcrumbNodeList.Add(new BreadcrumbNode()
      {
        Title = category.Name,
        Action = url.Action("update", nameof (category), (object) new
        {
          id = category.Id
        })
      });
      return breadcrumbNodeList;
    }

    public static List<BreadcrumbNode> BuildPath(mvcForum.Core.Forum forum, UrlHelper url)
    {
      List<BreadcrumbNode> breadcrumbNodeList = forum.ParentForum == null ? BreadcrumbHelper.BuildPath(forum.Category, url) : BreadcrumbHelper.BuildPath(forum.ParentForum, url);
      breadcrumbNodeList.Add(new BreadcrumbNode()
      {
        Title = forum.Name,
        Action = url.Action("update", nameof (forum), (object) new
        {
          id = forum.Id
        })
      });
      return breadcrumbNodeList;
    }
  }
}
