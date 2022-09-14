// Decompiled with JetBrains decompiler
// Type: mvcForum.SearchProvider.Lucene.LuceneSearchBuilder
// Assembly: mvcForum.SearchProvider.Lucene, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 13A5DCA9-908F-4689-9C6D-F584F417B850
// Assembly location: C:\Development\WebCarShop\mvcForum.SearchProvider.Lucene.dll

using ApplicationBoilerplate.DependencyInjection;
using mvcForum.SearchProvider.Lucene.Indexes;
using System.Collections.Generic;
using System.Web;

namespace mvcForum.SearchProvider.Lucene
{
  public class LuceneSearchBuilder : LuceneBaseSearchBuilder
  {
    public override void Configure(IDependencyContainer container)
    {
      IDirectoryResolver instance = (IDirectoryResolver) new FileSystemDirectoryResolver(HttpContext.Current.Server.MapPath("~/app_data"));
      container.RegisterSingleton<IDirectoryResolver>(instance);
      base.Configure(container);
    }

    public override void ValidateRequirements(IList<ApplicationRequirement> feedback)
    {
    }
  }
}
