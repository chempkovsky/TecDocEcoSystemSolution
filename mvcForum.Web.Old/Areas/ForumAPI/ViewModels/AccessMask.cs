// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.ViewModels.AccessMask
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.Areas.ForumAPI.ViewModels
{
  public class AccessMask
  {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public AccessFlag AccessFlag { get; set; }

    [Required]
    public int BoardId { get; set; }

    public string ApiUrl { get; set; }
  }
}
