// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Configuration.NamedComponent
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Configuration;

namespace mvcForum.Core.Configuration
{
  public class NamedComponent : ConfigurationElement
  {
    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    public string Name
    {
      get
      {
        return (string) this["name"];
      }
      set
      {
        this["name"] = (object) value;
      }
    }

    [ConfigurationProperty("type", IsKey = false, IsRequired = true)]
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
