// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.Controllers.StopForumSpamConfigurationController
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.AddOns.StopForumSpam;
using mvcForum.AddOns.ViewModels;
using mvcForum.Core.Interfaces.AddOns;
using mvcForum.Web.Controllers;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using System.Web.Mvc;

namespace mvcForum.AddOns.Controllers
{
  public class StopForumSpamConfigurationController : ForumAdminBaseController, IAntiSpamConfigurationController
  {
    private readonly StopForumSpamConfiguration config;

    public StopForumSpamConfigurationController(
      IWebUserProvider userProvider,
      IContext context,
      IAddOnConfiguration<StopForumSpamAddOn> config)
      : base(userProvider, context)
    {
      this.config = (StopForumSpamConfiguration) config;
    }

    public ViewResult Index()
    {
      return this.View((object) new StopForumSpamViewModel()
      {
        Key = this.config.Key,
        CheckNewUsers = this.config.CheckNewUsers,
        MarkAsSpamOnHit = this.config.MarkAsSpamOnHit,
        RunAsynchronously = this.config.RunAsynchronously,
        Delay = this.config.Delay,
        Enabled = this.config.Enabled
      });
    }

    [HttpPost]
    public ViewResult Index(StopForumSpamViewModel model)
    {
      if (this.ModelState.IsValid)
      {
        this.config.CheckNewUsers = model.CheckNewUsers;
        this.config.Key = model.Key;
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
        return ForumHelper.GetString(nameof (Name), (object) null, "mvcForum.AddOns.StopForumSpam");
      }
    }

    public string Description
    {
      get
      {
        return ForumHelper.GetString(nameof (Description), (object) null, "mvcForum.AddOns.StopForumSpam");
      }
    }
  }
}
