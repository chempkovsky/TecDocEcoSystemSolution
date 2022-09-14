// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Services.FormsAuthenticationService
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using MVCBootstrap.Web.Mvc.Interfaces;
using System;
using System.Web.Security;

namespace MVCBootstrap.Web.Mvc.Services
{
  public class FormsAuthenticationService : IFormsAuthenticationService
  {
    public void SignIn(string username, bool createPersistentCookie)
    {
      if (string.IsNullOrEmpty(username))
        throw new ArgumentException("Value cannot be null or empty.", "userName");
      FormsAuthentication.SetAuthCookie(username, createPersistentCookie);
    }

    public void SignOut()
    {
      FormsAuthentication.SignOut();
    }
  }
}
