// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.MultiPost.MultiPostConfiguration
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions;

namespace mvcForum.AddOns.MultiPost
{
  public class MultiPostConfiguration : AsyncAddOnConfiguration<MultiPostAddOn>
  {
    public MultiPostConfiguration(IRepository<AddOnConfiguration> configRepo)
      : base(configRepo)
    {
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

    public int Interval
    {
      get
      {
        return this.GetInt32(nameof (Interval));
      }
      set
      {
        this.Set(nameof (Interval), value);
      }
    }

    public int Posts
    {
      get
      {
        return this.GetInt32(nameof (Posts));
      }
      set
      {
        this.Set(nameof (Posts), value);
      }
    }

    private static class Keys
    {
      public const string Enabled = "Enabled";
      public const string Interval = "Interval";
      public const string Posts = "Posts";
    }
  }
}
