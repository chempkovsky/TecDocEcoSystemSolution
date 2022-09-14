// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.ForumUserSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class ForumUserSpecifications
  {
    public class SpecificProviderUserKey : ISpecification<ForumUser>
    {
      private readonly string providerUserKey;

      public SpecificProviderUserKey(string providerUserKey)
      {
        this.providerUserKey = providerUserKey;
      }

      public Expression<Func<ForumUser, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<ForumUser, bool>>) (x => x.ProviderId == this.providerUserKey);
        }
      }
    }

    public class SpecificUsername : ISpecification<ForumUser>
    {
      private readonly string username;

      public SpecificUsername(string username)
      {
        this.username = username;
      }

      public Expression<Func<ForumUser, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<ForumUser, bool>>) (x => x.Name == this.username);
        }
      }
    }

    public class SpecificEmailAddress : ISpecification<ForumUser>
    {
      private readonly string email;

      public SpecificEmailAddress(string email)
      {
        this.email = email;
      }

      public Expression<Func<ForumUser, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<ForumUser, bool>>) (x => x.EmailAddress == this.email);
        }
      }
    }

    public class ExistingForumUser : ISpecification<ForumUser>
    {
      private readonly string username;
      private readonly string email;

      public ExistingForumUser(string username, string email)
      {
        this.email = email;
        this.username = username;
      }

      public Expression<Func<ForumUser, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<ForumUser, bool>>) (x => x.EmailAddress == this.email || x.Name == this.username);
        }
      }
    }

    public class NotDeleted : ISpecification<ForumUser>
    {
      public Expression<Func<ForumUser, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<ForumUser, bool>>) (u => u.Deleted == false);
        }
      }
    }

    public class Search : ISpecification<ForumUser>
    {
      private readonly string query;

      public Search(string query)
      {
        this.query = query;
      }

      public Expression<Func<ForumUser, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<ForumUser, bool>>) (u => u.EmailAddress.Contains(this.query) || u.FullName.Contains(this.query) || u.Name.Contains(this.query));
        }
      }
    }
  }
}
