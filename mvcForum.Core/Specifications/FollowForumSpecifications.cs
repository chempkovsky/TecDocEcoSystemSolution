// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.FollowForumSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class FollowForumSpecifications
  {
    public class SpecificForum : ISpecification<FollowForum>
    {
      private readonly Forum forum;

      public SpecificForum(Forum forum)
      {
        this.forum = forum;
      }

      public Expression<Func<FollowForum, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<FollowForum, bool>>) (x => x.ForumId == this.forum.Id);
        }
      }
    }

    public class SpecificForumAndUser : ISpecification<FollowForum>
    {
      private readonly Forum forum;
      private readonly ForumUser user;

      public SpecificForumAndUser(Forum forum, ForumUser user)
      {
        this.forum = forum;
        this.user = user;
      }

      public Expression<Func<FollowForum, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<FollowForum, bool>>) (x => x.ForumId == this.forum.Id && x.ForumUserId == this.user.Id);
        }
      }
    }
  }
}
