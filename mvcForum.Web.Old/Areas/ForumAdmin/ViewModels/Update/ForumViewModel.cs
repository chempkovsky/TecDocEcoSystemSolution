// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAdmin.ViewModels.Update.ForumViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Web.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.Areas.ForumAdmin.ViewModels.Update
{
  public class ForumViewModel
  {
    public ForumViewModel()
    {
    }

    public ForumViewModel(mvcForum.Core.Forum forum)
    {
      this.Id = forum.Id;
      this.Name = forum.Name;
      this.Description = forum.Description;
      this.SortOrder = forum.SortOrder;
    }

    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    public int SortOrder { get; set; }

    public Dictionary<string, string> Path { get; set; }

    public List<GroupViewModel> Groups { get; set; }

    public List<ForumViewModel> SubForums { get; set; }

    public List<ForumAccessViewModel> ForumAccesses { get; set; }
  }
}
