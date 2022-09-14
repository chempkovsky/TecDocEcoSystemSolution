// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.DuplicateNameException
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

namespace mvcForum.Core
{
  public class DuplicateNameException : NewUserException
  {
    private readonly string Name;

    public DuplicateNameException(string name)
      : base(string.Format("The name {0} is already in use!", (object) name))
    {
      this.Name = name;
    }
  }
}
