// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.Akismet.AkismetInstall
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces;

namespace mvcForum.AddOns.Akismet
{
  public class AkismetInstall : IInstallable
  {
    private readonly IRepository<AddOnConfiguration> configRepo;

    public AkismetInstall(IRepository<AddOnConfiguration> configRepo)
    {
      this.configRepo = configRepo;
    }

    public void Install()
    {
      AkismetConfiguration akismetConfiguration = new AkismetConfiguration(this.configRepo);
      akismetConfiguration.Delay = 5;
      akismetConfiguration.Enabled = false;
      akismetConfiguration.RunAsynchronously = false;
      akismetConfiguration.SpamScore = 100;
      akismetConfiguration.MarkAsSpamOnHit = true;
    }
  }
}
