// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.StopForumSpam.StopForumSpamInstall
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces;

namespace mvcForum.AddOns.StopForumSpam
{
  public class StopForumSpamInstall : IInstallable
  {
    private readonly IRepository<AddOnConfiguration> configRepo;

    public StopForumSpamInstall(IRepository<AddOnConfiguration> configRepo)
    {
      this.configRepo = configRepo;
    }

    public void Install()
    {
      StopForumSpamConfiguration spamConfiguration = new StopForumSpamConfiguration(this.configRepo);
      spamConfiguration.Enabled = false;
      spamConfiguration.RunAsynchronously = false;
      spamConfiguration.Delay = 5;
      spamConfiguration.MarkAsSpamOnHit = true;
      spamConfiguration.CheckNewUsers = true;
    }
  }
}
