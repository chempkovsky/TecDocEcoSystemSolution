// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Configuration.NamedComponentsElementCollection
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Configuration;

namespace mvcForum.Core.Configuration
{
  public class NamedComponentsElementCollection : ConfigurationElementCollection
  {
    public NamedComponent this[int index]
    {
      get
      {
        return (NamedComponent) this.BaseGet(index);
      }
      set
      {
        if (this.BaseGet(index) != null)
          this.BaseRemoveAt(index);
        this.BaseAdd(index, (ConfigurationElement) value);
      }
    }

    public void Add(NamedComponent component)
    {
      this.BaseAdd((ConfigurationElement) component);
    }

    public void Clear()
    {
      this.BaseClear();
    }

    protected override ConfigurationElement CreateNewElement()
    {
      return (ConfigurationElement) new NamedComponent();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
      return (object) ((NamedComponent) element).Name;
    }

    public void Remove(NamedComponent component)
    {
      this.BaseRemove((object) component.Name);
    }

    public void RemoveAt(int index)
    {
      this.BaseRemoveAt(index);
    }

    public void Remove(string name)
    {
      this.BaseRemove((object) name);
    }
  }
}
