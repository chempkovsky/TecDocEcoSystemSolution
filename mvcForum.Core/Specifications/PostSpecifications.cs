// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.PostSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class PostSpecifications
  {
    public class NotSpamNotDeleted : ISpecification<Post>
    {
      private readonly Topic topic;

      public NotSpamNotDeleted(Topic topic)
      {
        this.topic = topic;
      }

      public Expression<Func<Post, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Post, bool>>) (x => x.TopicId == this.topic.Id && (x.FlagValue & 1) == 0 && (x.FlagValue & 2) == 0);
        }
      }
    }

    public class NotSpam : ISpecification<Post>
    {
      private readonly Topic topic;
      private readonly ForumUser user;
      private readonly bool moderator;

      public NotSpam(Topic topic, ForumUser user, bool moderator)
      {
        this.topic = topic;
        this.user = user;
        this.moderator = moderator;
      }

      public Expression<Func<Post, bool>> IsSatisfied
      {
        get
        {
          if (this.user != null)
            return (Expression<Func<Post, bool>>) (x => x.TopicId == this.topic.Id && ((x.FlagValue & 2) == 0 && (x.FlagValue & 1) == 0 || this.moderator || x.AuthorId == this.user.Id && (x.FlagValue & 1) != 0));
          return (Expression<Func<Post, bool>>) (x => x.TopicId == this.topic.Id && ((x.FlagValue & 2) == 0 && (x.FlagValue & 1) == 0 || this.moderator));
        }
      }
    }

    public class All : ISpecification<Post>
    {
      private readonly Topic topic;

      public All(Topic topic)
      {
        this.topic = topic;
      }

      public Expression<Func<Post, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Post, bool>>) (x => x.TopicId == this.topic.Id);
        }
      }
    }

    public class RecentlyByAuthor : ISpecification<Post>
    {
      private readonly ForumUser author;
      private readonly int interval;

      public RecentlyByAuthor(ForumUser author, int interval)
      {
        this.author = author;
        this.interval = interval;
      }

      public Expression<Func<Post, bool>> IsSatisfied
      {
        get
        {
          DateTime dt = DateTime.UtcNow.AddMinutes((double) (-1 * this.interval));
          return (Expression<Func<Post, bool>>) (x => x.AuthorId == this.author.Id && x.Posted >= dt);
        }
      }
    }

    public class AllVisibleByAuthor : ISpecification<Post>
    {
      private readonly ForumUser author;

      public AllVisibleByAuthor(ForumUser author)
      {
        this.author = author;
      }

      public Expression<Func<Post, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Post, bool>>) (x => x.AuthorId == this.author.Id && (x.FlagValue & 1) == 0 && (x.FlagValue & 2) == 0);
        }
      }
    }

    public class Visible : ISpecification<Post>
    {
      public Expression<Func<Post, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Post, bool>>) (p => p.FlagValue == 0);
        }
      }
    }

    public class RegularListing : ISpecification<Post>
    {
      private readonly Topic topic;
      private readonly bool getDeleted;

      public RegularListing(Topic topic, bool getDeleted)
      {
        this.topic = topic;
        this.getDeleted = getDeleted;
      }

      public Expression<Func<Post, bool>> IsSatisfied
      {
        get
        {
          if (this.getDeleted)
            return (Expression<Func<Post, bool>>) (p => p.Topic.Id == this.topic.Id && (p.FlagValue == 1 || p.FlagValue == 0));
          return (Expression<Func<Post, bool>>) (p => p.Topic.Id == this.topic.Id && p.FlagValue == 0);
        }
      }
    }
  }
}
