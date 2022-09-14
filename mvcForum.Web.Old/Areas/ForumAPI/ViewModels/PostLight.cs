// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.ViewModels.PostLight
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using System;

namespace mvcForum.Web.Areas.ForumAPI.ViewModels
{
  public class PostLight
  {
    public int Id { get; set; }

    public string Subject { get; set; }

    public int Position { get; set; }

    public PostFlag Flag { get; set; }

    public DateTime Posted { get; set; }

    public string ApiUrl { get; set; }
  }
}
