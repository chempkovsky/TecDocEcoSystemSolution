// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.StopForumSpam.StopForumSpamConfiguration
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions;

namespace mvcForum.AddOns.StopForumSpam
{
  public class StopForumSpamConfiguration : AsyncAddOnConfiguration<StopForumSpamAddOn>
  {
    public StopForumSpamConfiguration(IRepository<AddOnConfiguration> configRepo)
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

    public string Key
    {
      get
      {
        return this.GetString("APIKey");
      }
      set
      {
        this.Set("APIKey", value);
      }
    }

    public bool CheckNewUsers
    {
      get
      {
        return this.GetBoolean(nameof (CheckNewUsers));
      }
      set
      {
        this.Set(nameof (CheckNewUsers), value);
      }
    }

    public bool MarkAsSpamOnHit
    {
      get
      {
        return this.GetBoolean(nameof (MarkAsSpamOnHit));
      }
      set
      {
        this.Set(nameof (MarkAsSpamOnHit), value);
      }
    }

    private static class Keys
    {
      public const string APIKey = "APIKey";
      public const string MarkAsSpamOnHit = "MarkAsSpamOnHit";
      public const string CheckNewUsers = "CheckNewUsers";
      public const string Enabled = "Enabled";
    }
  }
}
