// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.Data.ITopicRepository
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System.Collections.Generic;

namespace mvcForum.Core.Interfaces.Data
{
  public interface ITopicRepository : IRepository<Topic>
  {
    IList<Topic> ReadAnnouncements(Forum forum, ForumUser user, bool isModerator);

    IList<Topic> ReadStickiesAndRegulars(
      Forum forum,
      int page,
      int announcementCount,
      ForumUser user,
      bool isModerator);

    IList<Topic> ReadQuarantined(Forum forum);

    IList<Topic> ReadTopics(Forum forum, int page, ForumUser user, bool isModerator);

    IEnumerable<Topic> ReadManyOptimized(ISpecification<Topic> spec);

    Topic ReadOneOptimizedWithPosts(int id);

    Topic ReadOneOptimized(ISpecification<Topic> spec);

    Topic ReadOneOptimizedWithPosts(ISpecification<Topic> spec);
  }
}
