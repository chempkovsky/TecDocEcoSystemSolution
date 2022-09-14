// Decompiled with JetBrains decompiler
// Type: mvcForum.SearchProvider.Lucene.LuceneBaseSearchBuilder
// Assembly: mvcForum.SearchProvider.Lucene, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 13A5DCA9-908F-4689-9C6D-F584F417B850
// Assembly location: C:\Development\WebCarShop\mvcForum.SearchProvider.Lucene.dll

using ApplicationBoilerplate.DependencyInjection;
using BoC.Web.Mvc.PrecompiledViews;
using mvcForum.Core.Abstractions;
using mvcForum.Core.Interfaces;
using mvcForum.Core.Interfaces.AddOns;
using mvcForum.Core.Interfaces.Search;
using mvcForum.SearchProvider.Lucene.Controllers;
using mvcForum.Web.Interfaces;
using System.Collections.Generic;

namespace mvcForum.SearchProvider.Lucene
{
  public abstract class LuceneBaseSearchBuilder : IDependencyBuilder
  {
    public virtual void Configure(IDependencyContainer container)
    {
      container.Register<IIndexer, Indexer>();
      container.Register<ISearcher, Indexer>();
      container.Register<ISearchAddOn, Indexer>();
      container.Register<IAddOnConfiguration<Indexer>, LuceneConfiguration>();
      container.Register<AsyncAddOnConfiguration<Indexer>, LuceneConfiguration>();
      container.Register<ISearchConfigurationController, LuceneConfigurationController>();
      container.Register<IInstallable, LuceneInstall>();
      ApplicationPartRegistry.Register(typeof (LuceneSearchBuilder).Assembly);
    }

    public abstract void ValidateRequirements(IList<ApplicationRequirement> feedback);
  }
}
