// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAdmin.ViewModels.Create.ForumViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.Areas.ForumAdmin.ViewModels.Create
{
  public class ForumViewModel
  {
    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    public int SortOrder { get; set; }
  }
}
