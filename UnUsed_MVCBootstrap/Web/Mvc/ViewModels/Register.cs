// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.ViewModels.Register
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MVCBootstrap.Web.Mvc.ViewModels
{
  public class Register
  {
    [Required]
    [LocalizedDisplay(typeof (Register), "Username")]
    public string Username { get; set; }

    [LocalizedDisplay(typeof (Register), "EmailAddress")]
    [DataType(DataType.EmailAddress)]
    [Required]
    public string EmailAddress { get; set; }

    [DataType(DataType.Password)]
    [LocalizedDisplay(typeof (Register), "Password")]
    [Required]
    public string Password { get; set; }

    [Required]
    [LocalizedDisplay(typeof (Register), "RepeatPassword")]
    [DataType(DataType.Password)]
    [LocalizedCompare("Password", typeof (Register), "NotMatchingPasswords")]
    public string RepeatPassword { get; set; }
  }
}
