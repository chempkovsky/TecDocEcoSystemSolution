// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Services.TopicService
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using mvcForum.Core;
using mvcForum.Core.Events;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mvcForum.Web.Services
{
  public class TopicService : ITopicService
  {
    private readonly IContext context;
    private readonly IRepository<Topic> topicRepo;
    private readonly IRepository<Post> postRepo;
    private readonly IForumAccessService accessService;
    private readonly IEventPublisher eventPublisher;

    public TopicService(
      IContext context,
      IForumAccessService accessService,
      IEventPublisher eventPublisher)
    {
      this.context = context;
      this.accessService = accessService;
      this.eventPublisher = eventPublisher;
      this.topicRepo = this.context.GetRepository<Topic>();
      this.postRepo = this.context.GetRepository<Post>();
    }

    public Topic Create(
      ForumUser author,
      Forum forum,
      string subject,
      TopicType type,
      string body,
      string authorIP,
      string userAgent,
      string forumUrl,
      List<string> feedbackOutput)
    {
      Topic topic = (Topic) null;
      AccessFlag accessFlag = this.accessService.GetAccessFlag(forum, author);
      if ((accessFlag & AccessFlag.Post) == AccessFlag.Post)
      {
        topic = new Topic(author, forum, subject.Replace("<", "&gt;"));
        if ((accessFlag & AccessFlag.Moderator) == AccessFlag.Moderator && type == TopicType.Announcement | type == TopicType.Sticky)
        {
          switch (type)
          {
            case TopicType.Sticky:
              topic.Type = TopicType.Sticky;
              break;
            case TopicType.Announcement:
              topic.Type = TopicType.Announcement;
              break;
          }
        }
        this.topicRepo.Create(topic);
        this.context.SaveChanges();
        this.postRepo.Create(new Post(author, topic, topic.Title, body, authorIP));
        this.context.SaveChanges();
        NewTopicEvent payload = new NewTopicEvent()
        {
          TopicId = topic.Id,
          ForumId = topic.Forum.Id,
          UserAgent = userAgent,
          ForumRelativeURL = forumUrl
        };
        this.eventPublisher.Publish<NewTopicEvent>(payload);
        IEnumerable<EventFeedback> feedback = payload.GetFeedback();
        if (feedback.Any<EventFeedback>())
        {
          foreach (EventFeedback eventFeedback in feedback)
            feedbackOutput.Add(ForumHelper.GetHtmlString(eventFeedback.TextKey, eventFeedback.Namespace, eventFeedback.Arguments).ToString());
        }
      }
      return topic;
    }

    public Topic Read(ForumUser user, int id)
    {
      Topic topic = this.topicRepo.Read(id);
      if ((this.accessService.GetAccessFlag(topic.Forum, user) & AccessFlag.Read) == AccessFlag.Read)
        return topic;
      return (Topic) null;
    }

    public bool Update(ForumUser user, Topic topic, string title, string body, string forumURL)
    {
      return this.Update(user, topic, title, body, topic.Type, topic.Flag, string.Empty, forumURL);
    }

    public bool Update(
      ForumUser user,
      Topic topic,
      string title,
      string body,
      TopicType type,
      TopicFlag topicFlag,
      string reason,
      string forumURL)
    {
      Post post = topic.Posts.OrderBy<Post, int>((Func<Post, int>) (p => p.Position)).First<Post>();
      TopicFlag flag = topic.Flag;
      AccessFlag accessFlag = this.accessService.GetAccessFlag(topic.Forum, user);
      if (((accessFlag & AccessFlag.Edit) != AccessFlag.Edit || user.Id != topic.Author.Id) && (accessFlag & AccessFlag.Moderator) != AccessFlag.Moderator)
        return false;
      topic.Title = title;
      post.Update(user, title, body);
      if (user.Id != topic.Author.Id || (accessFlag & AccessFlag.Moderator) != AccessFlag.None)
      {
        topic.Type = type;
        topic.SetFlag(topicFlag);
        if ((topicFlag & TopicFlag.Deleted) == TopicFlag.Deleted)
          post.DeleteReason = reason;
        else
          post.Update(user, title, body, reason);
        if (flag != topic.Flag)
          this.eventPublisher.Publish<TopicFlagUpdatedEvent>(new TopicFlagUpdatedEvent()
          {
            TopicId = topic.Id,
            OriginalFlag = flag,
            ForumRelativeURL = forumURL
          });
      }
      this.context.SaveChanges();
      return true;
    }
  }
}
