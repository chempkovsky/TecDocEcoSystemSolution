// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.ViewModels.AlgebraCaptcha
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System.ComponentModel.DataAnnotations;

namespace MVCBootstrap.Web.Mvc.ViewModels
{
  public class AlgebraCaptcha
  {
    [Required]
    public int? Result { get; set; }

    [StringLength(2147483647, MinimumLength = 1)]
    [Required]
    public string Prefix { get; set; }
  }
}
