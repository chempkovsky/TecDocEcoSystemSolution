// Decompiled with JetBrains decompiler
// Type: MVCBootstrap.Web.Security.IUser
// Assembly: MVCBootstrap, Version=0.7.3.31109, Culture=neutral, PublicKeyToken=null
// MVID: DDB5EB46-D133-4D70-972F-AD437AC12CD4
// Assembly location: C:\Development\WebCarShop\MVCBootstrap.dll

using System;

namespace MVCBootstrap.Web.Security
{
  public interface IUser
  {
    Guid Id { get; set; }

    string Password { get; set; }

    string PasswordAnswer { get; set; }

    string Salt { get; set; }

    string Email { get; set; }

    string Username { get; set; }

    string PasswordQuestion { get; set; }

    string Comment { get; set; }

    DateTime LastLockoutDate { get; set; }

    DateTime LastPasswordChangedDate { get; set; }

    DateTime LastActivityDate { get; set; }

    DateTime LastLoginDate { get; set; }

    DateTime CreationDate { get; set; }

    bool IsLockedOut { get; set; }

    bool IsAnonymous { get; set; }

    bool IsApproved { get; set; }

    int FailedPasswordAnswerAttemptCount { get; set; }

    DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }

    int FailedPasswordAttemptCount { get; set; }

    DateTime FailedPasswordAttemptWindowStart { get; set; }
  }
}
