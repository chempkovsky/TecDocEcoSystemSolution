// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.LogOnModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels
{
  public class LogOnModel
  {
    [DataType(DataType.EmailAddress)]
    [Required]
    [LocalizedDisplay("MVCForum.Web.ViewModels.LogOnModel", "EmailAddress")]
    public string EmailAddress { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.LogOnModel", "Password")]
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }

    public bool RememberMe { get; set; }

    public bool AllowLocalUsers { get; set; }

    public bool AllowSignUp { get; set; }

    public bool AllowOpenAuthUsers { get; set; }
  }
}
