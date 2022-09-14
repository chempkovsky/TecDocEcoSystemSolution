// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.ViewModels.ForumUserViewModel
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Web.ViewModels
{
  public class ForumUserViewModel
  {
    public ForumUserViewModel()
    {
    }

    public ForumUserViewModel(ForumUser user)
    {
      this.Id = user.Id;
      this.Name = user.Name;
      this.Email = user.EmailAddress;
      this.Deleted = user.Deleted;
      this.LastIP = user.LastIP;
      this.FirstVisit = user.FirstVisit;
      this.LastVisit = user.LastVisit;
    }

    [Required]
    public int Id { get; set; }

    public string Name { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public bool Active { get; set; }

    [Required]
    public bool Deleted { get; set; }

    public string NewPassword { get; set; }

    public string RepeatNewPassword { get; set; }

    public DateTime FirstVisit { get; set; }

    public DateTime LastVisit { get; set; }

    public string LastIP { get; set; }

    public IEnumerable<GroupViewModel> Groups { get; set; }

    public IEnumerable<GroupViewModel> MemberOf { get; set; }
  }
}
