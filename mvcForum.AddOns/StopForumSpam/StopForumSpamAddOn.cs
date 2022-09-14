// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.StopForumSpam.StopForumSpamAddOn
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using ApplicationBoilerplate.Logging;
using CreativeMinds.StopForumSpam;
using CreativeMinds.StopForumSpam.Responses;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Events;
using mvcForum.Core.Interfaces.AddOns;
using SimpleLocalisation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mvcForum.AddOns.StopForumSpam
{
  public class StopForumSpamAddOn : IAntiSpamAddOn, IAddOn, IAsyncEventListener<NewPostEvent>, IEventListener<NewPostEvent>, IAsyncEventListener<NewTopicEvent>, IEventListener<NewTopicEvent>, IAsyncEventListener<PostUpdatedEvent>, IEventListener<PostUpdatedEvent>, IAsyncEventListener<TopicUpdatedEvent>, IEventListener<TopicUpdatedEvent>, IEventListener<NewUserEvent>, IEventListener
  {
    private readonly IConfiguration config;
    private readonly StopForumSpamConfiguration addOnConfig;
    private readonly ILogger logger;
    private readonly IContext context;
    private readonly IRepository<Post> postRepo;
    private readonly IRepository<Topic> topicRepo;
    private readonly IAsyncTask task;
    private readonly TextManager textManager;

    public StopForumSpamAddOn(
      StopForumSpamConfiguration addOnConfig,
      IConfiguration config,
      ILogger logger,
      IContext context,
      IRepository<Post> postRepo,
      IRepository<Topic> topicRepo,
      IAsyncTask task,
      TextManager textManager)
    {
      this.config = config;
      this.addOnConfig = addOnConfig;
      this.logger = logger;
      this.context = context;
      this.postRepo = postRepo;
      this.topicRepo = topicRepo;
      this.task = task;
      this.textManager = textManager;
    }

    public void Queue(TopicUpdatedEvent payload)
    {
      this.Enqueue((object) payload);
    }

    public void Queue(PostUpdatedEvent payload)
    {
      this.Enqueue((object) payload);
    }

    public void Queue(NewTopicEvent payload)
    {
      this.Enqueue((object) payload);
    }

    public void Queue(NewPostEvent payload)
    {
      this.Enqueue((object) payload);
    }

    private void Enqueue(object payload)
    {
      if (!this.addOnConfig.Enabled || !this.addOnConfig.MarkAsSpamOnHit)
        return;
      this.task.Execute((IEventListener) this, payload, this.addOnConfig.Delay);
    }

    public void Handle(object payload)
    {
      if (!this.addOnConfig.Enabled || !this.addOnConfig.MarkAsSpamOnHit)
        return;
      if (payload is TopicUpdatedEvent)
        this.Handle((TopicUpdatedEvent) payload);
      else if (payload is PostUpdatedEvent)
        this.Handle((PostUpdatedEvent) payload);
      else if (payload is NewTopicEvent)
      {
        this.Handle((NewTopicEvent) payload);
      }
      else
      {
        if (!(payload is NewPostEvent))
          throw new ApplicationException("Unknown payload!");
        this.Handle((NewPostEvent) payload);
      }
    }

    public void Handle(TopicUpdatedEvent payload)
    {
      if (!this.addOnConfig.Enabled || !this.addOnConfig.MarkAsSpamOnHit)
        return;
      Topic topic = this.topicRepo.Read(payload.TopicId);
      if ((topic.Flag & TopicFlag.Deleted) != TopicFlag.None || (topic.Flag & TopicFlag.Quarantined) != TopicFlag.None || (!this.IsSpam(topic.Posts.OrderBy<Post, DateTime>((Func<Post, DateTime>) (p => p.Posted)).First<Post>()) || !this.addOnConfig.MarkAsSpamOnHit))
        return;
      topic.SetFlag(TopicFlag.Quarantined);
      this.MarkAsSpam(topic.Posts.OrderBy<Post, DateTime>((Func<Post, DateTime>) (p => p.Posted)).First<Post>(), (InformationBearingEvent) payload);
      this.context.SaveChanges();
    }

    public void Handle(NewTopicEvent payload)
    {
      if (!this.addOnConfig.Enabled || !this.addOnConfig.MarkAsSpamOnHit)
        return;
      Topic topic = this.topicRepo.Read(payload.TopicId);
      if ((topic.Flag & TopicFlag.Deleted) != TopicFlag.None || (topic.Flag & TopicFlag.Quarantined) != TopicFlag.None || (!this.IsSpam(topic.Posts.OrderBy<Post, DateTime>((Func<Post, DateTime>) (p => p.Posted)).First<Post>()) || !this.addOnConfig.MarkAsSpamOnHit))
        return;
      topic.SetFlag(TopicFlag.Quarantined);
      this.MarkAsSpam(topic.Posts.OrderBy<Post, DateTime>((Func<Post, DateTime>) (p => p.Posted)).First<Post>(), (InformationBearingEvent) payload);
      this.context.SaveChanges();
    }

    public void Handle(PostUpdatedEvent payload)
    {
      if (!this.addOnConfig.Enabled || !this.addOnConfig.MarkAsSpamOnHit)
        return;
      Post post = this.postRepo.Read(payload.PostId);
      if ((post.Flag & PostFlag.Deleted) != PostFlag.None || (post.Flag & PostFlag.Quarantined) != PostFlag.None || (!this.IsSpam(post) || !this.addOnConfig.MarkAsSpamOnHit))
        return;
      post.SetFlag(PostFlag.Quarantined);
      this.MarkAsSpam(post, (InformationBearingEvent) payload);
      this.context.SaveChanges();
    }

    public void Handle(NewPostEvent payload)
    {
      if (!this.addOnConfig.Enabled || !this.addOnConfig.MarkAsSpamOnHit)
        return;
      Post post = this.postRepo.Read(payload.PostId);
      if ((post.Flag & PostFlag.Deleted) != PostFlag.None || (post.Flag & PostFlag.Quarantined) != PostFlag.None || (!this.IsSpam(post) || !this.addOnConfig.MarkAsSpamOnHit))
        return;
      post.SetFlag(PostFlag.Quarantined);
      this.MarkAsSpam(post, (InformationBearingEvent) payload);
      this.context.SaveChanges();
    }

    public void Handle(NewUserEvent payload)
    {
      if (!this.addOnConfig.CheckNewUsers || payload.Bot)
        return;
      Response response = new Client(this.addOnConfig.Key).Check(payload.Username, payload.EmailAddress, payload.IPAddress);
      if (!response.Success)
        return;
      payload.Bot = ((IEnumerable<ResponsePart>) response.ResponseParts).Where<ResponsePart>((Func<ResponsePart, bool>) (rp => rp.Appears > 0)).Any<ResponsePart>();
    }

    private void MarkAsSpam(Post post, InformationBearingEvent payload)
    {
      Post post1 = post;
      post1.EditReason = post1.EditReason + Environment.NewLine + this.textManager.Get("QuarantineReason", (object) null, "mvcForum.AddOns.StopForumSpam");
      payload.AddFeedback("QuarantineFeedback", "mvcForum.AddOns.StopForumSpam", (object) null);
    }

    private bool IsSpam(Post post)
    {
      return ((IEnumerable<ResponsePart>) new Client(this.addOnConfig.Key).Check(post.Author.Name, post.Author.EmailAddress, post.IP).ResponseParts).Where<ResponsePart>((Func<ResponsePart, bool>) (rp => rp.Appears > 0)).Any<ResponsePart>();
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
        return 50;
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
