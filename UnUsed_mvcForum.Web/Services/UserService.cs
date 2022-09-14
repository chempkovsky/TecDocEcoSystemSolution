// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Services.UserService
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Web.Interfaces;
using mvcForum.Web.Providers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Security;

namespace mvcForum.Web.Services
{
  public class UserService : IUserService
  {
    private readonly IContext context;

    public UserService(IContext context)
    {
      this.context = context;
    }

    public IEnumerable<ForumUser> GetOnlineUsers()
    {
      if (Membership.Provider is MembershipProviderWrapper)
      {
        MembershipProviderWrapper provider = (MembershipProviderWrapper) Membership.Provider;
        if (provider.ActualProvider is MVCBootstrap.Web.Security.SimpleMembershipProvider)
        {
          DateTime last15Mins = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes((double) ((MVCBootstrap.Web.Security.SimpleMembershipProvider) provider.ActualProvider).UserIsOnlineTimeWindow));
          return this.context.GetRepository<ForumUser>().ReadMany((Expression<Func<ForumUser, bool>>) (u => u.LastVisit > last15Mins));
        }
      }
      return (IEnumerable<ForumUser>) new List<ForumUser>();
    }
  }
}
