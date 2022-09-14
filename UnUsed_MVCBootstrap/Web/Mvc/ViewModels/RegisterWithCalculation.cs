// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.ViewModels.RegisterWithCalculation
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MVCBootstrap.Web.Mvc.ViewModels
{
  public class RegisterWithCalculation
  {
    [LocalizedDisplay(typeof (RegisterWithCalculation), "Username")]
    [Required]
    public string Username { get; set; }

    [LocalizedDisplay(typeof (RegisterWithCalculation), "EmailAddress")]
    [Required]
    [DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [LocalizedDisplay(typeof (RegisterWithCalculation), "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [LocalizedCompare("Password", typeof (RegisterWithCalculation), "NotMatchingPasswords")]
    [Required]
    [LocalizedDisplay(typeof (RegisterWithCalculation), "RepeatPassword")]
    public string RepeatPassword { get; set; }

    [LocalizedDisplay(typeof (RegisterWithCalculation), "Result")]
    [Required]
    public AlgebraCaptcha Result { get; set; }
  }
}
