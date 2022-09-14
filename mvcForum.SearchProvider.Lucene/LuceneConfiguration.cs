// Decompiled with JetBrains decompiler
// Type: mvcForum.SearchProvider.Lucene.LuceneConfiguration
// Assembly: mvcForum.SearchProvider.Lucene, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 13A5DCA9-908F-4689-9C6D-F584F417B850
// Assembly location: C:\Development\WebCarShop\mvcForum.SearchProvider.Lucene.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions;

namespace mvcForum.SearchProvider.Lucene
{
  public class LuceneConfiguration : AsyncAddOnConfiguration<Indexer>
  {
    public LuceneConfiguration(IRepository<AddOnConfiguration> configRepo)
      : base(configRepo)
    {
    }

    public int TitleWeight
    {
      get
      {
        return this.GetInt32(nameof (TitleWeight));
      }
      set
      {
        this.Set(nameof (TitleWeight), value);
      }
    }

    public int TopicWeight
    {
      get
      {
        return this.GetInt32(nameof (TopicWeight));
      }
      set
      {
        this.Set(nameof (TopicWeight), value);
      }
    }

    public int StickyWeight
    {
      get
      {
        return this.GetInt32(nameof (StickyWeight));
      }
      set
      {
        this.Set(nameof (StickyWeight), value);
      }
    }

    public int AnnouncementWeight
    {
      get
      {
        return this.GetInt32(nameof (AnnouncementWeight));
      }
      set
      {
        this.Set(nameof (AnnouncementWeight), value);
      }
    }

    public bool Enabled
    {
      get
      {
        return this.GetBoolean(nameof (Enabled));
      }
      set
      {
        this.Set(nameof (Enabled), value);
      }
    }

    private static class Keys
    {
      public const string Enabled = "Enabled";
      public const string TitleWeight = "TitleWeight";
      public const string TopicWeight = "TopicWeight";
      public const string StickyWeight = "StickyWeight";
      public const string AnnouncementWeight = "AnnouncementWeight";
    }
  }
}
