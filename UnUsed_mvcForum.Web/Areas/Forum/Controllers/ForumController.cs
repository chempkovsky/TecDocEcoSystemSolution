// mvcForum.Web.Areas.Forum.Controllers.ForumController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces.Data;
using mvcForum.Core.Specifications;
using mvcForum.Web;
using mvcForum.Web.Areas.Forum.Controllers;
using mvcForum.Web.Controllers;
using mvcForum.Web.Extensions;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web.Areas.Forum.Controllers
{

    public class ForumController : ThemedForumBaseController
    {
        private readonly IConfiguration config;

        private readonly ITopicRepository topicRepo;

        private readonly IForumRepository forumRepo;

        public ForumController(IConfiguration config, IWebUserProvider userProvider, IContext context, ITopicRepository topicRepo, IForumRepository forumRepo)
            : base(userProvider, context)
        {
            this.config = config;
            this.topicRepo = topicRepo;
            this.forumRepo = forumRepo;
        }

        public ActionResult Index(int id, string title, int? page)
        {
            mvcForum.Core.Forum forum = forumRepo.ReadOneOptimized(new ForumSpecifications.ById(id));
            if (forum == null)
            {
                return new HttpStatusCodeResult(404);
            }
            if (title != forum.Name.ToSlug())
            {
                return RedirectPermanent(base.Url.RouteUrl("ShowForum", new
                {
                    area = "forum",
                    title = forum.Name.ToSlug(),
                    id = forum.Id,
                    page = ((!page.HasValue) ? 1 : page.Value)
                }));
            }
            ForumViewModel forumViewModel = new ForumViewModel(forum, config.TopicsPerPage);
            forumViewModel.Paging.Page = ((!page.HasValue) ? 1 : page.Value);
            if (forum.HasAccess(AccessFlag.Read))
            {
                forum.Track();
                bool flag = forum.HasAccess(AccessFlag.Moderator);
                flag = false;
                IList<Topic> source = topicRepo.ReadTopics(forum, forumViewModel.Paging.Page, base.Authenticated ? base.ActiveUser : null, flag);
                forumViewModel.Topics = from x in source
                                        select new TopicViewModel(x, new MessageViewModel[0], x.Posts.Visible(config).Count() - 1, config.MessagesPerPage, showDeleted: false);
                forumViewModel.SubForums = from x in forum.SubForums
                                           select new ForumViewModel(x, config.TopicsPerPage);
                forumViewModel.Path = new Dictionary<string, string>();
                HomeController.BuildPath(forum, forumViewModel.Path, base.Url);
                return View(forumViewModel);
            }
            base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.Forum", new
            {
                forum.Name
            }));
            return RedirectToRoute("NoAccess");
        }

        [Authorize]
        [ActionName("Mark As Read")]
        public ActionResult MarkAsRead(int id)
        {
            mvcForum.Core.Forum forum = GetRepository<mvcForum.Core.Forum>().Read(id);
            if (forum.HasAccess(AccessFlag.Read))
            {
                foreach (Topic topic in forum.Topics)
                {
                    topic.Track();
                }
            }
            return RedirectToAction("index", new
            {
                area = "forum",
                id = id,
                title = forum.Name.ToSlug()
            });
        }

        [Authorize]
        public ActionResult Follow(int forumId)
        {
            mvcForum.Core.Forum forum = GetRepository<mvcForum.Core.Forum>().Read(forumId);
            if (base.Authenticated)
            {
                FollowForum newEntity = new FollowForum(forum, base.ActiveUser);
                GetRepository<FollowForum>().Create(newEntity);
                base.Context.SaveChanges();
            }
            return RedirectToRoute("ShowForum", new RouteValueDictionary
        {
            {
                "id",
                forum.Id
            },
            {
                "title",
                forum.Name.ToSlug()
            }
        });
        }

        [Authorize]
        public ActionResult UnFollow(int forumId)
        {
            mvcForum.Core.Forum forum = GetRepository<mvcForum.Core.Forum>().Read(forumId);
            if (base.Authenticated)
            {
                IRepository<FollowForum> repository = GetRepository<FollowForum>();
                FollowForum followForum = repository.ReadOne(new FollowForumSpecifications.SpecificForumAndUser(forum, base.ActiveUser));
                if (followForum != null)
                {
                    repository.Delete(followForum);
                    base.Context.SaveChanges();
                }
            }
            return RedirectToRoute("ShowForum", new RouteValueDictionary
        {
            {
                "id",
                forum.Id
            },
            {
                "title",
                forum.Name.ToSlug()
            }
        });
        }
    }

}