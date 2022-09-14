// mvcForum.Web.Areas.Forum.Controllers.TopicController
using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces.Data;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Core.Specifications;
using mvcForum.Web;
using mvcForum.Web.Areas.Forum.Controllers;
using mvcForum.Web.Attributes;
using mvcForum.Web.Controllers;
using mvcForum.Web.Extensions;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using mvcForum.Web.ViewModels.Create;
using mvcForum.Web.ViewModels.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web.Areas.Forum.Controllers
{

    public class TopicController : ThemedForumBaseController
    {
        private readonly IConfiguration config;

        private readonly IEventPublisher eventPublisher;

        private readonly IPostService postService;

        private readonly IRepository<TopicTrack> ttRepo;

        private readonly IAttachmentService attachmentService;

        private readonly ITopicService topicService;

        private readonly IPostRepository postRepo;

        public TopicController(IWebUserProvider userProvider, IContext context, IConfiguration config, IEventPublisher eventPublisher, ITopicService topicService, IAttachmentService attachmentService, IPostService postService, IPostRepository postRepo)
            : base(userProvider, context)
        {
            this.config = config;
            this.eventPublisher = eventPublisher;
            ttRepo = base.context.GetRepository<TopicTrack>();
            this.postRepo = postRepo;
            this.attachmentService = attachmentService;
            this.topicService = topicService;
            this.postService = postService;
        }

        [Authorize]
        public ActionResult Create(int id)
        {
            mvcForum.Core.Forum forum = GetRepository<mvcForum.Core.Forum>().Read(id);
            if (forum.HasAccess(AccessFlag.Post))
            {
                CreateTopicViewModel createTopicViewModel = new CreateTopicViewModel(forum, config.TopicsPerPage);
                createTopicViewModel.Path = new Dictionary<string, string>();
                HomeController.BuildPath(forum, createTopicViewModel.Path, base.Url);
                createTopicViewModel.Path.Add("/", "New topic");
                return View(createTopicViewModel);
            }
            base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ForumPosting", new
            {
                forum.Name
            }));
            return RedirectToRoute("NoAccess");
        }

        [NotBanned]
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CreateTopicViewModel newTopic, HttpPostedFileBase[] files)
        {
            mvcForum.Core.Forum forum = GetRepository<mvcForum.Core.Forum>().Read(newTopic.ForumId);
            if (base.ModelState.IsValid)
            {
                List<string> list = new List<string>();
                Topic topic = topicService.Create(base.ActiveUser, forum, newTopic.Subject, newTopic.Type, newTopic.Body, base.Request.UserHostAddress, base.Request.UserAgent, base.Url.RouteUrl("ShowForum", new
                {
                    id = forum.Id,
                    title = forum.Name.ToSlug(),
                    area = "forum"
                }), list);
                if (topic != null)
                {
                    AccessFlag access = forum.GetAccess();
                    if ((access & AccessFlag.Upload) == AccessFlag.Upload)
                    {
                        if (files != null && files.Length > 0)
                        {
                            foreach (HttpPostedFileBase httpPostedFileBase in files)
                            {
                                if (httpPostedFileBase != null)
                                {
                                    AttachStatusCode attachStatusCode = attachmentService.AttachFile(base.ActiveUser, topic, httpPostedFileBase.FileName, httpPostedFileBase.ContentType, httpPostedFileBase.ContentLength, httpPostedFileBase.InputStream);
                                    if (attachStatusCode != 0)
                                    {
                                        list.Add(ForumHelper.GetString(attachStatusCode.ToString(), new
                                        {
                                            File = httpPostedFileBase.FileName,
                                            Size = httpPostedFileBase.ContentLength,
                                            MaxFileSize = config.MaxFileSize,
                                            MaxAttachmentsSize = config.MaxAttachmentsSize,
                                            Extensions = config.AllowedExtensions
                                        }, "mvcForum.Web.AttachmentErrors"));
                                    }
                                }
                            }
                        }
                        else if (newTopic.AttachFile)
                        {
                            return RedirectToAction("Attach", "File", new RouteValueDictionary
                        {
                            {
                                "id",
                                (from p in topic.Posts
                                orderby p.Posted
                                select p).First().Id
                            }
                        });
                        }
                    }
                    if (list.Any())
                    {
                        base.TempData.Add("Feedback", from f in list
                                                      select new MvcHtmlString(f));
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
            newTopic = new CreateTopicViewModel(forum, config.TopicsPerPage);
            newTopic.Path = new Dictionary<string, string>();
            HomeController.BuildPath(forum, newTopic.Path, base.Url);
            newTopic.Path.Add("/", "New topic");
            return View(newTopic);
        }

        public ActionResult Index(int id, string title, int? page, string additional)
        {
            Topic topic = topicService.Read(base.ActiveUser, id);
            if (topic != null)
            {
                if (title != topic.Title.ToSlug())
                {
                    return RedirectPermanent(base.Url.RouteUrl("ShowTopic", new
                    {
                        area = "forum",
                        title = topic.Title.ToSlug(),
                        id = topic.Id
                    }));
                }
                Post lastReadPost = null;
                IEnumerable<Post> source;
                if (!string.IsNullOrWhiteSpace(additional) && base.Authenticated && base.ActiveUser != null)
                {
                    source = postRepo.ReadSinceLast(base.ActiveUser, topic, config.MessagesPerPage, config.ShowDeletedMessages, out DateTime? lastRead, out int showingPage);
                    if (lastRead.HasValue)
                    {
                        lastReadPost = (from p in source
                                        where p.Posted > lastRead.Value
                                        orderby p.Posted
                                        select p).FirstOrDefault();
                    }
                    page = showingPage;
                }
                else
                {
                    source = postRepo.Read(base.ActiveUser, topic, (!page.HasValue) ? 1 : ((page.Value <= 0) ? 1 : page.Value), config.MessagesPerPage, config.ShowDeletedMessages);
                }
                topic.TrackAndView();
                TopicViewModel topicViewModel = new TopicViewModel(topic, from p in source
                                                                          select new MessageViewModel(p)
                                                                          {
                                                                              LastRead = (lastReadPost != null && ((p.Id == lastReadPost.Id) ? true : false)),
                                                                              Attachments = from a in p.Attachments
                                                                                            select new AttachmentViewModel(a)
                                                                          }, postRepo.ReadAll(base.ActiveUser, topic, config.ShowDeletedMessages).Count() - 1, config.MessagesPerPage, config.ShowDeletedMessages);
                topicViewModel.Page = ((!page.HasValue || page.Value <= 0) ? 1 : page.Value);
                topicViewModel.Path = new Dictionary<string, string>();
                HomeController.BuildPath(topic, topicViewModel.Path, base.Url);
                return View(topicViewModel);
            }
            topic = context.GetRepository<Topic>().Read(id);
            base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.Forum", new
            {
                topic.Forum.Name
            }));
            return RedirectToRoute("NoAccess");
        }

        [Authorize]
        public ActionResult Moderate(int id)
        {
            Topic topic = GetRepository<Topic>().Read(id);
            if (topic.Forum.HasAccess(AccessFlag.Moderator))
            {
                Dictionary<string, string> path = new Dictionary<string, string>();
                HomeController.BuildPath(topic, path, base.Url);
                return View(new UpdateTopicViewModel
                {
                    Id = topic.Id,
                    Title = topic.Title,
                    Body = (from p in topic.Posts
                            orderby p.Position
                            select p).First().Body,
                    Flag = topic.Flag,
                    Type = topic.Type,
                    Path = path,
                    IsModerator = topic.Forum.HasAccess(AccessFlag.Moderator),
                    Reason = (from p in topic.Posts
                              orderby p.Position
                              select p).First().EditReason
                });
            }
            base.TempData.Add("Reason", ForumHelper.GetString("NoAccess.ModeratorForum", new
            {
                topic.Forum.Name
            }));
            return RedirectToRoute("NoAccess");
        }

        [ValidateInput(false)]
        [HttpPost]
        [NotBanned]
        [Authorize]
        public ActionResult Moderate(UpdateTopicViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                Topic topic = GetRepository<Topic>().Read(model.Id);
                if (topicService.Update(base.ActiveUser, topic, model.Title, model.Body, model.Type, model.Flag, model.Reason, base.Url.RouteUrl("ShowForum", new
                {
                    id = topic.Forum.Id,
                    title = topic.Forum.Name.ToSlug(),
                    area = "forum"
                })))
                {
                    return RedirectToAction("index", "moderate", new RouteValueDictionary
                {
                    {
                        "id",
                        topic.Forum.Id
                    },
                    {
                        "area",
                        "forum"
                    }
                });
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            Topic topic = GetRepository<Topic>().Read(id);
            if ((topic.Forum.HasAccess(AccessFlag.Edit) && base.ActiveUser.Id == topic.Author.Id) || topic.Forum.HasAccess(AccessFlag.Moderator))
            {
                Dictionary<string, string> path = new Dictionary<string, string>();
                HomeController.BuildPath(topic, path, base.Url);
                return View(new UpdateTopicViewModel
                {
                    Id = topic.Id,
                    Title = topic.Title,
                    Body = (from p in topic.Posts
                            orderby p.Position
                            select p).First().Body,
                    Flag = topic.Flag,
                    Type = topic.Type,
                    Path = path,
                    IsModerator = topic.Forum.HasAccess(AccessFlag.Moderator)
                });
            }
            base.TempData.Add("Reason", ForumHelper.GetString("NoAccess.EditTopic"));
            return RedirectToRoute("NoAccess");
        }

        [Authorize]
        [HttpPost]
        [NotBanned]
        [ValidateInput(false)]
        public ActionResult Edit(UpdateTopicViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                Topic topic = GetRepository<Topic>().Read(model.Id);
                if (topicService.Update(base.ActiveUser, topic, model.Title, model.Body, base.Url.RouteUrl("ShowForum", new
                {
                    id = topic.Forum.Id,
                    title = topic.Forum.Name.ToSlug(),
                    area = "forum"
                })))
                {
                    return RedirectToRoute("ShowTopic", new RouteValueDictionary
                {
                    {
                        "id",
                        topic.Id
                    },
                    {
                        "title",
                        topic.Title.ToSlug()
                    }
                });
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Follow(int topicId)
        {
            Topic topic = GetRepository<Topic>().Read(topicId);
            if (base.Authenticated)
            {
                FollowTopic newEntity = new FollowTopic(topic, base.ActiveUser);
                GetRepository<FollowTopic>().Create(newEntity);
                base.Context.SaveChanges();
            }
            return RedirectToRoute("ShowTopic", new RouteValueDictionary
        {
            {
                "id",
                topic.Id
            },
            {
                "title",
                topic.Title.ToSlug()
            }
        });
        }

        [Authorize]
        public ActionResult UnFollow(int topicId)
        {
            Topic topic = GetRepository<Topic>().Read(topicId);
            if (base.Authenticated)
            {
                IRepository<FollowTopic> repository = GetRepository<FollowTopic>();
                FollowTopic followTopic = repository.ReadOne(new FollowTopicSpecifications.SpecificTopicAndUser(topic, base.ActiveUser));
                if (followTopic != null)
                {
                    repository.Delete(followTopic);
                    base.Context.SaveChanges();
                }
            }
            return RedirectToRoute("ShowTopic", new RouteValueDictionary
        {
            {
                "id",
                topic.Id
            },
            {
                "title",
                topic.Title.ToSlug()
            }
        });
        }
    }

}