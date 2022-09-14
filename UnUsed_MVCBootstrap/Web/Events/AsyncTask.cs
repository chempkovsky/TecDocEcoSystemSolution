// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Events.AsyncTask
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using ApplicationBoilerplate.Events;
using System;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Events
{
  public class AsyncTask : IAsyncTask
  {
    public virtual void Execute(IEventListener listener, object data, int delay)
    {
      string key = this.GenerateKey(listener);
      if (listener.UniqueEvent && (!listener.UniqueEvent || HttpContext.Current.Cache[key] != null))
        return;
      HttpContext.Current.Cache.Add(key, data, (CacheDependency) null, DateTime.UtcNow.AddSeconds((double) delay), Cache.NoSlidingExpiration, CacheItemPriority.Normal, (CacheItemRemovedCallback) ((key2, data2, reason) => this.Execute(key2, data2)));
    }

    public virtual void Execute(string key, object payload)
    {
      string listener = this.GetListener(key);
      foreach (IEventListener service in DependencyResolver.Current.GetServices<IEventListener>())
      {
        if (service.GetType().FullName == listener)
          service.Handle(payload);
      }
    }

    protected string GenerateKey(IEventListener listener)
    {
      if (!listener.UniqueEvent)
        return string.Format("[{0}]{1}", (object) Guid.NewGuid().ToString(), (object) listener.GetType().FullName);
      return listener.GetType().FullName;
    }

    protected string GetListener(string key)
    {
      if (key.IndexOf("]") <= -1)
        return key;
      return key.Substring(key.IndexOf(']') + 1);
    }
  }
}
