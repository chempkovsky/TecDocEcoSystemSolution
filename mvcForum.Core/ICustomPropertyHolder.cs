// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.ICustomPropertyHolder
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Xml.Linq;

namespace mvcForum.Core
{
  public interface ICustomPropertyHolder
  {
    string CustomProperties { get; set; }

    XDocument CustomData { get; set; }
  }
}
