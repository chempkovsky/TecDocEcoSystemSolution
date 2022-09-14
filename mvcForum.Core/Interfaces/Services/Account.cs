// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.Services.Account
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;

namespace mvcForum.Core.Interfaces.Services
{
  public class Account : IAccount
  {
    public string AccountName { get; set; }

    public bool IsLockedOut { get; set; }

    public bool IsApproved { get; set; }

    public string EmailAddress { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastActivityDate { get; set; }

    public DateTime LastLoginDate { get; set; }

    public DateTime LastLockoutDate { get; set; }

    public object ProviderUserKey { get; set; }
  }
}
