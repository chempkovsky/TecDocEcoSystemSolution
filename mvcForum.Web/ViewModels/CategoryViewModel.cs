// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.CategoryViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels
{
  public class CategoryViewModel : ForumViewModelBase
  {
    public CategoryViewModel()
    {
    }

    public CategoryViewModel(Category category)
    {
      this.Id = category.Id;
      this.Name = category.Name;
      this.SortOrder = category.SortOrder;
    }

    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int SortOrder { get; set; }

    public IEnumerable<ForumViewModel> Forums { get; set; }
  }
}
