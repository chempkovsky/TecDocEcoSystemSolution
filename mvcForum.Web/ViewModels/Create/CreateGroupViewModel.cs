// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.Create.CreateGroupViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels.Create
{
  public class CreateGroupViewModel
  {
    [Required]
    public string Name { get; set; }
  }
}
