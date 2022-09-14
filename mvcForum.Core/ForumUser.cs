// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.ForumUser
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace mvcForum.Core
{
  public class ForumUser
  {
    public ForumUser()
    {
    }

    public ForumUser(string providerId, string name, string emailAddress, string ip)
    {
      this.ProviderId = providerId;
      this.Name = name;
      this.EmailAddress = emailAddress;
      this.FirstVisit = DateTime.UtcNow;
      this.LastVisit = DateTime.UtcNow;
      this.LastIP = ip;
      this.Deleted = false;
      this.UserFlag = UserFlag.EmailWhenPM;
      this.Active = false;
      this.ExternalAccount = false;
      this.Theme = string.Empty;
    }

    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string ProviderId { get; set; }

    [StringLength(256)]
    [Required]
    public string Name { get; set; }

    [Required]
    [StringLength(200)]
    public string EmailAddress { get; set; }

    [Required]
    public DateTime FirstVisit { get; set; }

    [Required]
    public DateTime LastVisit { get; set; }

    [StringLength(50)]
    public string LastIP { get; set; }

    [Required]
    public bool Deleted { get; set; }

    [Required]
    public bool Active { get; set; }

    [Required]
    [StringLength(100)]
    public string Timezone { get; set; }

    [Required]
    [StringLength(10)]
    public string Culture { get; set; }

    [StringLength(200)]
    public string FullName { get; set; }

    public UserFlag UserFlag
    {
      get
      {
        return (UserFlag) this.UserFlagValue;
      }
      set
      {
        this.UserFlagValue = (int) value;
      }
    }

    [Required]
    public int UserFlagValue { get; set; }

    [Required]
    public bool UseFullName { get; set; }

    [Required]
    public bool ExternalAccount { get; set; }

    [StringLength(50)]
    public string ExternalProvider { get; set; }

    [StringLength(200)]
    public string ExternalProviderId { get; set; }

    [StringLength(200)]
    public string Theme { get; set; }
  }
}
