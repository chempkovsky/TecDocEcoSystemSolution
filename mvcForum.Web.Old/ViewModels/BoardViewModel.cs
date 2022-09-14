// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.BoardViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels
{
  public class BoardViewModel : ForumViewModelBase
  {
    public BoardViewModel()
    {
    }

    public BoardViewModel(Board board)
    {
      this.Id = board.Id;
      this.Name = board.Name;
      this.Description = board.Description;
    }

    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    public ReadOnlyCollection<AccessMaskViewModel> AccessMasks { get; set; }

    public ReadOnlyCollection<CategoryViewModel> Categories { get; set; }

    public bool ShowOnline { get; set; }

    public IEnumerable<ForumUser> OnlineUsers { get; set; }
  }
}
