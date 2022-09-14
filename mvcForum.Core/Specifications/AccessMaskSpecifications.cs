// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.AccessMaskSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class AccessMaskSpecifications
  {
    public class SpecificBoard : ISpecification<AccessMask>
    {
      private readonly Board board;

      public SpecificBoard(Board board)
      {
        this.board = board;
      }

      public Expression<Func<AccessMask, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<AccessMask, bool>>) (x => x.BoardId == this.board.Id);
        }
      }
    }
  }
}
