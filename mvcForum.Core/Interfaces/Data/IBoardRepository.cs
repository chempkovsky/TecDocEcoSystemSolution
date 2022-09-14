// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.Data.IBoardRepository
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System.Collections.Generic;

namespace mvcForum.Core.Interfaces.Data
{
  public interface IBoardRepository : IRepository<Board>
  {
    IEnumerable<Board> ReadManyOptimized(ISpecification<Board> spec);

    Board ReadOneOptimized(ISpecification<Board> spec);
  }
}
