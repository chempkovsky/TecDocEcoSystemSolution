// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.Services.IAccountExtensions
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

namespace mvcForum.Core.Interfaces.Services
{
  public static class IAccountExtensions
  {
    public static string ResetPassword(this IAccount account)
    {
      return string.Empty;
    }

    public static void ChangePassword(
      this IAccount account,
      string oldPassword,
      string newPassword)
    {
    }
  }
}
