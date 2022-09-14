// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.ViewModels.Topic
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using System;

namespace mvcForum.Web.Areas.ForumAPI.ViewModels
{
  public class Topic
  {
    public int Id { get; set; }

    public string Title { get; set; }

    public int ViewCount { get; set; }

    public int PostCount { get; set; }

    public TopicFlag Flag { get; set; }

    public TopicType Type { get; set; }

    public DateTime Posted { get; set; }

    public DateTime LastPosted { get; set; }

    public int AuthorId { get; set; }

    public string AuthorApiUrl { get; set; }

    public string Author { get; set; }

    public int? LastPostAuthorId { get; set; }

    public string LastPostAuthor { get; set; }

    public string LastPostAuthorApiUrl { get; set; }
  }
}
