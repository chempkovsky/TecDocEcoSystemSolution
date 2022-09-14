// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.RoleSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class RoleSpecifications
  {
    public class SpecificNames : ISpecification<Role>
    {
      private readonly string[] names;

      public SpecificNames(string[] names)
      {
        this.names = names;
      }

      public Expression<Func<Role, bool>> IsSatisfied
      {
        get
        {
          return this.BuildOr<Role, string>((Expression<Func<Role, string>>) (r => r.Name), (IEnumerable<string>) this.names);
        }
      }
    }

    public class SpecificName : ISpecification<Role>
    {
      private readonly string name;

      public SpecificName(string name)
      {
        this.name = name;
      }

      public Expression<Func<Role, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Role, bool>>) (r => r.Name == this.name);
        }
      }
    }
  }
}
