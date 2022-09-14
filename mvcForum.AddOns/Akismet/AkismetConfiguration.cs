// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.Akismet.AkismetConfiguration
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions;

namespace mvcForum.AddOns.Akismet
{
  public class AkismetConfiguration : AsyncAddOnConfiguration<AkismetAddOn>
  {
    public AkismetConfiguration(IRepository<AddOnConfiguration> configRepo)
      : base(configRepo)
    {
    }

    public string Key
    {
      get
      {
        return this.GetString("AkismetKey");
      }
      set
      {
        this.Set("AkismetKey", value);
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

    public int SpamScore
    {
      get
      {
        return this.GetInt32(nameof (SpamScore));
      }
      set
      {
        this.Set(nameof (SpamScore), value);
      }
    }

    private static class Keys
    {
      public const string AkismetKey = "AkismetKey";
      public const string Enabled = "Enabled";
      public const string MarkAsSpamOnHit = "MarkAsSpamOnHit";
      public const string SpamScore = "SpamScore";
    }
  }
}
