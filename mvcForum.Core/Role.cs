// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Role
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.Collections.Generic;

namespace mvcForum.Core
{
  public class Role
  {
    public Guid Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<User> Users { get; set; }

    public Role()
    {
      this.Users = (ICollection<User>) new List<User>();
    }
  }
}
