// Decompiled with JetBrains decompiler
// Type: mvcForum.SearchProvider.Lucene.Controllers.LuceneConfigurationController
// Assembly: mvcForum.SearchProvider.Lucene, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 13A5DCA9-908F-4689-9C6D-F584F417B850
// Assembly location: C:\Development\WebCarShop\mvcForum.SearchProvider.Lucene.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core.Interfaces.AddOns;
using mvcForum.SearchProvider.Lucene.ViewModels;
using mvcForum.Web.Controllers;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using System.Web.Mvc;

namespace mvcForum.SearchProvider.Lucene.Controllers
{
  public class LuceneConfigurationController : ForumAdminBaseController, ISearchConfigurationController
  {
    private readonly LuceneConfiguration config;

    public LuceneConfigurationController(
      IWebUserProvider userProvider,
      IContext context,
      IAddOnConfiguration<Indexer> config)
      : base(userProvider, context)
    {
      this.config = (LuceneConfiguration) config;
    }

    public ViewResult Index()
    {
      return this.View((object) new LuceneViewModel()
      {
        Enabled = this.config.Enabled,
        RunAsynchronously = this.config.RunAsynchronously,
        Delay = this.config.Delay,
        AnnouncementWeight = this.config.AnnouncementWeight,
        StickyWeight = this.config.StickyWeight,
        TitleWeight = this.config.TitleWeight,
        TopicWeight = this.config.TopicWeight
      });
    }

    [HttpPost]
    public ViewResult Index(LuceneViewModel model)
    {
      if (this.ModelState.IsValid)
      {
        this.config.Enabled = model.Enabled;
        this.config.Delay = model.Delay;
        this.config.RunAsynchronously = model.RunAsynchronously;
        this.config.AnnouncementWeight = model.AnnouncementWeight;
        this.config.StickyWeight = model.StickyWeight;
        this.config.TitleWeight = model.TitleWeight;
        this.config.TopicWeight = model.TopicWeight;
        this.Context.SaveChanges();
        this.TempData["Saved"] = (object) true;
      }
      return this.View((object) model);
    }

    public string Name
    {
      get
      {
        return ForumHelper.GetString<Indexer>(nameof (Name));
      }
    }

    public string Description
    {
      get
      {
        return ForumHelper.GetString<Indexer>(nameof (Description));
      }
    }
  }
}
