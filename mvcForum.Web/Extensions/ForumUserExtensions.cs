// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Extensions.ForumUserExtensions
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Extensions
{
  public static class ForumUserExtensions
  {
    public static IEnumerable<Group> Groups(this ForumUser user)
    {
      DependencyResolver.Current.GetService<IRepository<Group>>();
      return DependencyResolver.Current.GetService<IRepository<GroupMember>>().ReadMany((ISpecification<GroupMember>) new GroupMemberSpecifications.SpecificUser(user)).Select<GroupMember, Group>((Func<GroupMember, Group>) (x => x.Group)).Distinct<Group>();
    }
  }
}
