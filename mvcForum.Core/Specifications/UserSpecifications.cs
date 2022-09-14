// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.UserSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class UserSpecifications
  {
    public class SpecificUsernames : ISpecification<User>
    {
      private readonly string[] names;

      public SpecificUsernames(string[] names)
      {
        this.names = names;
      }

      public Expression<Func<User, bool>> IsSatisfied
      {
        get
        {
          return this.BuildOr<User, string>((Expression<Func<User, string>>) (r => r.Username), (IEnumerable<string>) this.names);
        }
      }
    }

    public class SpecificUsernameAndPassword : ISpecification<User>
    {
      private readonly string username;
      private readonly string password;

      public SpecificUsernameAndPassword(string username, string password)
      {
        this.username = username;
        this.password = password;
      }

      public Expression<Func<User, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<User, bool>>) (u => u.Username == this.username && u.Password == this.password);
        }
      }
    }

    public class SpecificEmailAddress : ISpecification<User>
    {
      private readonly string email;

      public SpecificEmailAddress(string email)
      {
        this.email = email;
      }

      public Expression<Func<User, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<User, bool>>) (u => u.EmailAddress == this.email);
        }
      }
    }

    public class SpecificUsername : ISpecification<User>
    {
      private readonly string username;

      public SpecificUsername(string username)
      {
        this.username = username;
      }

      public Expression<Func<User, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<User, bool>>) (u => u.Username == this.username);
        }
      }
    }

    public class SpecificProviderUserKey : ISpecification<User>
    {
      private readonly Guid providerUserKey;

      public SpecificProviderUserKey(Guid providerUserKey)
      {
        this.providerUserKey = providerUserKey;
      }

      public Expression<Func<User, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<User, bool>>) (u => u.Id == this.providerUserKey);
        }
      }
    }
  }
}
