// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Services.PostService
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
  public class PostService : IPostService
  {
    private readonly IContext context;
    private readonly IForumAccessService accessService;
    private readonly IEventPublisher eventPublisher;
    private readonly IRepository<Post> postRepo;

    public PostService(
      IContext context,
      IForumAccessService accessService,
      IEventPublisher eventPublisher)
    {
      this.context = context;
      this.accessService = accessService;
      this.eventPublisher = eventPublisher;
      this.postRepo = this.context.GetRepository<Post>();
    }

    public Post Create(
      ForumUser author,
      Topic topic,
      string subject,
      string body,
      string authorIP,
      string userAgent,
      string topicUrl,
      List<string> feedbackOutput,
      Post replyTo)
    {
      Post post = (Post) null;
      if ((this.accessService.GetAccessFlag(topic.Forum, author) & AccessFlag.Post) == AccessFlag.Post)
      {
        post = replyTo == null || replyTo.Topic != topic ? new Post(author, topic, subject.Replace("<", "&gt;"), body, authorIP) : new Post(author, topic, subject.Replace("<", "&gt;"), body, authorIP, replyTo);
        post.Position = post.Topic.Posts.Where<Post>((Func<Post, bool>) (p => p.Posted < post.Posted)).Count<Post>();
        this.postRepo.Create(post);
        this.context.SaveChanges();
        NewPostEvent payload = new NewPostEvent()
        {
          PostId = post.Id,
          TopicId = topic.Id,
          ForumId = topic.Forum.Id,
          UserAgent = userAgent,
          TopicRelativeURL = topicUrl
        };
        this.eventPublisher.Publish<NewPostEvent>(payload);
        IEnumerable<EventFeedback> feedback = payload.GetFeedback();
        if (feedback.Any<EventFeedback>())
        {
          foreach (EventFeedback eventFeedback in feedback)
            feedbackOutput.Add(ForumHelper.GetHtmlString(eventFeedback.TextKey, eventFeedback.Namespace, eventFeedback.Arguments).ToString());
        }
      }
      return post;
    }

    public Post Create(
      ForumUser author,
      Topic topic,
      string subject,
      string body,
      string authorIP,
      string userAgent,
      string topicUrl,
      List<string> feedbackOutput)
    {
      return this.Create(author, topic, subject, body, authorIP, userAgent, topicUrl, feedbackOutput, (Post) null);
    }
  }
}
