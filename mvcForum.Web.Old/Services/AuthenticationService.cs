// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Services.AuthenticationService
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core.Interfaces.Services;
using System;
using System.Web.Security;

namespace mvcForum.Web.Services
{
  public class AuthenticationService : IAuthenticationService
  {
    public void SignIn(IAccount account, bool createPersistentCookie)
    {
      if (account == null)
        throw new ArgumentNullException(nameof (account));
      FormsAuthentication.SetAuthCookie(account.AccountName, createPersistentCookie);
    }

    public void SignOut()
    {
      FormsAuthentication.SignOut();
    }
  }
}
