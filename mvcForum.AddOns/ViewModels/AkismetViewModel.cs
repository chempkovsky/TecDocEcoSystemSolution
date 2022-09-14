// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.ViewModels.AkismetViewModel
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.AddOns.ViewModels
{
  public class AkismetViewModel
  {
    [Required]
    [LocalizedDisplay("mvcForum.AddOns.Akismet", "RunAsync")]
    public bool RunAsynchronously { get; set; }

    [LocalizedDisplay("mvcForum.AddOns.Akismet", "Delay")]
    [Required]
    public int Delay { get; set; }

    [LocalizedDisplay("mvcForum.AddOns.Akismet", "Key")]
    public string Key { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.AddOns.Akismet", "Enabled")]
    public bool Enabled { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.AddOns.Akismet", "MarkAsSpamOnHit")]
    public bool MarkAsSpamOnHit { get; set; }

    [LocalizedDisplay("mvcForum.AddOns.Akismet", "SpamScore")]
    public int? SpamScore { get; set; }
  }
}
