// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.User
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.Collections.Generic;

namespace mvcForum.Core
{
  public class User
  {
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string EmailAddress { get; set; }

    public string Password { get; set; }

    public DateTime Created { get; set; }

    public DateTime LastVisit { get; set; }

    public bool Locked { get; set; }

    public bool Approved { get; set; }

    public DateTime LastPasswordFailure { get; set; }

    public int PasswordFailures { get; set; }

    public DateTime LastLockout { get; set; }

    public virtual ICollection<Role> Roles { get; set; }

    public User()
    {
      this.Roles = (ICollection<Role>) new List<Role>();
    }
  }
}
