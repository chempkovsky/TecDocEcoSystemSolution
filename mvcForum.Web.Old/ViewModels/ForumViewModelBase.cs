// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.ForumViewModelBase
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using mvcForum.Web.Extensions;
using mvcForum.Web.Interfaces;
using System.Collections.Generic;
using System.Web.Mvc;

namespace mvcForum.Web.ViewModels
{
  public abstract class ForumViewModelBase
  {
    private bool authenChecked;
    private bool authenticated;
    private ForumUser currentUser;

    public Dictionary<string, string> Path { get; set; }

    public bool Authenticated
    {
      get
      {
        if (!this.authenChecked)
        {
          this.authenticated = DependencyResolver.Current.GetService<IWebUserProvider>().Authenticated;
          this.authenChecked = true;
        }
        return this.authenticated;
      }
    }

    protected bool HasAccess(Forum forum)
    {
      return (forum.GetAccess() & AccessFlag.Read) == AccessFlag.Read;
    }

    protected bool HasAccess(Forum forum, AccessFlag flag)
    {
      return (forum.GetAccess() & flag) == flag;
    }

    protected ForumUser CurrentUser
    {
      get
      {
        if (this.Authenticated)
        {
          if (this.currentUser == null)
          {
            try
            {
              this.currentUser = DependencyResolver.Current.GetService<IWebUserProvider>().ActiveUser;
            }
            catch
            {
            }
          }
        }
        return this.currentUser;
      }
    }
  }
}
