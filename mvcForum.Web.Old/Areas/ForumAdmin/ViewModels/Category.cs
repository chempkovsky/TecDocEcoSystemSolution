// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAdmin.ViewModels.Category
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.Collections.Generic;

namespace mvcForum.Web.Areas.ForumAdmin.ViewModels
{
  public class Category
  {
    public int Id { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.Category", "Name")]
    public string Name { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.Category", "SortOrder")]
    public int SortOrder { get; set; }

    public List<BreadcrumbNode> Path { get; set; }
  }
}
