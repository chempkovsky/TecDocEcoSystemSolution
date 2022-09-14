// Decompiled with JetBrains decompiler
// Type: mvcForum.Web.Areas.ForumAPI.Controllers.TopicController
// Assembly: mvcForum.Web, Version=1.4.5.40417, Culture=neutral, PublicKeyToken=null
// MVID: BA18DE97-F165-447C-8066-B8093E68A75C
// Assembly location: C:\Development\WebCarShop\mvcForum.Web.dll

using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces.Data;
using mvcForum.Web.Areas.ForumAPI.ViewModels;
using mvcForum.Web.Controllers;
using mvcForum.Web.Extensions;
using System;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI.Controllers
{
  public class TopicController : BaseAPIController
  {
    private readonly IPostRepository postRepo;
    private readonly IConfiguration config;

    public TopicController(IPostRepository postRepo, IConfiguration config)
    {
      this.postRepo = postRepo;
      this.config = config;
    }

    public ActionResult Read(int id, string mode)
    {
      mvcForum.Core.Topic topic1 = this.GetRepository<mvcForum.Core.Topic>().Read(id);
      if (!topic1.Forum.HasAccess(AccessFlag.Read))
        return (ActionResult) new HttpStatusCodeResult(403, "Access is denied");
      mvcForum.Web.Areas.ForumAPI.ViewModels.Topic topic2 = new mvcForum.Web.Areas.ForumAPI.ViewModels.Topic()
      {
        Id = topic1.Id,
        Title = topic1.Title,
        Author = topic1.AuthorName,
        AuthorId = topic1.Author.Id,
        Flag = topic1.Flag,
        Type = topic1.Type,
        PostCount = topic1.PostCount,
        ViewCount = topic1.ViewCount,
        Posted = topic1.Posted,
        LastPosted = topic1.LastPosted,
        AuthorApiUrl = this.Url.Action("read", "user", (object) new
        {
          mode = "json",
          area = "forumapi",
          id = topic1.Author.Id
        })
      };
      if (topic1.LastPost != null)
      {
        topic2.LastPostAuthor = topic1.LastPostUsername;
        topic2.LastPostAuthorId = new int?(topic1.LastPostAuthor.Id);
        topic2.LastPostAuthorApiUrl = this.Url.Action("read", "user", (object) new
        {
          mode = "json",
          area = "forumapi",
          id = topic2.LastPostAuthorId
        });
      }
      return (ActionResult) this.CustomJson((object) topic2);
    }

    public ActionResult List(int id, string mode, int? page)
    {
      mvcForum.Core.Topic topic = this.GetRepository<mvcForum.Core.Topic>().Read(id);
      if (!topic.Forum.HasAccess(AccessFlag.Read))
        return (ActionResult) new HttpStatusCodeResult(403, "Access is denied");
      topic.Forum.HasAccess(AccessFlag.Moderator);
      return (ActionResult) this.CustomJson((object) new
      {
        Posts = this.postRepo.Read(this.ActiveUser, topic, page.HasValue ? page.Value : 1, this.config.MessagesPerPage, this.config.ShowDeletedMessages).Select<mvcForum.Core.Post, PostLight>((Func<mvcForum.Core.Post, PostLight>) (p => new PostLight()
        {
          Id = p.Id,
          Subject = p.Subject,
          Flag = p.Flag,
          Posted = p.Posted,
          Position = p.Position,
          ApiUrl = this.Url.Action("read", "post", (object) new
          {
            mode = "json",
            area = "forumapi",
            id = p.Id
          })
        }))
      });
    }
  }
}
