// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.BoardSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class BoardSpecifications
  {
    public class ById : ISpecification<Board>
    {
      private int id;

      public ById(int id)
      {
        this.id = id;
      }

      public Expression<Func<Board, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Board, bool>>) (b => b.Id == this.id);
        }
      }
    }

    public class Enabled : ISpecification<Board>
    {
      public Expression<Func<Board, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Board, bool>>) (b => b.Disabled == false);
        }
      }
    }
  }
}
