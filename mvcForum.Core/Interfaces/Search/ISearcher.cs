// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.Search.ISearcher
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using mvcForum.Core.Search;
using System.Collections.Generic;

namespace mvcForum.Core.Interfaces.Search
{
  public interface ISearcher
  {
    IEnumerable<SearchResult> Search(string query, IList<int> forums);
  }
}
