// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.Controllers.MultiPostConfigurationController
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.AddOns.MultiPost;
using mvcForum.AddOns.ViewModels;
using mvcForum.Core.Interfaces.AddOns;
using mvcForum.Web.Controllers;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using System.Web.Mvc;

namespace mvcForum.AddOns.Controllers
{
  public class MultiPostConfigurationController : ForumAdminBaseController, IAntiSpamConfigurationController
  {
    private readonly MultiPostConfiguration config;

    public MultiPostConfigurationController(
      IWebUserProvider userProvider,
      IContext context,
      IAddOnConfiguration<MultiPostAddOn> config)
      : base(userProvider, context)
    {
      this.config = (MultiPostConfiguration) config;
    }

    public ViewResult Index()
    {
      return this.View((object) new MultiPostViewModel()
      {
        Enabled = this.config.Enabled,
        Interval = new int?(this.config.Interval),
        Posts = new int?(this.config.Posts),
        RunAsynchronously = this.config.RunAsynchronously,
        Delay = this.config.Delay
      });
    }

    [HttpPost]
    public ViewResult Index(MultiPostViewModel model)
    {
      if (this.ModelState.IsValid)
      {
        this.config.Enabled = model.Enabled;
        if (model.Interval.HasValue)
          this.config.Interval = model.Interval.Value;
        if (model.Posts.HasValue)
          this.config.Posts = model.Posts.Value;
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
        return ForumHelper.GetString(nameof (Name), (object) null, "mvcForum.AddOns.MultiPost");
      }
    }

    public string Description
    {
      get
      {
        return ForumHelper.GetString(nameof (Description), (object) null, "mvcForum.AddOns.MultiPost");
      }
    }
  }
}
