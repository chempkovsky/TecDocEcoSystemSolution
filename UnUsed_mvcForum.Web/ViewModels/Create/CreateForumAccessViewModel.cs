// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.Create.CreateForumAccessViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels.Create
{
  public class CreateForumAccessViewModel
  {
    public CreateForumAccessViewModel()
    {
    }

    public CreateForumAccessViewModel(Forum forum)
    {
      this.Name = forum.Name;
      this.Id = forum.Id;
    }

    public Dictionary<string, string> Path { get; set; }

    [Required]
    public int Id { get; set; }

    public string Name { get; set; }

    [Required]
    public int AccessMaskId { get; set; }

    [Required]
    public int GroupId { get; set; }
  }
}
