// Decompiled with JetBrains decompiler
// Type: mvcForum.SearchProvider.Lucene.LuceneInstall
// Assembly: mvcForum.SearchProvider.Lucene, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 13A5DCA9-908F-4689-9C6D-F584F417B850
// Assembly location: C:\Development\WebCarShop\mvcForum.SearchProvider.Lucene.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces;

namespace mvcForum.SearchProvider.Lucene
{
  public class LuceneInstall : IInstallable
  {
    private readonly IRepository<AddOnConfiguration> configRepo;

    public LuceneInstall(IRepository<AddOnConfiguration> configRepo)
    {
      this.configRepo = configRepo;
    }

    public void Install()
    {
      LuceneConfiguration luceneConfiguration = new LuceneConfiguration(this.configRepo);
      luceneConfiguration.Enabled = true;
      luceneConfiguration.RunAsynchronously = false;
      luceneConfiguration.Delay = 15;
      luceneConfiguration.AnnouncementWeight = 400;
      luceneConfiguration.StickyWeight = 300;
      luceneConfiguration.TitleWeight = 200;
      luceneConfiguration.TopicWeight = 200;
    }
  }
}
