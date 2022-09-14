// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.Controllers.NewUserTrialConfigurationController
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.AddOns.NewUserTrial;
using mvcForum.AddOns.ViewModels;
using mvcForum.Core.Interfaces.AddOns;
using mvcForum.Web.Controllers;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.AddOns.Controllers
{
  public class NewUserTrialConfigurationController : ForumAdminBaseController, IAntiSpamConfigurationController
  {
    private readonly NewUserTrialConfiguration config;

    public NewUserTrialConfigurationController(
      IWebUserProvider userProvider,
      IContext context,
      IAddOnConfiguration<NewUserTrialAddOn> config)
      : base(userProvider, context)
    {
      this.config = (NewUserTrialConfiguration) config;
    }

    public ViewResult Index()
    {
      return this.View((object) new NewUserTrialViewModel()
      {
        Enabled = this.config.Enabled,
        AutoLimit = this.config.AutoLimit,
        ExcludeGroups = this.config.ExcludeGroups.ToList<int>(),
        RunAsynchronously = this.config.RunAsynchronously,
        Delay = this.config.Delay
      });
    }

    [HttpPost]
    public ViewResult Index(NewUserTrialViewModel model)
    {
      if (this.ModelState.IsValid)
      {
        this.config.Enabled = model.Enabled;
        this.config.AutoLimit = model.AutoLimit;
        this.config.RunAsynchronously = model.RunAsynchronously;
        this.config.Delay = model.Delay;
        this.config.ExcludeGroups = (IEnumerable<int>) model.ExcludeGroups;
        this.Context.SaveChanges();
        this.TempData["Saved"] = (object) true;
      }
      return this.View((object) model);
    }

    public string Name
    {
      get
      {
        return ForumHelper.GetString(nameof (Name), (object) null, "mvcForum.AddOns.NewUserTrial");
      }
    }

    public string Description
    {
      get
      {
        return ForumHelper.GetString(nameof (Description), (object) null, "mvcForum.AddOns.NewUserTrial");
      }
    }
  }
}
