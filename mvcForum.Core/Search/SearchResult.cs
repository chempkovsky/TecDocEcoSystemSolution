// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Search.SearchResult
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

namespace mvcForum.Core.Search
{
  public class SearchResult
  {
    public int PostId { get; set; }

    public int TopicId { get; set; }

    public string Title { get; set; }

    public float Score { get; set; }
  }
}
