// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.AttachmentSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class AttachmentSpecifications
  {
    public class ByAuthor : ISpecification<Attachment>
    {
      private readonly ForumUser user;

      public ByAuthor(ForumUser user)
      {
        this.user = user;
      }

      public Expression<Func<Attachment, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Attachment, bool>>) (a => a.AuthorId == this.user.Id);
        }
      }
    }
  }
}
