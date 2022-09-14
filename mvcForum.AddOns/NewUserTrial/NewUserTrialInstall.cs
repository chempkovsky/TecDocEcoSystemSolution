// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.NewUserTrial.NewUserTrialInstall
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces;
using mvcForum.Core.Specifications;
using System.Collections.Generic;

namespace mvcForum.AddOns.NewUserTrial
{
  public class NewUserTrialInstall : IInstallable
  {
    private readonly IRepository<AddOnConfiguration> configRepo;
    private readonly IRepository<Group> groupRepo;

    public NewUserTrialInstall(
      IRepository<AddOnConfiguration> configRepo,
      IRepository<Group> groupRepo)
    {
      this.configRepo = configRepo;
      this.groupRepo = groupRepo;
    }

    public void Install()
    {
      NewUserTrialConfiguration trialConfiguration = new NewUserTrialConfiguration(this.configRepo);
      trialConfiguration.Enabled = true;
      trialConfiguration.AutoLimit = 2;
      trialConfiguration.RunAsynchronously = false;
      trialConfiguration.Delay = 5;
      List<int> intList = new List<int>();
      Group group1 = this.groupRepo.ReadOne((ISpecification<Group>) new GroupSpecifications.SpecificName("Administrator"));
      if (group1 != null)
        intList.Add(group1.Id);
      Group group2 = this.groupRepo.ReadOne((ISpecification<Group>) new GroupSpecifications.SpecificName("Moderator"));
      if (group2 != null)
        intList.Add(group2.Id);
      trialConfiguration.ExcludeGroups = (IEnumerable<int>) intList;
    }
  }
}
