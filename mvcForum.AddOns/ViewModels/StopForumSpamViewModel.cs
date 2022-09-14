// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.ViewModels.StopForumSpamViewModel
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.AddOns.ViewModels
{
  public class StopForumSpamViewModel
  {
    [Required]
    [LocalizedDisplay("mvcForum.AddOns.NewUserTrial", "Enabled")]
    public bool Enabled { get; set; }

    [LocalizedDisplay("mvcForum.AddOns.StopForumSpam", "RunAsync")]
    [Required]
    public bool RunAsynchronously { get; set; }

    [LocalizedDisplay("mvcForum.AddOns.StopForumSpam", "Delay")]
    [Required]
    public int Delay { get; set; }

    [LocalizedDisplay("mvcForum.AddOns.StopForumSpam", "Key")]
    public string Key { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.AddOns.StopForumSpam", "MarkAsSpamOnHit")]
    public bool MarkAsSpamOnHit { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.AddOns.StopForumSpam", "CheckNewUsers")]
    public bool CheckNewUsers { get; set; }
  }
}
