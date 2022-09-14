// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.Services.IAttachmentService
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.IO;

namespace mvcForum.Core.Interfaces.Services
{
  public interface IAttachmentService
  {
    AttachStatusCode AttachFile(
      ForumUser author,
      Topic topic,
      string filename,
      string contentType,
      int length,
      Stream stream);

    AttachStatusCode AttachFile(
      ForumUser author,
      Post post,
      string filename,
      string contentType,
      int length,
      Stream stream);

    int GetSpaceUsed(ForumUser user);
  }
}
