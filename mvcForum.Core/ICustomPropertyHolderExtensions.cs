// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.ICustomPropertyHolderExtensions
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace mvcForum.Core
{
  public static class ICustomPropertyHolderExtensions
  {
    private static void LoadProperties(ICustomPropertyHolder holder)
    {
      if (holder.CustomData != null)
        return;
      if (!string.IsNullOrWhiteSpace(holder.CustomProperties))
        holder.CustomData = XDocument.Parse(holder.CustomProperties);
      else
        holder.CustomData = new XDocument(new object[1]
        {
          (object) new XElement((XName) "CustomProperties")
        });
    }

    public static Dictionary<string, string> GetCustomProperties(
      this ICustomPropertyHolder holder)
    {
      ICustomPropertyHolderExtensions.LoadProperties(holder);
      return holder.CustomData.Root.Elements((XName) "CustomProperty").ToDictionary<XElement, string, string>((Func<XElement, string>) (e => e.Attribute((XName) "Name").Value), (Func<XElement, string>) (e => e.Value), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    public static string GetCustomProperty(this ICustomPropertyHolder holder, string key)
    {
      ICustomPropertyHolderExtensions.LoadProperties(holder);
      XElement xelement = holder.CustomData.Root.Elements((XName) "CustomProperty").Where<XElement>((Func<XElement, bool>) (p =>
      {
        if (p.Attribute((XName) "Name") != null)
          return p.Attribute((XName) "Name").Value == key;
        return false;
      })).FirstOrDefault<XElement>();
      if (xelement != null)
        return xelement.Value;
      return string.Empty;
    }

    public static void SetCustomProperty(
      this ICustomPropertyHolder holder,
      string key,
      string value)
    {
      ICustomPropertyHolderExtensions.LoadProperties(holder);
      XElement xelement = holder.CustomData.Root.Elements((XName) "CustomProperty").Where<XElement>((Func<XElement, bool>) (p =>
      {
        if (p.Attribute((XName) "Name") != null)
          return p.Attribute((XName) "Name").Value == key;
        return false;
      })).FirstOrDefault<XElement>();
      if (xelement == null)
        holder.CustomData.Root.Add((object) new XElement((XName) "CustomProperty", new object[2]
        {
          (object) new XAttribute((XName) "Name", (object) key),
          (object) new XCData(value)
        }));
      else
        xelement.Value = value;
      holder.CustomProperties = holder.CustomData.ToString();
    }
  }
}
