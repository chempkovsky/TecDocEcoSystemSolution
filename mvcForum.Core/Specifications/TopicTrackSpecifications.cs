// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.TopicTrackSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class TopicTrackSpecifications
  {
    public class SpecificTopic : ISpecification<TopicTrack>
    {
      private readonly Topic topic;

      public SpecificTopic(Topic topic)
      {
        this.topic = topic;
      }

      public Expression<Func<TopicTrack, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<TopicTrack, bool>>) (x => x.TopicId == this.topic.Id);
        }
      }
    }

    public class SpecificTopicAndUser : ISpecification<TopicTrack>
    {
      private readonly Topic topic;
      private readonly ForumUser user;

      public SpecificTopicAndUser(Topic topic, ForumUser user)
      {
        this.topic = topic;
        this.user = user;
      }

      public Expression<Func<TopicTrack, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<TopicTrack, bool>>) (x => x.TopicId == this.topic.Id && x.ForumUserId == this.user.Id);
        }
      }
    }
  }
}
