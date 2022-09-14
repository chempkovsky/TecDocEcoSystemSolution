// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.MultiPost.MultiPostInstall
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces;

namespace mvcForum.AddOns.MultiPost
{
  public class MultiPostInstall : IInstallable
  {
    private readonly IRepository<AddOnConfiguration> configRepo;

    public MultiPostInstall(IRepository<AddOnConfiguration> configRepo)
    {
      this.configRepo = configRepo;
    }

    public void Install()
    {
      new MultiPostConfiguration(this.configRepo)
      {
        Enabled = true,
        Interval = 2,
        Posts = 5
      }.RunAsynchronously = false;
    }
  }
}
