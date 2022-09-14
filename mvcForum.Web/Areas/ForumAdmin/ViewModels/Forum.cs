// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAdmin.ViewModels.Forum
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.Collections.Generic;

namespace mvcForum.Web.Areas.ForumAdmin.ViewModels
{
  public class Forum
  {
    public int Id { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.Forum", "Name")]
    public string Name { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.Forum", "SortOrder")]
    public int SortOrder { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.Forum", "Description")]
    public string Description { get; set; }

    public int BoardId { get; set; }

    public int CategoryId { get; set; }

    public List<BreadcrumbNode> Path { get; set; }
  }
}
