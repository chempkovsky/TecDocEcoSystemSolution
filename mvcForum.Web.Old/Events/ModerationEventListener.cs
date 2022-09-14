// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Events.ModerationEventListener
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
using mvcForum.Core.Specifications;
using SimpleLocalisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace mvcForum.Web.Events
{
  public class ModerationEventListener : IEventListener<NewPostEvent>, IEventListener<NewTopicEvent>, IEventListener<PostFlagUpdatedEvent>, IEventListener<TopicFlagUpdatedEvent>, IEventListener
  {
    private readonly IRepository<Topic> topicRepo;
    private readonly IRepository<Post> postRepo;
    private readonly IConfiguration configuration;
    private readonly IMailService mail;
    private readonly IRepository<ForumAccess> faRepo;
    private readonly IRepository<GroupMember> gmRepo;
    private readonly ILogger logger;
    private readonly TextManager textManager;

    public ModerationEventListener(
      IRepository<Topic> topicRepo,
      IRepository<Post> postRepo,
      IConfiguration configuration,
      IMailService mail,
      IRepository<ForumAccess> faRepo,
      IRepository<GroupMember> gmRepo,
      ILogger logger,
      TextManager textManager)
    {
      this.topicRepo = topicRepo;
      this.postRepo = postRepo;
      this.configuration = configuration;
      this.mail = mail;
      this.gmRepo = gmRepo;
      this.faRepo = faRepo;
      this.logger = logger;
      this.textManager = textManager;
    }

    public void Handle(object payload)
    {
      if (payload is TopicFlagUpdatedEvent)
        this.Handle((TopicFlagUpdatedEvent) payload);
      else if (payload is PostFlagUpdatedEvent)
        this.Handle((PostFlagUpdatedEvent) payload);
      else if (payload is NewPostEvent)
      {
        this.Handle((NewPostEvent) payload);
      }
      else
      {
        if (!(payload is NewTopicEvent))
          throw new ApplicationException("Unknown payload!");
        this.Handle((NewTopicEvent) payload);
      }
    }

    public void Handle(TopicFlagUpdatedEvent payload)
    {
      if (!this.configuration.InformOnQuarantine)
        return;
      Topic topic = this.topicRepo.Read(payload.TopicId);
      if (payload.OriginalFlag == TopicFlag.Quarantined || topic.Flag != TopicFlag.Quarantined)
        return;
      Dictionary<string, string> moderators = this.GetModerators(topic.Forum);
      string subject = this.textManager.Get("TopicSubject", (object) null, "mvcForum.Web.Events.ModerationEventListener");
      string siteUrl = this.configuration.SiteURL;
      if (!siteUrl.EndsWith("/"))
        siteUrl += "/";
      string str1 = siteUrl + "forum/moderate/index/" + (object) topic.ForumId;
      TextManager textManager = this.textManager;
      string str2 = "mvcForum.Web.Events.ModerationEventListener";
      object values = (object) new
      {
        Title = topic.Title,
        Url = str1,
        Reason = topic.Posts.OrderBy<Post, DateTime>((Func<Post, DateTime>) (p => p.Posted)).First<Post>().EditReason
      };
      string ns = str2;
      string body = textManager.Get("TopicBody", values, ns);
      this.Send(moderators, subject, body);
    }

    public void Handle(PostFlagUpdatedEvent payload)
    {
      if (!this.configuration.InformOnQuarantine)
        return;
      Post post = this.postRepo.Read(payload.PostId);
      if (payload.OriginalFlag == PostFlag.Quarantined || post.Flag != PostFlag.Quarantined)
        return;
      Dictionary<string, string> moderators = this.GetModerators(post.Topic.Forum);
      string subject = this.textManager.Get("PostSubject", (object) null, "mvcForum.Web.Events.ModerationEventListener");
      string siteUrl = this.configuration.SiteURL;
      if (!siteUrl.EndsWith("/"))
        siteUrl += "/";
      string str1 = siteUrl + "forum/moderate/index/" + (object) post.Topic.ForumId;
      TextManager textManager = this.textManager;
      string str2 = "mvcForum.Web.Events.ModerationEventListener";
      object values = (object) new
      {
        Subject = post.Subject,
        Title = post.Topic.Title,
        Url = str1,
        Reason = post.EditReason
      };
      string ns = str2;
      string body = textManager.Get("PostBody", values, ns);
      this.Send(moderators, subject, body);
    }

    public void Handle(NewTopicEvent payload)
    {
      if (!this.configuration.InformOnQuarantine)
        return;
      Topic topic = this.topicRepo.Read(payload.TopicId);
      if (topic.Flag != TopicFlag.Quarantined)
        return;
      Dictionary<string, string> moderators = this.GetModerators(topic.Forum);
      string subject = this.textManager.Get("TopicSubject", (object) null, "mvcForum.Web.Events.ModerationEventListener");
      string siteUrl = this.configuration.SiteURL;
      if (!siteUrl.EndsWith("/"))
        siteUrl += "/";
      string str1 = siteUrl + "forum/moderate/index/" + (object) topic.ForumId;
      TextManager textManager = this.textManager;
      string str2 = "mvcForum.Web.Events.ModerationEventListener";
      object values = (object) new
      {
        Title = topic.Title,
        Url = str1,
        Reason = topic.Posts.OrderBy<Post, DateTime>((Func<Post, DateTime>) (p => p.Posted)).First<Post>().EditReason
      };
      string ns = str2;
      string body = textManager.Get("TopicBody", values, ns);
      this.Send(moderators, subject, body);
    }

    public void Handle(NewPostEvent payload)
    {
      if (!this.configuration.InformOnQuarantine)
        return;
      Post post = this.postRepo.Read(payload.PostId);
      if (post.Flag != PostFlag.Quarantined)
        return;
      Dictionary<string, string> moderators = this.GetModerators(post.Topic.Forum);
      string subject = this.textManager.Get("PostSubject", (object) null, "mvcForum.Web.Events.ModerationEventListener");
      string siteUrl = this.configuration.SiteURL;
      if (!siteUrl.EndsWith("/"))
        siteUrl += "/";
      string str1 = siteUrl + "forum/moderate/index/" + (object) post.Topic.ForumId;
      TextManager textManager = this.textManager;
      string str2 = "mvcForum.Web.Events.ModerationEventListener";
      object values = (object) new
      {
        Subject = post.Subject,
        Title = post.Topic.Title,
        Url = str1,
        Reason = post.EditReason
      };
      string ns = str2;
      string body = textManager.Get("PostBody", values, ns);
      this.Send(moderators, subject, body);
    }

    private Dictionary<string, string> GetModerators(Forum forum)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      IEnumerable<ForumAccess> forumAccesses = this.faRepo.ReadMany((ISpecification<ForumAccess>) new ForumAccessSpecifications.SpecificForum(forum));
      List<Group> groupList = new List<Group>();
      foreach (ForumAccess forumAccess in forumAccesses)
      {
        if ((forumAccess.AccessMask.AccessFlag & AccessFlag.Moderator) == AccessFlag.Moderator && !groupList.Contains(forumAccess.Group))
          groupList.Add(forumAccess.Group);
      }
      IEnumerable<ForumUser> forumUsers = (IEnumerable<ForumUser>) new List<ForumUser>();
      foreach (Group group in groupList)
      {
        IEnumerable<GroupMember> source = this.gmRepo.ReadMany((ISpecification<GroupMember>) new GroupMemberSpecifications.SpecificGroup(group));
        forumUsers = forumUsers.Concat<ForumUser>(source.Select<GroupMember, ForumUser>((Func<GroupMember, ForumUser>) (m => m.ForumUser)));
      }
      return forumUsers.ToDictionary<ForumUser, string, string>((Func<ForumUser, string>) (u => u.EmailAddress), (Func<ForumUser, string>) (u =>
      {
        if (!(u.UseFullName & !string.IsNullOrWhiteSpace(u.FullName)))
          return u.Name;
        return u.FullName;
      }));
    }

    private void Send(Dictionary<string, string> moderators, string subject, string body)
    {
      foreach (KeyValuePair<string, string> moderator in moderators)
      {
        try
        {
          this.mail.Send(new MailAddress(this.configuration.RobotEmailAddress, this.configuration.RobotName), (IList<MailAddress>) ((IEnumerable<MailAddress>) new MailAddress[1]
          {
            new MailAddress(moderator.Key, moderator.Value)
          }).ToList<MailAddress>(), subject, body);
          this.logger.Log(EventType.Debug, string.Format("The e-mail to user {0} ({1}) has been sent.", (object) moderator.Value, (object) moderator.Key));
        }
        catch (Exception ex)
        {
          this.logger.Log(EventType.Error, string.Format("Could not send e-mail to {0}, MessageAdded", (object) moderator.Key), ex);
        }
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
