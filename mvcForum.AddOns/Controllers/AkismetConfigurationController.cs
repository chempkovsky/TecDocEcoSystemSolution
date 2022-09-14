// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.Controllers.AkismetConfigurationController
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.AddOns.Akismet;
using mvcForum.AddOns.ViewModels;
using mvcForum.Core.Interfaces.AddOns;
using mvcForum.Web.Controllers;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using System.Web.Mvc;

namespace mvcForum.AddOns.Controllers
{
  public class AkismetConfigurationController : ForumAdminBaseController, IAntiSpamConfigurationController
  {
    private readonly AkismetConfiguration config;

    public AkismetConfigurationController(
      IWebUserProvider userProvider,
      IContext context,
      IAddOnConfiguration<AkismetAddOn> config)
      : base(userProvider, context)
    {
      this.config = (AkismetConfiguration) config;
    }

    public ViewResult Index()
    {
      return this.View((object) new AkismetViewModel()
      {
        Key = this.config.Key,
        Enabled = this.config.Enabled,
        SpamScore = new int?(this.config.SpamScore),
        MarkAsSpamOnHit = this.config.MarkAsSpamOnHit,
        RunAsynchronously = this.config.RunAsynchronously,
        Delay = this.config.Delay
      });
    }

    [HttpPost]
    public ViewResult Index(AkismetViewModel model)
    {
      if (this.ModelState.IsValid)
      {
        this.config.Enabled = model.Enabled;
        this.config.Key = model.Key;
        if (model.SpamScore.HasValue)
          this.config.SpamScore = model.SpamScore.Value;
        this.config.MarkAsSpamOnHit = model.MarkAsSpamOnHit;
        this.config.RunAsynchronously = model.RunAsynchronously;
        this.config.Delay = model.Delay;
        this.Context.SaveChanges();
        this.TempData["Saved"] = (object) true;
      }
      return this.View((object) model);
    }

    public string Name
    {
      get
      {
        return ForumHelper.GetString(nameof (Name), (object) null, "mvcForum.AddOns.Akismet");
      }
    }

    public string Description
    {
      get
      {
        return ForumHelper.GetString(nameof (Description), (object) null, "mvcForum.AddOns.Akismet");
      }
    }
  }
}
