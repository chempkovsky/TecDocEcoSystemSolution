// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Cache.TypeCache`1
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using System.Collections.Generic;

namespace mvcForum.Web.Cache
{
  public class TypeCache<T> where T : class
  {
    public readonly Dictionary<int, T> Elements = new Dictionary<int, T>();
  }
}
