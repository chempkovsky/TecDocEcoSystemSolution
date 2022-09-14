// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.Controllers.ForumController
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces.Data;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Core.Specifications;
using mvcForum.Web.Areas.ForumAPI.ViewModels;
using mvcForum.Web.Controllers;
using mvcForum.Web.Events;
using mvcForum.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI.Controllers
{
  public class ForumController : BaseAPIController
  {
    private readonly ITopicRepository topicRepo;
    private readonly IMembershipService membershipService;

    public ForumController(ITopicRepository topicRepo, IMembershipService membershipService)
    {
      this.topicRepo = topicRepo;
      this.membershipService = membershipService;
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    [HttpPost]
    public ActionResult Create(mvcForum.Web.Areas.ForumAPI.ViewModels.Forum model, string mode)
    {
      if (this.ModelState.IsValid)
      {
        if (model.CategoryId > 0)
        {
          IRepository<mvcForum.Core.Forum> repository1 = this.GetRepository<mvcForum.Core.Forum>();
          IRepository<mvcForum.Core.Category> repository2 = this.GetRepository<mvcForum.Core.Category>();
          mvcForum.Core.Forum forum1 = (mvcForum.Core.Forum) null;
          mvcForum.Core.Forum forum2;
          if (model.ParentForumId.HasValue)
          {
            forum1 = repository1.Read(model.ParentForumId.Value);
            forum2 = new mvcForum.Core.Forum(forum1.Category, model.Name, model.SortOrder, model.Description, forum1);
          }
          else
            forum2 = new mvcForum.Core.Forum(repository2.Read(model.CategoryId), model.Name, model.SortOrder, model.Description);
          repository1.Create(forum2);
          this.Context.SaveChanges();
          if (forum1 != null)
          {
            IRepository<mvcForum.Core.ForumAccess> repository3 = this.GetRepository<mvcForum.Core.ForumAccess>();
            foreach (mvcForum.Core.ForumAccess forumAccess in repository3.ReadMany((ISpecification<mvcForum.Core.ForumAccess>) new ForumAccessSpecifications.SpecificForum(forum1)))
              repository3.Create(new mvcForum.Core.ForumAccess(forum2, forumAccess.Group, forumAccess.AccessMask));
            this.Context.SaveChanges();
          }
          return (ActionResult) new HttpStatusCodeResult(200);
        }
        this.ModelState.AddModelError("", "");
      }
      return (ActionResult) new HttpStatusCodeResult(500, this.ModelState.ErrorString());
    }

    public ActionResult Read(int id, string mode)
    {
      mvcForum.Core.Forum forum1 = this.GetRepository<mvcForum.Core.Forum>().Read(id);
      if (!((IEnumerable<string>) this.membershipService.GetRolesForAccount(this.membershipService.GetAccount((object) this.userProvider.ActiveUser.ProviderId).AccountName)).Contains<string>("Solution Administrator") && !forum1.HasAccess(AccessFlag.Read))
        return (ActionResult) new HttpStatusCodeResult(403, "Access is denied");
      mvcForum.Web.Areas.ForumAPI.ViewModels.Forum forum2 = new mvcForum.Web.Areas.ForumAPI.ViewModels.Forum()
      {
        Id = forum1.Id,
        Name = forum1.Name,
        Description = forum1.Description,
        SortOrder = forum1.SortOrder,
        CategoryId = forum1.Category.Id,
        CategoryUrl = this.Url.Action("read", "category", (object) new
        {
          mode = "json",
          area = "forumapi",
          id = forum1.Category.Id
        })
      };
      if (forum1.ParentForum != null)
      {
        forum2.ParentForumId = new int?(forum1.ParentForum.Id);
        forum2.ParentForumUrl = this.Url.Action("read", "forum", (object) new
        {
          mode = "json",
          area = "forumapi",
          id = forum2.ParentForumId
        });
      }
      return (ActionResult) this.CustomJson((object) forum2);
    }

    [HttpPost]
    public ActionResult Update(mvcForum.Web.Areas.ForumAPI.ViewModels.Forum model, string mode)
    {
      if (this.ModelState.IsValid)
      {
        if (model.Id > 0)
        {
          mvcForum.Core.Forum forum = this.GetRepository<mvcForum.Core.Forum>().Read(model.Id);
          if (!((IEnumerable<string>) this.membershipService.GetRolesForAccount(this.membershipService.GetAccount((object) this.userProvider.ActiveUser.ProviderId).AccountName)).Contains<string>("Solution Administrator") && !forum.HasAccess(AccessFlag.Read))
            return (ActionResult) new HttpStatusCodeResult(403, "Access is denied");
          forum.Name = model.Name;
          forum.SortOrder = model.SortOrder;
          forum.Description = model.Description;
          this.Context.SaveChanges();
          return (ActionResult) new HttpStatusCodeResult(200);
        }
        this.ModelState.AddModelError("", "");
      }
      return (ActionResult) new HttpStatusCodeResult(500, this.ModelState.ErrorString());
    }

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public ActionResult Delete(int id, string mode, bool deleteAll)
    {
      IRepository<mvcForum.Core.Forum> repository = this.GetRepository<mvcForum.Core.Forum>();
      mvcForum.Core.Forum forum = repository.Read(id);
      mvcForum.Core.Forum parentForum = forum.ParentForum;
      if (parentForum != null)
      {
        forum.LastPostId = new int?();
        forum.LastPostUserId = new int?();
        forum.LastPostUsername = (string) null;
        forum.LastPosted = new DateTime?();
        forum.LastTopicId = new int?();
        this.context.SaveChanges();
      }
      this.DeleteForum(forum, repository, deleteAll);
      if (parentForum != null)
        NewAndUpdatedContentEventListener.UpdateForum(parentForum, this.context);
      return (ActionResult) new HttpStatusCodeResult(200);
    }

    private void DeleteForum(mvcForum.Core.Forum forum, IRepository<mvcForum.Core.Forum> forumRepo, bool deleteAll)
    {
      if (deleteAll)
      {
        forum.LastPostId = new int?();
        forum.LastPostUserId = new int?();
        forum.LastPostUsername = (string) null;
        forum.LastPosted = new DateTime?();
        forum.LastTopicId = new int?();
        this.context.SaveChanges();
      }
      if (forum.SubForums.Count > 0)
      {
        foreach (mvcForum.Core.Forum forum1 in new List<mvcForum.Core.Forum>((IEnumerable<mvcForum.Core.Forum>) forum.SubForums))
          this.DeleteForum(forum1, forumRepo, deleteAll);
      }
      if (deleteAll)
      {
        IRepository<FollowForum> repository1 = this.GetRepository<FollowForum>();
        foreach (FollowForum followForum in repository1.ReadMany((ISpecification<FollowForum>) new FollowForumSpecifications.SpecificForum(forum)).ToList<FollowForum>())
          repository1.Delete(followForum.Id);
        IRepository<ForumTrack> repository2 = this.GetRepository<ForumTrack>();
        foreach (ForumTrack forumTrack in repository2.ReadMany((ISpecification<ForumTrack>) new ForumTrackSpecifications.SpecificForum(forum)).ToList<ForumTrack>())
          repository2.Delete(forumTrack.Id);
        IRepository<mvcForum.Core.ForumAccess> repository3 = this.GetRepository<mvcForum.Core.ForumAccess>();
        foreach (mvcForum.Core.ForumAccess forumAccess in repository3.ReadMany((ISpecification<mvcForum.Core.ForumAccess>) new ForumAccessSpecifications.SpecificForum(forum)).ToList<mvcForum.Core.ForumAccess>())
          repository3.Delete(forumAccess.Id);
        this.Context.SaveChanges();
        IRepository<mvcForum.Core.Topic> repository4 = this.GetRepository<mvcForum.Core.Topic>();
        foreach (mvcForum.Core.Topic topic in new List<mvcForum.Core.Topic>((IEnumerable<mvcForum.Core.Topic>) forum.Topics))
        {
          topic.LastPostId = new int?();
          topic.LastPostUsername = (string) null;
          topic.LastPostAuthorId = new int?();
          IRepository<FollowTopic> repository5 = this.GetRepository<FollowTopic>();
          foreach (FollowTopic followTopic in repository5.ReadMany((ISpecification<FollowTopic>) new FollowTopicSpecifications.SpecificTopic(topic)).ToList<FollowTopic>())
            repository5.Delete(followTopic.Id);
          IRepository<TopicTrack> repository6 = this.GetRepository<TopicTrack>();
          foreach (TopicTrack topicTrack in repository6.ReadMany((ISpecification<TopicTrack>) new TopicTrackSpecifications.SpecificTopic(topic)).ToList<TopicTrack>())
            repository6.Delete(topicTrack.Id);
          this.context.SaveChanges();
          IRepository<mvcForum.Core.Post> repository7 = this.GetRepository<mvcForum.Core.Post>();
          IEnumerable<mvcForum.Core.Post> posts = (IEnumerable<mvcForum.Core.Post>) new List<mvcForum.Core.Post>((IEnumerable<mvcForum.Core.Post>) topic.Posts).OrderByDescending<mvcForum.Core.Post, DateTime>((Func<mvcForum.Core.Post, DateTime>) (p => p.Posted));
          IRepository<PostReport> repository8 = this.GetRepository<PostReport>();
          foreach (mvcForum.Core.Post post in posts)
          {
            foreach (PostReport postReport in repository8.ReadMany((ISpecification<PostReport>) new PostReportSpecifications.SpecificPost(post)).ToList<PostReport>())
              repository8.Delete(postReport.Id);
            this.context.SaveChanges();
            repository7.Delete(post);
            this.context.SaveChanges();
          }
          repository4.Delete(topic.Id);
        }
        this.context.SaveChanges();
      }
      forumRepo.Delete(forum);
      this.Context.SaveChanges();
    }

    public ActionResult List(int id, string mode)
    {
      mvcForum.Core.Forum forum = this.GetRepository<mvcForum.Core.Forum>().Read(id);
      if (!((IEnumerable<string>) this.membershipService.GetRolesForAccount(this.membershipService.GetAccountByName(this.userProvider.ActiveUser.Name).AccountName)).Contains<string>("Solution Administrator") && !forum.HasAccess(AccessFlag.Read))
        return (ActionResult) new HttpStatusCodeResult(403, "Access is denied");
      return (ActionResult) this.CustomJson((object) new
      {
        Forums = forum.SubForums.Select<mvcForum.Core.Forum, ForumLight>((Func<mvcForum.Core.Forum, ForumLight>) (f => new ForumLight()
        {
          Id = f.Id,
          Name = f.Name,
          SortOrder = f.SortOrder,
          ApiUrl = this.Url.Action("read", "forum", (object) new
          {
            mode = "json",
            area = "forumapi",
            id = f.Id
          })
        })).OrderBy<ForumLight, int>((Func<ForumLight, int>) (f => f.SortOrder))
      });
    }

    public ActionResult ListLatestTopics(string mode, int id, int? count)
    {
      int count1 = 5;
      if (count.HasValue)
        count1 = count.Value;
      IEnumerable<mvcForum.Core.Topic> topics = (IEnumerable<mvcForum.Core.Topic>) new List<mvcForum.Core.Topic>();
      mvcForum.Core.Forum forum = this.GetRepository<mvcForum.Core.Forum>().Read(id);
      if (forum == null || !forum.HasAccess(AccessFlag.Read))
        return (ActionResult) new HttpStatusCodeResult(404, "Not Found");
      IQueryable<mvcForum.Core.Topic> source = this.GetRepository<mvcForum.Core.Topic>().ReadAll().Where<mvcForum.Core.Topic>((Func<mvcForum.Core.Topic, bool>) (t => t.ForumId == forum.Id)).AsQueryable<mvcForum.Core.Topic>().Where<mvcForum.Core.Topic>(new TopicSpecifications.Visible().IsSatisfied);
      Expression<Func<mvcForum.Core.Topic, DateTime>> keySelector = (Expression<Func<mvcForum.Core.Topic, DateTime>>) (t => t.Posted);
      return (ActionResult) this.CustomJson((object) new
      {
        Topics = source.OrderByDescending<mvcForum.Core.Topic, DateTime>(keySelector).Take<mvcForum.Core.Topic>(count1).Select<mvcForum.Core.Topic, TopicLight>((Func<mvcForum.Core.Topic, TopicLight>) (t => new TopicLight()
        {
          Id = t.Id,
          Flag = t.Flag,
          Title = t.Title,
          Posted = t.Posted,
          Type = t.Type,
          Url = this.Url.RouteUrl("ShowTopic", (object) new
          {
            Title = t.Title.ToSlug(),
            Id = t.Id
          }),
          ApiUrl = this.Url.Action("read", "topic", (object) new
          {
            mode = "json",
            area = "forumapi",
            id = t.Id
          }),
          Author = t.AuthorName
        }))
      });
    }
  }
}
