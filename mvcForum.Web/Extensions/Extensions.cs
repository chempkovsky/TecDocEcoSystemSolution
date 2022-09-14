// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Extensions.Extensions
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using MVCThemes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Extensions
{
  public static class Extensions
  {
    public static IEnumerable<Post> Visible(
      this IEnumerable<Post> posts,
      IConfiguration config)
    {
      if (config.ShowDeletedMessages)
        return posts.Where<Post>((Func<Post, bool>) (p => p.FlagValue != 2));
      return posts.Where<Post>((Func<Post, bool>) (p => p.FlagValue == 0));
    }

    public static string ForumImage(this UrlHelper url, string path)
    {
      string str = "/content/img/forum/";
      string theme = DependencyResolver.Current.GetService<IThemeProvider>().GetTheme();
      if (!string.IsNullOrWhiteSpace(theme))
      {
        string themeBaseUrl = DependencyResolver.Current.GetService<IThemeURLProvider>().GetThemeBaseURL(theme);
        if (!string.IsNullOrWhiteSpace(themeBaseUrl))
          str = string.Format("{0}gfx/", (object) themeBaseUrl);
      }
      return url.Content(string.Format("{0}{1}", (object) str, (object) path));
    }

    public static void SetSticky(this Topic topic)
    {
      if ((topic.Forum.GetAccess() & AccessFlag.Priority) != AccessFlag.Priority)
        return;
      topic.Type = TopicType.Sticky;
    }

    public static void SetAnnouncement(this Topic topic)
    {
      if ((topic.Forum.GetAccess() & AccessFlag.Priority) != AccessFlag.Priority)
        return;
      topic.Type = TopicType.Announcement;
    }

    public static void SetLocked(this Topic topic)
    {
      if ((topic.Forum.GetAccess() & AccessFlag.Moderator) != AccessFlag.Moderator)
        return;
      topic.SetFlag(TopicFlag.Locked);
    }
  }
}
