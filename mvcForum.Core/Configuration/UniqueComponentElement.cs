// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Configuration.UniqueComponentElement
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Configuration;

namespace mvcForum.Core.Configuration
{
  public class UniqueComponentElement : ConfigurationElement
  {
    [ConfigurationProperty("type", IsRequired = true)]
    public string Type
    {
      get
      {
        return (string) this["type"];
      }
      set
      {
        this["type"] = (object) value;
      }
    }
  }
}
