// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.AuthenticateCallbackViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using SimpleAuthentication.Core;
using System;

namespace mvcForum.Web.ViewModels
{
  public class AuthenticateCallbackViewModel
  {
    public IAuthenticatedClient AuthenticatedClient { get; set; }

    public Exception Exception { get; set; }

    public string ReturnUrl { get; set; }
  }
}
