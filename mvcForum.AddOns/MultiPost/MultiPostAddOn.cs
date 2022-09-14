// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.MultiPost.MultiPostAddOn
// Assembly: mvcForum.AddOns, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: 987259CA-A2FC-47C9-8492-5A5AEDD0BEF3
// Assembly location: C:\Development\WebCarShop\mvcForum.AddOns.dll

using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using ApplicationBoilerplate.Logging;
using mvcForum.Core;
using mvcForum.Core.Events;
using mvcForum.Core.Interfaces.AddOns;
using mvcForum.Core.Specifications;
using SimpleLocalisation;
using System;
using System.Linq;

namespace mvcForum.AddOns.MultiPost
{
  public class MultiPostAddOn : IAntiSpamAddOn, IAddOn, IEventListener<NewPostEvent>, IEventListener<NewTopicEvent>, IEventListener
  {
    private readonly MultiPostConfiguration addOnConfig;
    private readonly ILogger logger;
    private readonly IRepository<Post> postRepo;
    private readonly IRepository<Topic> topicRepo;
    private readonly IContext context;
    private readonly TextManager textManager;

    public MultiPostAddOn(
      MultiPostConfiguration addOnConfig,
      ILogger logger,
      IRepository<Post> postRepo,
      IRepository<Topic> topicRepo,
      IContext context,
      TextManager textManager)
    {
      this.addOnConfig = addOnConfig;
      this.logger = logger;
      this.postRepo = postRepo;
      this.topicRepo = topicRepo;
      this.context = context;
      this.textManager = textManager;
    }

    public void Handle(object payload)
    {
      if (!this.addOnConfig.Enabled)
        return;
      if (payload is NewTopicEvent)
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

    public void Handle(NewPostEvent payload)
    {
      if (!this.addOnConfig.Enabled)
        return;
      Post post = this.postRepo.Read(payload.PostId);
      if ((post.Flag & PostFlag.Deleted) != PostFlag.None || (post.Flag & PostFlag.Quarantined) != PostFlag.None || !this.ShouldQuarantine(post.Author, (InformationBearingEvent) payload))
        return;
      post.SetFlag(PostFlag.Quarantined);
      this.MarkAsSpam(post, (InformationBearingEvent) payload);
      this.context.SaveChanges();
    }

    private void MarkAsSpam(Post post, InformationBearingEvent payload)
    {
      Post post1 = post;
      string editReason = post1.EditReason;
      string newLine = Environment.NewLine;
      TextManager textManager = this.textManager;
      string str1 = "mvcForum.AddOns.MultiPost";
      object values = (object) new
      {
        Interval = this.addOnConfig.Interval,
        Posts = this.addOnConfig.Posts
      };
      string ns = str1;
      string str2 = textManager.Get("QuarantineReason", values, ns);
      post1.EditReason = editReason + newLine + str2;
      payload.AddFeedback("QuarantineFeedback", "mvcForum.AddOns.MultiPost", (object) null);
    }

    public void Handle(NewTopicEvent payload)
    {
      if (!this.addOnConfig.Enabled)
        return;
      Topic topic = this.topicRepo.Read(payload.TopicId);
      if ((topic.Flag & TopicFlag.Deleted) != TopicFlag.None || (topic.Flag & TopicFlag.Quarantined) != TopicFlag.None || !this.ShouldQuarantine(topic.Author, (InformationBearingEvent) payload))
        return;
      topic.SetFlag(TopicFlag.Quarantined);
      this.MarkAsSpam(topic.Posts.OrderBy<Post, DateTime>((Func<Post, DateTime>) (p => p.Posted)).First<Post>(), (InformationBearingEvent) payload);
      this.context.SaveChanges();
    }

    private bool ShouldQuarantine(ForumUser author, InformationBearingEvent e)
    {
      bool flag = false;
      if (this.addOnConfig.Posts > 0 && this.addOnConfig.Interval > 0)
        flag = this.postRepo.ReadMany((ISpecification<Post>) new PostSpecifications.RecentlyByAuthor(author, this.addOnConfig.Interval)).Count<Post>() > this.addOnConfig.Posts;
      return flag;
    }

    public bool CanRunAsync
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
        return 125;
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
