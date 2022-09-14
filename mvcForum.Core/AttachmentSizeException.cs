// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.AttachmentSizeException
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

namespace mvcForum.Core
{
  public class AttachmentSizeException : AttachmentException
  {
    public readonly int Size;
    public readonly int MaxSize;

    public AttachmentSizeException(int size, int maxSize)
      : base(string.Format("The attachment is {0} bytes, the max allowed file size is {1}.", (object) size, (object) maxSize))
    {
      this.Size = size;
      this.MaxSize = maxSize;
    }
  }
}
