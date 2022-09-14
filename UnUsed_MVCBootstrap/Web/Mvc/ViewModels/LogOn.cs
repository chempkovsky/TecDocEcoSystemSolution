// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.ViewModels.LogOn
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVCBootstrap.Web.Mvc.ViewModels
{
  public class LogOn
  {
    [Required]
    [DataType(DataType.EmailAddress)]
    [DisplayName("E-mail address")]
    public string EmailAddress { get; set; }

    [DataType(DataType.Password)]
    [DisplayName("Password")]
    [Required]
    public string Password { get; set; }

    [DisplayName("Remember me")]
    public bool RememberMe { get; set; }
  }
}
