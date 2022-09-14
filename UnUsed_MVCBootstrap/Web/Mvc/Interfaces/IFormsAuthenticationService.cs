// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Mvc.Interfaces.IFormsAuthenticationService
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

namespace MVCBootstrap.Web.Mvc.Interfaces
{
  public interface IFormsAuthenticationService
  {
    void SignIn(string userName, bool createPersistentCookie);

    void SignOut();
  }
}
