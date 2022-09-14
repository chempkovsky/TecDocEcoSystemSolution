// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.UserViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using MVCBootstrap.Web.Mvc.Attributes;
using mvcForum.Core;
using System;

namespace mvcForum.Web.ViewModels
{
  public class UserViewModel : ForumViewModelBase
  {
    public UserViewModel(ForumUser user)
    {
      this.Culture = user.Culture;
      this.Timezone = user.Timezone;
      this.FullName = user.FullName;
      this.LastIP = user.LastIP;
      this.UserFlag = user.UserFlag;
      this.Name = user.Name;
      this.FirstVisit = user.FirstVisit;
      this.Email = user.EmailAddress;
      this.Id = user.Id;
      this.LastVisit = user.LastVisit;
    }

    public int Id { get; private set; }

    public string Culture { get; set; }

    public string Timezone { get; set; }

    [LocalizedDisplay(typeof (UserViewModel), "FullName")]
    public string FullName { get; set; }

    public string LastIP { get; private set; }

    public UserFlag UserFlag { get; private set; }

    [LocalizedDisplay(typeof (UserViewModel), "Name")]
    public string Name { get; private set; }

    [LocalizedDisplay(typeof (UserViewModel), "FirstVisit")]
    public DateTime FirstVisit { get; private set; }

    [LocalizedDisplay(typeof (UserViewModel), "LastVisit")]
    public DateTime LastVisit { get; private set; }

    [LocalizedDisplay(typeof (UserViewModel), "Email")]
    public string Email { get; set; }
  }
}
