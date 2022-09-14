// Decompiled with JetBrains decompiler
// Type: mvcForum.Core.Specifications.TopicSpecifications
// Assembly: mvcForum.Core, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 8359638B-AE62-48DB-A4A0-DF77AC7B7C29
// Assembly location: C:\Development\WebCarShop\mvcForum.Core.dll

using ApplicationBoilerplate.DataProvider;
using System;
using System.Linq.Expressions;

namespace mvcForum.Core.Specifications
{
  public static class TopicSpecifications
  {
    public class EmptyTopic : ISpecification<Topic>
    {
      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Topic, bool>>) (x => x.PostCount == 0 && x.FlagValue == 0);
        }
      }
    }

    public class AllVisibleByAuthor : ISpecification<Topic>
    {
      private readonly ForumUser author;

      public AllVisibleByAuthor(ForumUser author)
      {
        this.author = author;
      }

      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Topic, bool>>) (x => x.AuthorId == this.author.Id && (x.FlagValue & 4) == 0 && (x.FlagValue & 2) == 0);
        }
      }
    }

    public class AnnouncementsNotSpam : ISpecification<Topic>
    {
      private readonly Forum forum;
      private readonly int spamAverage;
      private readonly ForumUser user;
      private readonly bool moderator;

      public AnnouncementsNotSpam(Forum forum, int spamAverage, ForumUser user, bool moderator)
      {
        this.forum = forum;
        this.spamAverage = spamAverage;
        this.moderator = moderator;
        this.user = user;
      }

      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Topic, bool>>) (x => x.ForumId == this.forum.Id && x.TypeValue == 2 && (x.FlagValue & 4) == 0 && (x.FlagValue & 2) == 0 && (x.SpamReporters == 0 || x.SpamScore / x.SpamReporters <= this.spamAverage));
        }
      }
    }

    public class Announcements : ISpecification<Topic>
    {
      private readonly Forum forum;

      public Announcements(Forum forum)
      {
        this.forum = forum;
      }

      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Topic, bool>>) (x => x.ForumId == this.forum.Id && x.TypeValue == 2);
        }
      }
    }

    public class StickiesNotSpam : ISpecification<Topic>
    {
      private readonly Forum forum;
      private readonly int spamAverage;

      public StickiesNotSpam(Forum forum, int spamAverage)
      {
        this.forum = forum;
        this.spamAverage = spamAverage;
      }

      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Topic, bool>>) (x => x.ForumId == this.forum.Id && x.TypeValue == 1 && (x.FlagValue & 2) == 0 && (x.SpamReporters == 0 || x.SpamScore / x.SpamReporters >= this.spamAverage));
        }
      }
    }

    public class Stickies : ISpecification<Topic>
    {
      private readonly Forum forum;

      public Stickies(Forum forum)
      {
        this.forum = forum;
      }

      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Topic, bool>>) (x => x.ForumId == this.forum.Id && x.TypeValue == 1);
        }
      }
    }

    public class RegularsNotSpam : ISpecification<Topic>
    {
      private readonly Forum forum;
      private readonly int spamAverage;

      public RegularsNotSpam(Forum forum, int spamAverage)
      {
        this.forum = forum;
        this.spamAverage = spamAverage;
      }

      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Topic, bool>>) (x => x.ForumId == this.forum.Id && x.TypeValue == 4 && (x.FlagValue & 2) == 0 && (x.SpamReporters == 0 || x.SpamScore / x.SpamReporters >= this.spamAverage));
        }
      }
    }

    public class Regulars : ISpecification<Topic>
    {
      private readonly Forum forum;

      public Regulars(Forum forum)
      {
        this.forum = forum;
      }

      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Topic, bool>>) (x => x.ForumId == this.forum.Id && x.TypeValue == 4);
        }
      }
    }

    public class StickiesAndRegularsNotSpam : ISpecification<Topic>
    {
      private readonly Forum forum;
      private readonly int spamAverage;
      private readonly ForumUser user;
      private readonly bool moderator;

      public StickiesAndRegularsNotSpam(
        Forum forum,
        int spamAverage,
        ForumUser user,
        bool moderator)
      {
        this.forum = forum;
        this.spamAverage = spamAverage;
        this.moderator = moderator;
        this.user = user;
      }

      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          if (this.user != null)
            return (Expression<Func<Topic, bool>>) (x => x.ForumId == this.forum.Id && (x.TypeValue == 1 || x.TypeValue == 4) && ((x.FlagValue & 2) == 0 && (x.FlagValue & 4) == 0 || this.moderator || x.AuthorId == this.user.Id && (x.FlagValue & 4) != 0));
          return (Expression<Func<Topic, bool>>) (x => x.ForumId == this.forum.Id && (x.TypeValue == 1 || x.TypeValue == 4) && ((x.FlagValue & 2) == 0 && (x.FlagValue & 4) == 0 || this.moderator));
        }
      }
    }

    public class StickiesAndRegulars : ISpecification<Topic>
    {
      private readonly Forum forum;

      public StickiesAndRegulars(Forum forum)
      {
        this.forum = forum;
      }

      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Topic, bool>>) (x => x.ForumId == this.forum.Id && (x.TypeValue == 1 || x.TypeValue == 4));
        }
      }
    }

    public class MovedTopic : ISpecification<Topic>
    {
      private readonly Topic topic;

      public MovedTopic(Topic topic)
      {
        this.topic = topic;
      }

      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Topic, bool>>) (t => t.OriginalTopicId == (int?) this.topic.Id);
        }
      }
    }

    public class Visible : ISpecification<Topic>
    {
      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Topic, bool>>) (t => t.FlagValue == 0 || t.FlagValue == 1 || t.FlagValue == 8);
        }
      }
    }

    public class ById : ISpecification<Topic>
    {
      private readonly int id;

      public ById(int id)
      {
        this.id = id;
      }

      public Expression<Func<Topic, bool>> IsSatisfied
      {
        get
        {
          return (Expression<Func<Topic, bool>>) (t => t.Id == this.id);
        }
      }
    }
  }
}
