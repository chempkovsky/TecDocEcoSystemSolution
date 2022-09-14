// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.UserFlag
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;

namespace mvcForum.Core
{
  [Flags]
  public enum UserFlag
  {
    None = 0,
    PublicEmailAddress = 1,
    EmailWhenPM = 2,
  }
}
