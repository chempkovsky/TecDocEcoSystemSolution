// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.ForumAccessSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class ForumAccessSpecifications
  {
    public class SpecificForumAndGroup : ISpecification<ForumAccess>
    {
      private readonly Forum forum;
      private readonly Group group;

      public SpecificForumAndGroup(Forum forum, Group group)
      {
        this.forum = forum;
        this.group = group;
      }

      public Expression<Func<ForumAccess, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<ForumAccess, bool>>) (x => x.GroupId == this.group.Id && x.ForumId == this.forum.Id);
        }
      }
    }

    public class SpecificForum : ISpecification<ForumAccess>
    {
      private readonly Forum forum;

      public SpecificForum(Forum forum)
      {
        this.forum = forum;
      }

      public Expression<Func<ForumAccess, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<ForumAccess, bool>>) (x => x.Forum.Id == this.forum.Id);
        }
      }
    }
  }
}
