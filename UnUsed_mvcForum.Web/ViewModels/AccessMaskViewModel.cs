// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.AccessMaskViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels
{
  public class AccessMaskViewModel
  {
    public AccessMaskViewModel()
    {
    }

    public AccessMaskViewModel(AccessMask mask)
    {
      this.Flag = mask.AccessFlag;
      this.Name = mask.Name;
      this.Id = mask.Id;
    }

    public int Id { get; set; }

    public AccessFlag Flag { get; set; }

    [Required]
    public string Name { get; set; }
  }
}
