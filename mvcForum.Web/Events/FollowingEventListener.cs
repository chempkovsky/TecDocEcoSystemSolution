// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Events.FollowingEventListener
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using ApplicationBoilerplate.Logging;
using MVCBootstrap.Web.Mvc.Interfaces;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Events;
using SimpleLocalisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace mvcForum.Web.Events
{
  public class FollowingEventListener : IEventListener<NewPostEvent>, IEventListener<NewTopicEvent>, IEventListener<PostFlagUpdatedEvent>, IEventListener<TopicFlagUpdatedEvent>, IEventListener
  {
    private readonly IRepository<Post> postRepo;
    private readonly IRepository<Topic> topicRepo;
    private readonly IConfiguration config;
    private readonly ILogger logger;
    private readonly IMailService mailService;
    private readonly TextManager texts;

    public FollowingEventListener(
      IRepository<Post> postRepo,
      IRepository<Topic> topicRepo,
      IMailService mailService,
      IConfiguration config,
      ILogger logger,
      TextManager texts)
    {
      this.postRepo = postRepo;
      this.topicRepo = topicRepo;
      this.config = config;
      this.logger = logger;
      this.texts = texts;
      this.mailService = mailService;
    }

    public void Handle(object payload)
    {
      if (payload is NewTopicEvent)
        this.Handle((NewTopicEvent) payload);
      else if (payload is NewPostEvent)
        this.Handle((NewPostEvent) payload);
      else if (payload is TopicFlagUpdatedEvent)
      {
        this.Handle((TopicFlagUpdatedEvent) payload);
      }
      else
      {
        if (!(payload is PostFlagUpdatedEvent))
          throw new ApplicationException("Unknown payload!");
        this.Handle((PostFlagUpdatedEvent) payload);
      }
    }

    public void Handle(NewTopicEvent payload)
    {
      Topic topic = this.topicRepo.Read(payload.TopicId);
      if ((topic.Flag & TopicFlag.Deleted) != TopicFlag.None || (topic.Flag & TopicFlag.Quarantined) != TopicFlag.None)
        return;
      this.SendToForumFollowers(topic, payload.ForumRelativeURL);
    }

    public void Handle(NewPostEvent payload)
    {
      Post post = this.postRepo.Read(payload.PostId);
      if (post.Flag == PostFlag.Deleted || post.Flag == PostFlag.Quarantined)
        return;
      this.SendToTopicFollowers(post, payload.TopicRelativeURL);
    }

    public void Handle(TopicFlagUpdatedEvent payload)
    {
      Topic topic = this.topicRepo.Read(payload.TopicId);
      if ((payload.OriginalFlag & TopicFlag.Deleted) == TopicFlag.None && (payload.OriginalFlag & TopicFlag.Quarantined) == TopicFlag.None || ((topic.Flag & TopicFlag.Quarantined) != TopicFlag.None || (topic.Flag & TopicFlag.Deleted) != TopicFlag.None))
        return;
      this.SendToForumFollowers(topic, payload.ForumRelativeURL);
    }

    public void Handle(PostFlagUpdatedEvent payload)
    {
      Post post = this.postRepo.Read(payload.PostId);
      if (payload.OriginalFlag != PostFlag.Deleted && payload.OriginalFlag != PostFlag.Quarantined || (post.Flag == PostFlag.Deleted || post.Flag == PostFlag.Quarantined))
        return;
      this.SendToTopicFollowers(post, payload.TopicRelativeURL);
    }

    private void SendToForumFollowers(Topic topic, string actionURL)
    {
      string str1 = this.config.SiteURL;
      if (str1.EndsWith("/"))
        str1 = str1.Substring(0, str1.Length - 1);
      string str2 = string.Format("{0}{1}", (object) str1, (object) actionURL);
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
      foreach (FollowForum follower in (IEnumerable<FollowForum>) topic.Forum.Followers)
      {
        if (follower.ForumUser.Id != topic.Author.Id && !string.IsNullOrWhiteSpace(follower.ForumUser.EmailAddress))
        {
          string culture = follower.ForumUser.Culture;
          if (!dictionary1.ContainsKey(culture))
          {
            dictionary1.Add(culture, this.texts.Get<ForumConfigurator>("FollowForum.EmailSubject", (object) new
            {
              Topic = topic,
              Forum = topic.Forum
            }));
            dictionary2.Add(culture, this.texts.Get<ForumConfigurator>("FollowForum.EmailBody", (object) new
            {
              Topic = topic,
              Forum = topic.Forum,
              Link = str2
            }));
          }
          this.SendMail(follower.ForumUser.EmailAddress, follower.ForumUser.Name, culture, dictionary1[culture], dictionary2[culture]);
        }
      }
    }

    private void SendToTopicFollowers(Post post, string actionURL)
    {
      string str1 = this.config.SiteURL;
      if (str1.EndsWith("/"))
        str1 = str1.Substring(0, str1.Length - 1);
      string str2 = string.Format("{0}{1}", (object) str1, (object) actionURL);
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
      foreach (FollowTopic follower in (IEnumerable<FollowTopic>) post.Topic.Followers)
      {
        if (follower.ForumUser.Id != post.Author.Id && !string.IsNullOrWhiteSpace(follower.ForumUser.EmailAddress))
        {
          string culture = follower.ForumUser.Culture;
          if (!dictionary1.ContainsKey(culture))
          {
            dictionary1.Add(culture, this.texts.Get<ForumConfigurator>("FollowTopic.EmailSubject", (object) new
            {
              Post = post,
              Topic = post.Topic
            }));
            dictionary2.Add(culture, this.texts.Get<ForumConfigurator>("FollowTopic.EmailBody", (object) new
            {
              Post = post,
              Topic = post.Topic,
              Link = str2
            }));
          }
          this.SendMail(follower.ForumUser.EmailAddress, follower.ForumUser.Name, culture, dictionary1[culture], dictionary2[culture]);
        }
      }
    }

    private void SendMail(
      string recipientEmail,
      string recipientName,
      string culture,
      string subject,
      string body)
    {
      try
      {
        this.mailService.Send(new MailAddress(this.config.RobotEmailAddress, this.config.RobotName), (IList<MailAddress>) ((IEnumerable<MailAddress>) new MailAddress[1]
        {
          new MailAddress(recipientEmail, recipientName)
        }).ToList<MailAddress>(), subject, body);
        this.logger.Log(EventType.Debug, string.Format("The e-mail to user {0} ({1}) has been sent.", (object) recipientName, (object) recipientEmail));
      }
      catch (Exception ex)
      {
        this.logger.Log(EventType.Error, string.Format("Could not send e-mail to {0}, MessageAdded", (object) recipientEmail), ex);
      }
    }

    public bool RunAsynchronously
    {
      get
      {
        return false;
      }
    }

    public byte Priority
    {
      get
      {
        return byte.MaxValue;
      }
    }

    public bool UniqueEvent
    {
      get
      {
        return false;
      }
    }
  }
}
