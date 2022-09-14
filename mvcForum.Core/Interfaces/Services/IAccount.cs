// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.Services.IAccount
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;

namespace mvcForum.Core.Interfaces.Services
{
  public interface IAccount
  {
    string AccountName { get; set; }

    bool IsLockedOut { get; }

    bool IsApproved { get; set; }

    string EmailAddress { get; set; }

    DateTime CreationDate { get; }

    DateTime LastLoginDate { get; }

    DateTime LastActivityDate { get; }

    DateTime LastLockoutDate { get; }

    object ProviderUserKey { get; }
  }
}
