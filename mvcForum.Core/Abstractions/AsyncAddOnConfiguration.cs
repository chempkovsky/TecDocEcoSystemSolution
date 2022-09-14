// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Abstractions.AsyncAddOnConfiguration`1
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core.Interfaces.AddOns;

namespace mvcForum.Core.Abstractions
{
  public abstract class AsyncAddOnConfiguration<TAddOn> : AddOnConfiguration<TAddOn>
    where TAddOn : IAddOn
  {
    protected AsyncAddOnConfiguration(IRepository<AddOnConfiguration> configRepo)
      : base(configRepo)
    {
    }

    public bool RunAsynchronously
    {
      get
      {
        return this.GetBoolean(nameof (RunAsynchronously));
      }
      set
      {
        this.Set(nameof (RunAsynchronously), value);
      }
    }

    public int Delay
    {
      get
      {
        return this.GetInt32(nameof (Delay));
      }
      set
      {
        this.Set(nameof (Delay), value);
      }
    }

    private static class Keys
    {
      public const string RunAsynchronously = "RunAsynchronously";
      public const string Delay = "Delay";
    }
  }
}
