// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.Data.IPostRepository
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System;
using System.Collections.Generic;

namespace mvcForum.Core.Interfaces.Data
{
  public interface IPostRepository
  {
    IEnumerable<Post> Read(
      ForumUser user,
      Topic topic,
      int page,
      int postsPerPage,
      bool showDeleted);

    IEnumerable<Post> ReadAll(ForumUser user, Topic topic, bool showDeleted);

    IEnumerable<Post> ReadSinceLast(
      ForumUser user,
      Topic topic,
      int postsPerPage,
      bool showDeleted,
      out DateTime? lastRead,
      out int showingPage);
  }
}
