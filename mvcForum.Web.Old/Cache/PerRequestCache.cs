// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Cache.PerRequestCache
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mvcForum.Web.Cache
{
  public class PerRequestCache : IPerRequestCache
  {
    private Dictionary<string, TypeCache<ICacheable>> cache = new Dictionary<string, TypeCache<ICacheable>>();
    private Dictionary<string, object> untypedCache = new Dictionary<string, object>();

    public T Pull<T>(int id) where T : ICacheable
    {
      string name = typeof (T).Name;
      if (this.cache.ContainsKey(name) && this.cache[name].Elements.ContainsKey(id))
        return (T) this.cache[name].Elements[id];
      return default (T);
    }

    public void Push<T>(T element) where T : ICacheable
    {
      string name = typeof (T).Name;
      if (!this.cache.ContainsKey(name))
        this.cache.Add(name, new TypeCache<ICacheable>());
      if (this.cache[name].Elements.Where<KeyValuePair<int, ICacheable>>((Func<KeyValuePair<int, ICacheable>, bool>) (x => x.Key == element.Id)).Any<KeyValuePair<int, ICacheable>>())
        this.cache[name].Elements[element.Id] = (ICacheable) element;
      else
        this.cache[name].Elements.Add(element.Id, (ICacheable) element);
    }

    public object Pull(string key)
    {
      if (this.untypedCache.ContainsKey(key))
        return this.untypedCache[key];
      return (object) null;
    }

    public void Push(string key, object value)
    {
      if (this.untypedCache.ContainsKey(key))
        this.untypedCache[key] = value;
      else
        this.untypedCache.Add(key, value);
    }
  }
}
