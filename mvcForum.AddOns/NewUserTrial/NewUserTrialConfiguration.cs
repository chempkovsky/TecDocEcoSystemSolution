// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.NewUserTrial.NewUserTrialConfiguration
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions;
using System.Collections.Generic;

namespace mvcForum.AddOns.NewUserTrial
{
  public class NewUserTrialConfiguration : AsyncAddOnConfiguration<NewUserTrialAddOn>
  {
    public NewUserTrialConfiguration(IRepository<AddOnConfiguration> configRepo)
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

    public int AutoLimit
    {
      get
      {
        return this.GetInt32(nameof (AutoLimit));
      }
      set
      {
        this.Set(nameof (AutoLimit), value);
      }
    }

    public IEnumerable<int> ExcludeGroups
    {
      get
      {
        return this.GetCollection(nameof (ExcludeGroups));
      }
      set
      {
        this.Set(nameof (ExcludeGroups), value);
      }
    }

    private static class Keys
    {
      public const string Enabled = "Enabled";
      public const string AutoQuarantine = "AutoQuarantine";
      public const string AutoLimit = "AutoLimit";
      public const string ExcludeGroups = "ExcludeGroups";
    }
  }
}
