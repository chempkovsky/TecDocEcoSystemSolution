// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.ForumConfiguration
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DependencyInjection;
using mvcForum.Core.Interfaces;
using mvcForum.Core.Interfaces.Search;
using System;

namespace mvcForum.Core
{
  public class ForumConfiguration
  {
    private static bool initialized = false;
    private static object objectLock = new object();
    public readonly IDependencyContainer container;

    protected ForumConfiguration()
    {
    }

    public ForumConfiguration SetAttachmentConfiguration(
      IAttachmentStorage attachmentStorage)
    {
      this.container.UnRegister<IAttachmentStorage>();
      this.container.RegisterSingleton<IAttachmentStorage>(attachmentStorage);
      return this;
    }

    public ForumConfiguration AddIndexer(IIndexer indexer)
    {
      this.container.RegisterSingleton<IIndexer>(indexer);
      return this;
    }

    public ForumConfiguration AddSearcher(ISearcher searcher)
    {
      this.container.RegisterSingleton<ISearcher>(searcher);
      return this;
    }

    public static ForumConfiguration Initialize()
    {
      if (!ForumConfiguration.initialized)
      {
        lock (ForumConfiguration.objectLock)
        {
          if (!ForumConfiguration.initialized)
            return new ForumConfiguration();
        }
      }
      throw new ApplicationException("Already initialized");
    }
  }
}
