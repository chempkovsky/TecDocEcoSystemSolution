// Decompiled with JetBrains decompiler
// Type: mvcForum.AddOns.NewUserTrial.NewUserTrialAddOn
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

namespace mvcForum.AddOns.NewUserTrial
{
  public class NewUserTrialAddOn : IAntiSpamAddOn, IAddOn, IEventListener<NewPostEvent>, IEventListener<NewTopicEvent>, IEventListener
  {
    private readonly NewUserTrialConfiguration addOnConfig;
    private readonly ILogger logger;
    private readonly IRepository<Post> postRepo;
    private readonly IRepository<Topic> topicRepo;
    private readonly IRepository<GroupMember> gmRepo;
    private readonly IContext context;
    private readonly IAsyncTask task;
    private readonly TextManager textManager;

    public NewUserTrialAddOn(
      NewUserTrialConfiguration addOnConfig,
      ILogger logger,
      IRepository<Post> postRepo,
      IContext context,
      IAsyncTask task,
      IRepository<Topic> topicRepo,
      IRepository<GroupMember> gmRepo,
      TextManager textManager)
    {
      this.addOnConfig = addOnConfig;
      this.logger = logger;
      this.postRepo = postRepo;
      this.context = context;
      this.task = task;
      this.topicRepo = topicRepo;
      this.gmRepo = gmRepo;
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

    public void Handle(NewTopicEvent payload)
    {
      if (!this.addOnConfig.Enabled)
        return;
      Topic topic = this.topicRepo.Read(payload.TopicId);
      if ((topic.Flag & TopicFlag.Deleted) != TopicFlag.None || (topic.Flag & TopicFlag.Quarantined) != TopicFlag.None || !this.ShouldQuarantine(topic.Author, false, topic.Id, (InformationBearingEvent) payload))
        return;
      topic.SetFlag(TopicFlag.Quarantined);
      this.MarkAsSpam(topic.Posts.OrderBy<Post, DateTime>((Func<Post, DateTime>) (p => p.Posted)).First<Post>(), (InformationBearingEvent) payload);
      this.context.SaveChanges();
    }

    public void Handle(NewPostEvent payload)
    {
      if (!this.addOnConfig.Enabled)
        return;
      Post post = this.postRepo.Read(payload.PostId);
      if ((post.Flag & PostFlag.Deleted) != PostFlag.None || (post.Flag & PostFlag.Quarantined) != PostFlag.None || !this.ShouldQuarantine(post.Author, true, post.Id, (InformationBearingEvent) payload))
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
      string str1 = "mvcForum.AddOns.NewUserTrial";
      object values = (object) new
      {
        Limit = this.addOnConfig.AutoLimit
      };
      string ns = str1;
      string str2 = textManager.Get("QuarantineReason", values, ns);
      post1.EditReason = editReason + newLine + str2;
      payload.AddFeedback("QuarantineFeedback", "mvcForum.AddOns.NewUserTrial", (object) null);
    }

    private bool ShouldQuarantine(
      ForumUser author,
      bool checkPost,
      int id,
      InformationBearingEvent e)
    {
      bool flag1 = false;
      if (this.addOnConfig.AutoLimit > 0)
      {
        bool flag2 = false;
        if (this.addOnConfig.ExcludeGroups.Count<int>() > 0)
        {
          foreach (GroupMember groupMember in this.gmRepo.ReadMany((ISpecification<GroupMember>) new GroupMemberSpecifications.SpecificUser(author)))
          {
            if (this.addOnConfig.ExcludeGroups.Contains<int>(groupMember.Group.Id))
            {
              flag2 = true;
              break;
            }
          }
        }
        if (!flag2)
        {
          int num = !checkPost ? this.topicRepo.ReadMany((ISpecification<Topic>) new TopicSpecifications.AllVisibleByAuthor(author)).Where<Topic>((Func<Topic, bool>) (t => t.Id != id)).Count<Topic>() : this.postRepo.ReadMany((ISpecification<Post>) new PostSpecifications.AllVisibleByAuthor(author)).Where<Post>((Func<Post, bool>) (p => p.Id != id)).Count<Post>();
          flag1 = num < this.addOnConfig.AutoLimit;
          if (flag1)
            e.AddFeedback("QuarantineReason", "mvcForum.AddOns.NewUserTrial", (object) new
            {
              Posts = num,
              Limit = this.addOnConfig.AutoLimit
            });
        }
      }
      return flag1;
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
