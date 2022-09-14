// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.InvalidExtensionException
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

namespace mvcForum.Core
{
  public class InvalidExtensionException : AttachmentException
  {
    public readonly string Extension;

    public InvalidExtensionException(string extension)
      : base(string.Format("The extension {0} is not allowed.", (object) extension))
    {
      this.Extension = extension;
    }
  }
}
