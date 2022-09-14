// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.BasicInstallViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels
{
  public class BasicInstallViewModel
  {
    [LocalizedDisplay("mvcForum.Web.ViewModels.BasicInstallViewModel", "ImportMembershipUsers")]
    public bool ImportMembershipUsers { get; set; }

    [LocalizedDisplay("mvcForum.Web.ViewModels.BasicInstallViewModel", "ExistingUserEmail")]
    public string ExistingUserEmail { get; set; }

    [LocalizedDisplay("mvcForum.Web.ViewModels.BasicInstallViewModel", "CreateAdmin")]
    public bool CreateAdmin { get; set; }

    [LocalizedDisplay("mvcForum.Web.ViewModels.BasicInstallViewModel", "AdminUsername")]
    public string AdminUsername { get; set; }

    [LocalizedDisplay("mvcForum.Web.ViewModels.BasicInstallViewModel", "AdminEmail")]
    public string AdminEmail { get; set; }

    [LocalizedDisplay("mvcForum.Web.ViewModels.BasicInstallViewModel", "AdminPassword")]
    public string AdminPassword { get; set; }

    [LocalizedDisplay("mvcForum.Web.ViewModels.BasicInstallViewModel", "CreateGroups")]
    public bool CreateGroups { get; set; }

    [LocalizedDisplay("mvcForum.Web.ViewModels.BasicInstallViewModel", "CreateSimpleForum")]
    public bool CreateSimpleForum { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.Web.ViewModels.BasicInstallViewModel", "SiteURL")]
    public string SiteURL { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.Web.ViewModels.BasicInstallViewModel", "SubmitInstallation")]
    public bool SubmitInstallation { get; set; }

    public bool CanCreateAdmin { get; set; }

    public bool CanCreateGroups { get; set; }

    public bool CanCreateSimpleForum { get; set; }
  }
}
