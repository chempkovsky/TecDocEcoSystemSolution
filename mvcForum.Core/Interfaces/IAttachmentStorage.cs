// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.IAttachmentStorage
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.IO;

namespace mvcForum.Core.Interfaces
{
  public interface IAttachmentStorage
  {
    string CreateAttachment(Attachment attachment, Stream inputStream, string contentType);

    void DeleteAttachment(Attachment attachment);

    void DeleteAttachments(Post post);

    string Root { get; }
  }
}
