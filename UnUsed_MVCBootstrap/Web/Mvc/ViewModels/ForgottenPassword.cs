// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.ViewModels.ForgottenPassword
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System.ComponentModel;

namespace MVCBootstrap.Web.Mvc.ViewModels
{
  public class ForgottenPassword
  {
    [DisplayName("E-mail address")]
    public string EmailAddress { get; set; }

    [DisplayName("Username")]
    public string Username { get; set; }
  }
}
