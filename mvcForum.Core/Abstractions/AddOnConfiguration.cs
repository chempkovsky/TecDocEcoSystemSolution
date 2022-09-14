// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Abstractions.AddOnConfiguration`1
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core.Interfaces.AddOns;
using mvcForum.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace mvcForum.Core.Abstractions
{
  public abstract class AddOnConfiguration<TAddOn> : IAddOnConfiguration<TAddOn> where TAddOn : IAddOn
  {
    private readonly AddOnConfiguration config;
    private readonly XmlDocument xml;
    private readonly XmlNode root;

    protected AddOnConfiguration(IRepository<AddOnConfiguration> configRepo)
    {
      string name = this.GetType().Name;
      this.config = configRepo.ReadOne((ISpecification<AddOnConfiguration>) new AddOnConfigurationSpecifications.SpecificKey(name));
      if (this.config == null)
      {
        this.config = new AddOnConfiguration(name, "<Settings />");
        configRepo.Create(this.config);
      }
      this.xml = new XmlDocument();
      this.xml.LoadXml(this.config.Value);
      this.root = this.xml.SelectSingleNode("./*");
    }

    protected virtual IEnumerable<string> GetStrings(string key)
    {
      List<string> stringList = new List<string>();
      XmlNode xmlNode = this.root.SelectSingleNode(key);
      if (xmlNode != null)
      {
        foreach (XmlNode selectNode in xmlNode.SelectNodes("Value"))
        {
          if (!stringList.Contains(selectNode.InnerText))
            stringList.Add(selectNode.InnerText);
        }
      }
      return (IEnumerable<string>) stringList;
    }

    protected virtual IEnumerable<int> GetCollection(string key)
    {
      IEnumerable<string> strings = this.GetStrings(key);
      List<int> intList = new List<int>();
      foreach (string s in strings)
      {
        int result;
        if (int.TryParse(s, out result) && !intList.Contains(result))
          intList.Add(result);
      }
      return (IEnumerable<int>) intList;
    }

    protected virtual int GetInt32(string key)
    {
      int result;
      if (this.root.Attributes.GetNamedItem(key) != null && int.TryParse(this.root.Attributes.GetNamedItem(key).Value, out result))
        return result;
      this.Set(key, 0);
      return this.GetInt32(key);
    }

    protected virtual string GetString(string key)
    {
      if (this.root.SelectSingleNode(key) != null)
        return this.root.SelectSingleNode(key).InnerText;
      this.Set(key, "");
      return this.GetString(key);
    }

    protected virtual bool GetBoolean(string key)
    {
      bool result;
      if (this.root.Attributes.GetNamedItem(key) != null && bool.TryParse(this.root.Attributes.GetNamedItem(key).Value, out result))
        return result;
      this.Set(key, false);
      return this.GetBoolean(key);
    }

    protected virtual void Set(string key, IEnumerable<string> values)
    {
      XmlNode newChild = this.root.SelectSingleNode(key);
      if (newChild == null)
      {
        newChild = (XmlNode) this.xml.CreateElement(key);
        this.root.AppendChild(newChild);
      }
      newChild.RemoveAll();
      foreach (string data in values)
      {
        XmlNode element = (XmlNode) this.xml.CreateElement("Value");
        newChild.AppendChild(element);
        element.AppendChild((XmlNode) this.xml.CreateCDataSection(data));
      }
      this.config.Value = this.xml.OuterXml;
    }

    protected virtual void Set(string key, IEnumerable<int> values)
    {
      this.Set(key, values.Select<int, string>((Func<int, string>) (v => v.ToString())));
    }

    protected virtual void Set(string key, int value)
    {
      this.SetAttributeValue(key, value.ToString());
    }

    protected virtual void Set(string key, bool value)
    {
      this.SetAttributeValue(key, value.ToString());
    }

    protected virtual void Set(string key, string value)
    {
      this.SetNodeContent(key, value);
    }

    private void SetAttributeValue(string key, string value)
    {
      if (this.root.Attributes.GetNamedItem(key) == null)
        this.root.Attributes.Append(this.xml.CreateAttribute(key));
      this.root.Attributes.GetNamedItem(key).Value = value;
      this.config.Value = this.xml.OuterXml;
    }

    private void SetNodeContent(string key, string value)
    {
      if (this.root.SelectSingleNode(key) == null)
        this.root.AppendChild((XmlNode) this.xml.CreateElement(key));
      this.root.SelectSingleNode(key).InnerText = value;
      this.config.Value = this.xml.OuterXml;
    }
  }
}
