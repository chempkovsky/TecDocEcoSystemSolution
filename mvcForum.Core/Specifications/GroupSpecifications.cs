// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.GroupSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class GroupSpecifications
  {
    public class SpecificName : ISpecification<Group>
    {
      private readonly string name;

      public SpecificName(string name)
      {
        this.name = name;
      }

      public Expression<Func<Group, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Group, bool>>) (x => x.Name == this.name);
        }
      }
    }
  }
}
