// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.ViewModels.NewUserTrialViewModel
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.AddOns.ViewModels
{
  public class NewUserTrialViewModel
  {
    [Required]
    [LocalizedDisplay("mvcForum.AddOns.NewUserTrial", "Enabled")]
    public bool Enabled { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.AddOns.NewUserTrial", "RunAsync")]
    public bool RunAsynchronously { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.AddOns.NewUserTrial", "Delay")]
    public int Delay { get; set; }

    [LocalizedDisplay("mvcForum.AddOns.NewUserTrial", "AutoLimit")]
    public int AutoLimit { get; set; }

    [LocalizedDisplay("mvcForum.AddOns.NewUserTrial", "ExcludeGroups")]
    public List<int> ExcludeGroups { get; set; }
  }
}
