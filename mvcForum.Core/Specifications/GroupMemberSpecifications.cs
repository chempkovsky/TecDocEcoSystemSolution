// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.GroupMemberSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class GroupMemberSpecifications
  {
    public class SpecificUser : ISpecification<GroupMember>
    {
      private readonly ForumUser user;

      public SpecificUser(ForumUser user)
      {
        this.user = user;
      }

      public Expression<Func<GroupMember, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<GroupMember, bool>>) (x => x.ForumUserId == this.user.Id);
        }
      }
    }

    public class SpecificGroup : ISpecification<GroupMember>
    {
      private readonly Group group;

      public SpecificGroup(Group group)
      {
        this.group = group;
      }

      public Expression<Func<GroupMember, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<GroupMember, bool>>) (x => x.GroupId == this.group.Id);
        }
      }
    }

    public class SpecificUserAndGroup : ISpecification<GroupMember>
    {
      private readonly ForumUser user;
      private readonly Group group;

      public SpecificUserAndGroup(ForumUser user, Group group)
      {
        this.user = user;
        this.group = group;
      }

      public Expression<Func<GroupMember, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<GroupMember, bool>>) (x => x.ForumUserId == this.user.Id && x.GroupId == this.group.Id);
        }
      }
    }
  }
}
