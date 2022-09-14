// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Interfaces.Services.ITopicService
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using System.Collections.Generic;

namespace mvcForum.Core.Interfaces.Services
{
  public interface ITopicService
  {
    Topic Create(
      ForumUser author,
      Forum forum,
      string subject,
      TopicType type,
      string body,
      string authorIP,
      string userAgent,
      string forumUrl,
      List<string> feedbackOutput);

    Topic Read(ForumUser user, int id);

    bool Update(ForumUser user, Topic topic, string title, string body, string forumURL);

    bool Update(
      ForumUser user,
      Topic topic,
      string title,
      string body,
      TopicType type,
      TopicFlag flag,
      string reason,
      string forumURL);
  }
}
