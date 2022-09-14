// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.ForumSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class ForumSpecifications
  {
    public class SpecificCategoryNoParentForum : ISpecification<Forum>
    {
      private readonly Category category;

      public SpecificCategoryNoParentForum(Category category)
      {
        this.category = category;
      }

      public Expression<Func<Forum, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Forum, bool>>) (f => f.CategoryId == this.category.Id && f.ParentForum == (object) null);
        }
      }
    }

    public class ById : ISpecification<Forum>
    {
      private readonly int id;

      public ById(int id)
      {
        this.id = id;
      }

      public Expression<Func<Forum, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Forum, bool>>) (f => f.Id == this.id);
        }
      }
    }
  }
}
