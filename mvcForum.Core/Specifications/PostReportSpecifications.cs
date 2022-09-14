// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.PostReportSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class PostReportSpecifications
  {
    public class SpecificPost : ISpecification<PostReport>
    {
      private readonly Post post;

      public SpecificPost(Post post)
      {
        this.post = post;
      }

      public Expression<Func<PostReport, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<PostReport, bool>>) (x => x.PostId == this.post.Id);
        }
      }
    }

    public class NotResolved : ISpecification<PostReport>
    {
      public Expression<Func<PostReport, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<PostReport, bool>>) (x => !x.Resolved);
        }
      }
    }
  }
}
