// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.UserNotActiveException
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

namespace mvcForum.Core
{
  public class UserNotActiveException : LogOnException
  {
    public readonly ForumUser User;

    public UserNotActiveException(ForumUser user)
      : base(string.Format("The user {0} is not active.", (object) user.Name))
    {
      this.User = user;
    }
  }
}
