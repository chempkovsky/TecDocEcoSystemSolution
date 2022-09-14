// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.StopForumSpam.StopForumSpamBuilder
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DependencyInjection;
using mvcForum.AddOns.Controllers;
using mvcForum.Core.Interfaces;
using mvcForum.Core.Interfaces.AddOns;
using mvcForum.Web.Interfaces;
using System.Collections.Generic;

namespace mvcForum.AddOns.StopForumSpam
{
  public class StopForumSpamBuilder : IDependencyBuilder
  {
    public void Configure(IDependencyContainer container)
    {
      container.Register<IAntiSpamAddOn, StopForumSpamAddOn>();
      container.Register<IAddOnConfiguration<StopForumSpamAddOn>, StopForumSpamConfiguration>();
      container.Register<IAntiSpamConfigurationController, StopForumSpamConfigurationController>();
      container.Register<IInstallable, StopForumSpamInstall>();
    }

    public void ValidateRequirements(IList<ApplicationRequirement> feedback)
    {
    }
  }
}
