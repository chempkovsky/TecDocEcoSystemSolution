// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Abstractions.Interfaces.IPerRequestCache
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

namespace mvcForum.Core.Abstractions.Interfaces
{
  public interface IPerRequestCache
  {
    T Pull<T>(int id) where T : ICacheable;

    void Push<T>(T element) where T : ICacheable;

    object Pull(string key);

    void Push(string key, object value);
  }
}
