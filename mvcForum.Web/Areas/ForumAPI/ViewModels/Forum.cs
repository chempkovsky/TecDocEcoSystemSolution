// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.ViewModels.Forum
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.Areas.ForumAPI.ViewModels
{
  public class Forum
  {
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; }

    [Required]
    [Range(0, 2147483647)]
    public int SortOrder { get; set; }

    public string Description { get; set; }

    public int? ParentForumId { get; set; }

    public string ParentForumUrl { get; set; }

    public int CategoryId { get; set; }

    public string CategoryUrl { get; set; }
  }
}
