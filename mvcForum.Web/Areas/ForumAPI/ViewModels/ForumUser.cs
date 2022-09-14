// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.ViewModels.ForumUser
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.Areas.ForumAPI.ViewModels
{
  public class ForumUser
  {
    [Required]
    public int Id { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumUser", "Name")]
    public string Name { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumUser", "EmailAddress")]
    [Required]
    public string EmailAddress { get; set; }

    [Required]
    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumUser", "Deleted")]
    public bool Deleted { get; set; }

    [Required]
    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumUser", "Active")]
    public bool Active { get; set; }

    [Required]
    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumUser", "Locked")]
    public bool Locked { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumUser", "FirstVisit")]
    public DateTime FirstVisit { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumUser", "LastVisit")]
    public DateTime LastVisit { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumUser", "LastIP")]
    public string LastIP { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumUser", "Password")]
    public string Password { get; set; }

    [LocalizedDisplay("MVCForum.Web.ViewModels.ForumUser", "RepeatPassword")]
    public string RepeatPassword { get; set; }

    public string ApiUrl { get; set; }
  }
}
