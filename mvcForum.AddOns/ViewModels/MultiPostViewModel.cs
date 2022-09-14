// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.ViewModels.MultiPostViewModel
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.AddOns.ViewModels
{
  public class MultiPostViewModel
  {
    [LocalizedDisplay("mvcForum.AddOns.MultiPost", "RunAsync")]
    public bool RunAsynchronously { get; set; }

    [LocalizedDisplay("mvcForum.AddOns.MultiPost", "Delay")]
    [Required]
    public int Delay { get; set; }

    [Required]
    [LocalizedDisplay("mvcForum.AddOns.MultiPost", "Enabled")]
    public bool Enabled { get; set; }

    [LocalizedDisplay("mvcForum.AddOns.MultiPost", "Interval")]
    public int? Interval { get; set; }

    [LocalizedDisplay("mvcForum.AddOns.MultiPost", "Posts")]
    public int? Posts { get; set; }
  }
}
