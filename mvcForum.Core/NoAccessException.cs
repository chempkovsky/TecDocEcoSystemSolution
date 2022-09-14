// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.NoAccessException
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;

namespace mvcForum.Core
{
  public class NoAccessException : ApplicationException
  {
    public readonly Forum Forum;
    public readonly AccessFlag AccessFlag;

    public NoAccessException(Forum forum, AccessFlag flag)
      : base(string.Format("The user does not have the required access ({1}) to forum '{0}'.", (object) forum.Name, (object) flag))
    {
      this.Forum = forum;
      this.AccessFlag = flag;
    }
  }
}
