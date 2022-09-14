// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.AccessFlag
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;

namespace mvcForum.Core
{
  [Flags]
  public enum AccessFlag
  {
    None = 0,
    Read = 1,
    Post = 2,
    Reply = 4,
    Priority = 8,
    Poll = 16, // 0x00000010
    Vote = 32, // 0x00000020
    Moderator = 64, // 0x00000040
    Edit = 128, // 0x00000080
    Delete = 256, // 0x00000100
    Upload = 512, // 0x00000200
  }
}
