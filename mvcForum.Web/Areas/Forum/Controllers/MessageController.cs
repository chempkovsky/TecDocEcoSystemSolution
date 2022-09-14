// mvcForum.Web.Areas.Forum.Controllers.MessageController
using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Events;
using mvcForum.Core.Interfaces.Search;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Web;
using mvcForum.Web.Areas.Forum.Controllers;
using mvcForum.Web.Attributes;
using mvcForum.Web.Controllers;
using mvcForum.Web.Extensions;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using mvcForum.Web.ViewModels.Create;
using mvcForum.Web.ViewModels.Delete;
using mvcForum.Web.ViewModels.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web.Areas.Forum.Controllers
{

    public class MessageController : ThemedForumBaseController
    {
        private readonly IConfiguration config;

        private readonly IIndexer indexer;

        private readonly IEventPublisher eventPublisher;

        public readonly IPostService postService;

        private readonly IAttachmentService attachmentService;

        public MessageController(IWebUserProvider userProvider, IContext context, IConfiguration config, IIndexer indexer, IEventPublisher eventPublisher, IAttachmentService attachmentService, IPostService postService)
            : base(userProvider, context)
        {
            this.config = config;
            this.indexer = indexer;
            this.eventPublisher = eventPublisher;
            this.attachmentService = attachmentService;
            this.postService = postService;
        }

        [Authorize]
        public ActionResult Create(int id, int? replyToId)
        {
            Topic topic = GetRepository<Topic>().Read(id);
            if (topic.Forum.HasAccess(AccessFlag.Post))
            {
                Post post = null;
                if (replyToId.HasValue && replyToId.Value > 0)
                {
                    post = GetRepository<Post>().Read(replyToId.Value);
                }
                CreateMessageViewModel createMessageViewModel = new CreateMessageViewModel();
                createMessageViewModel.TopicId = topic.Id;
                createMessageViewModel.Topic = new TopicViewModel(topic, new MessageViewModel[0], 0, config.MessagesPerPage, showDeleted: false);
                createMessageViewModel.Posts = new List<MessageViewModel>();
                createMessageViewModel.CanUpload = topic.Forum.HasAccess(AccessFlag.Upload);
                CreateMessageViewModel createMessageViewModel2 = createMessageViewModel;
                createMessageViewModel2.Subject = $"Re: {topic.Title}";
                if (config.ShowOldPostsOnReply)
                {
                    IEnumerable<Post> enumerable = from p in topic.Posts.Visible(config)
                                                   orderby p.Posted descending
                                                   select p;
                    if (config.PostsOnReply > 0)
                    {
                        enumerable = enumerable.Take(config.PostsOnReply);
                    }
                    foreach (Post item in enumerable)
                    {
                        createMessageViewModel2.Posts.Add(new MessageViewModel(item));
                    }
                }
                if (post != null && post.Topic.Id == topic.Id)
                {
                    createMessageViewModel2.ReplyTo = post.Id;
                    createMessageViewModel2.Body = ForumHelper.Quote(post.AuthorName, post.Body);
                    createMessageViewModel2.Subject = $"Re: {post.Subject}";
                }
                createMessageViewModel2.Path = new Dictionary<string, string>();
                HomeController.BuildPath(topic, createMessageViewModel2.Path, base.Url);
                createMessageViewModel2.Path.Add("/", "New reply");
                return View(createMessageViewModel2);
            }
            base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ForumPosting", new
            {
                topic.Forum.Name
            }));
            return RedirectToRoute("NoAccess");
        }

        [NotBanned]
        [ValidateInput(false)]
        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateMessageViewModel newMessage, HttpPostedFileBase[] files)
        {
            GetRepository<Post>();
            Topic topic = GetRepository<Topic>().Read(newMessage.TopicId);
            if (base.ModelState.IsValid)
            {
                if (topic.Forum.HasAccess(AccessFlag.Post))
                {
                    Post replyTo = null;
                    if (newMessage.ReplyTo.HasValue)
                    {
                        replyTo = context.GetRepository<Post>().Read(newMessage.ReplyTo.Value);
                    }
                    List<string> list = new List<string>();
                    Post post = postService.Create(base.ActiveUser, topic, newMessage.Subject, newMessage.Body, base.Request.UserHostAddress, base.Request.UserAgent, base.Url.RouteUrl("ShowTopic", new
                    {
                        id = topic.Id,
                        area = "forum",
                        title = topic.Title.ToSlug()
                    }), list, replyTo);
                    if (post != null)
                    {
                        AccessFlag access = topic.Forum.GetAccess();
                        if ((access & AccessFlag.Upload) == AccessFlag.Upload)
                        {
                            if (files != null && files.Length > 0)
                            {
                                foreach (HttpPostedFileBase httpPostedFileBase in files)
                                {
                                    if (httpPostedFileBase != null)
                                    {
                                        AttachStatusCode attachStatusCode = attachmentService.AttachFile(base.ActiveUser, post, httpPostedFileBase.FileName, httpPostedFileBase.ContentType, httpPostedFileBase.ContentLength, httpPostedFileBase.InputStream);
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
                            else if (newMessage.AttachFile)
                            {
                                return RedirectToAction("Attach", "File", new RouteValueDictionary
                            {
                                {
                                    "id",
                                    post.Id
                                }
                            });
                            }
                        }
                        if (list.Any())
                        {
                            base.TempData.Add("Feedback", from f in list
                                                          select new MvcHtmlString(f));
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
                    },
                    {
                        "page",
                        (int)Math.Ceiling((decimal)topic.Posts.Visible(config).Count() / (decimal)config.MessagesPerPage)
                    }
                });
                }
                base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ForumPosting", new
                {
                    topic.Forum.Name
                }));
                return RedirectToRoute("NoAccess");
            }
            newMessage.Topic = new TopicViewModel(topic, new MessageViewModel[0], 0, config.MessagesPerPage, showDeleted: false);
            newMessage.Path = new Dictionary<string, string>();
            newMessage.CanUpload = topic.Forum.HasAccess(AccessFlag.Upload);
            HomeController.BuildPath(topic, newMessage.Path, base.Url);
            if (config.ShowOldPostsOnReply)
            {
                IEnumerable<Post> source = from m in topic.Posts.Visible(config)
                                           orderby m.Posted descending
                                           select m;
                if (config.PostsOnReply > 0)
                {
                    source = source.Take(config.PostsOnReply);
                }
                newMessage.Posts = (from m in source
                                    select new MessageViewModel(m)).ToList();
            }
            return View(newMessage);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            Post post = GetRepository<Post>().Read(id);
            if ((post.Topic.Forum.HasAccess(AccessFlag.Edit) && base.ActiveUser.Id == post.Author.Id) || post.Topic.Forum.HasAccess(AccessFlag.Moderator))
            {
                Dictionary<string, string> path = new Dictionary<string, string>();
                HomeController.BuildPath(post.Topic, path, base.Url);
                return View(new UpdateMessageViewModel
                {
                    TopicId = post.Topic.Id,
                    TopicTitle = post.Topic.Title,
                    Path = path,
                    Body = post.Body,
                    Subject = post.Subject,
                    Id = post.Id,
                    IsModerator = post.Topic.Forum.HasAccess(AccessFlag.Moderator),
                    Flag = post.Flag
                });
            }
            base.TempData.Add("Reason", ForumHelper.GetString("NoAccess.EditPost"));
            return RedirectToRoute("NoAccess");
        }

        [NotBanned]
        [ValidateInput(false)]
        [HttpPost]
        [Authorize]
        public ActionResult Edit(UpdateMessageViewModel messageVM)
        {
            IRepository<Post> repository = GetRepository<Post>();
            Post post = repository.Read(messageVM.Id);
            if (base.ModelState.IsValid)
            {
                AccessFlag access = post.Topic.Forum.GetAccess();
                PostFlag flag = post.Flag;
                TopicFlag flag2 = post.Topic.Flag;
                if ((access & AccessFlag.Edit) == AccessFlag.Edit && base.ActiveUser.Id == post.Author.Id)
                {
                    post.Update(base.ActiveUser, messageVM.Subject.Replace("<", "&gt;"), messageVM.Body);
                    if (post.Position == 0 && post.Topic.Author.Id == base.ActiveUser.Id)
                    {
                        post.Topic.Title = messageVM.Subject.Replace("<", "&gt;");
                    }
                    base.Context.SaveChanges();
                    if (post.Position == 0 && post.Topic.Author.Id == base.ActiveUser.Id)
                    {
                        eventPublisher.Publish(new TopicUpdatedEvent
                        {
                            TopicId = post.Topic.Id,
                            UserAgent = base.Request.UserAgent,
                            ForumId = post.Topic.Forum.Id
                        });
                    }
                    else
                    {
                        eventPublisher.Publish(new PostUpdatedEvent
                        {
                            PostId = post.Id,
                            UserAgent = base.Request.UserAgent,
                            TopicId = post.Topic.Id,
                            ForumId = post.Topic.Forum.Id
                        });
                    }
                    base.Context.SaveChanges();
                    return RedirectToRoute("ShowTopic", new RouteValueDictionary
                {
                    {
                        "id",
                        post.Topic.Id
                    },
                    {
                        "title",
                        post.Topic.Title.ToSlug()
                    }
                });
                }
            }
            messageVM.TopicId = post.Topic.Id;
            messageVM.TopicTitle = post.Topic.Title;
            Dictionary<string, string> path = new Dictionary<string, string>();
            HomeController.BuildPath(post.Topic, path, base.Url);
            return View(messageVM);
        }

        [Authorize]
        public ActionResult Moderate(int id)
        {
            Post post = GetRepository<Post>().Read(id);
            if (post.Topic.Forum.HasAccess(AccessFlag.Moderator))
            {
                Dictionary<string, string> path = new Dictionary<string, string>();
                HomeController.BuildPath(post.Topic, path, base.Url);
                return View(new UpdateMessageViewModel
                {
                    TopicId = post.TopicId,
                    TopicTitle = post.Topic.Title,
                    Id = post.Id,
                    Subject = post.Subject,
                    Body = post.Body,
                    Flag = post.Flag,
                    Path = path,
                    IsModerator = post.Topic.Forum.HasAccess(AccessFlag.Moderator),
                    Reason = post.EditReason
                });
            }
            base.TempData.Add("Reason", ForumHelper.GetString("NoAccess.ModeratorForum", new
            {
                post.Topic.Forum.Name
            }));
            return RedirectToRoute("NoAccess");
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Moderate(UpdateMessageViewModel model)
        {
            IRepository<Post> repository = GetRepository<Post>();
            Post post = repository.Read(model.Id);
            if (base.ModelState.IsValid)
            {
                AccessFlag access = post.Topic.Forum.GetAccess();
                PostFlag flag = post.Flag;
                TopicFlag flag2 = post.Topic.Flag;
                if ((access & AccessFlag.Moderator) == AccessFlag.Moderator)
                {
                    post.Update(base.ActiveUser, model.Subject.Replace("<", "&gt;"), model.Body, model.Reason);
                    post.SetFlag(model.Flag);
                    if (post.Flag == PostFlag.None && (from p in post.Topic.Posts.Visible(config)
                                                       orderby p.Posted descending
                                                       select p).FirstOrDefault() == post)
                    {
                        post.Topic.LastPost = post;
                        post.Topic.LastPostAuthor = post.Author;
                        post.Topic.LastPosted = post.Posted;
                        post.Topic.LastPostUsername = post.AuthorName;
                    }
                    context.SaveChanges();
                    if (flag != post.Flag)
                    {
                        eventPublisher.Publish(new PostFlagUpdatedEvent
                        {
                            PostId = post.Id,
                            OriginalFlag = flag,
                            TopicRelativeURL = base.Url.RouteUrl("ShowTopic", new
                            {
                                id = post.Topic.Id,
                                area = "forum",
                                title = post.Topic.Title.ToSlug()
                            })
                        });
                    }
                    else
                    {
                        eventPublisher.Publish(new PostUpdatedEvent
                        {
                            PostId = post.Id,
                            UserAgent = base.Request.UserAgent,
                            TopicId = post.Topic.Id,
                            ForumId = post.Topic.Forum.Id
                        });
                    }
                    return RedirectToAction("index", "moderate", new RouteValueDictionary
                {
                    {
                        "id",
                        post.Topic.Forum.Id
                    },
                    {
                        "area",
                        "forum"
                    }
                });
                }
                base.TempData.Add("Reason", ForumHelper.GetString("NoAccess.ModeratorForum", new
                {
                    post.Topic.Forum.Name
                }));
                return RedirectToRoute("NoAccess");
            }
            model.TopicId = post.Topic.Id;
            model.TopicTitle = post.Topic.Title;
            Dictionary<string, string> path = new Dictionary<string, string>();
            HomeController.BuildPath(post.Topic, path, base.Url);
            return View(model);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            Post post = GetRepository<Post>().Read(id);
            if (CanDeletePost(post))
            {
                Dictionary<string, string> path = new Dictionary<string, string>();
                HomeController.BuildPath(post.Topic, path, base.Url);
                return View(new DeleteMessageViewModel
                {
                    Id = post.Id,
                    TopicId = post.Topic.Id,
                    TopicTitle = post.Topic.Title,
                    Path = path,
                    Subject = post.Subject
                });
            }
            base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.MessageDelete", new
            {
                post.Subject
            }));
            return RedirectToRoute("NoAccess");
        }

        [NonAction]
        private bool CanDeletePost(Post post)
        {
            mvcForum.Core.Forum forum = post.Topic.Forum;
            AccessFlag access = forum.GetAccess();
            if ((access & AccessFlag.Moderator) == AccessFlag.Moderator || ((access & AccessFlag.Delete) == AccessFlag.Delete && base.ActiveUser.Id == post.Author.Id))
            {
                return true;
            }
            return false;
        }

        [NotBanned]
        [Authorize]
        [HttpPost]
        public ActionResult Delete(DeleteMessageViewModel model)
        {
            IRepository<Post> repository = GetRepository<Post>();
            Post post = repository.Read(model.Id);
            PostFlag flag = post.Flag;
            if (post != null)
            {
                Topic topic = post.Topic;
                if (base.ModelState.IsValid)
                {
                    if (post.Position == 0)
                    {
                        throw new NotImplementedException("Deleting a topic!?");
                    }
                    if (model.Delete && CanDeletePost(post))
                    {
                        post.Delete(base.ActiveUser, model.Reason);
                        base.Context.SaveChanges();
                        eventPublisher.Publish(new PostFlagUpdatedEvent
                        {
                            PostId = post.Id,
                            OriginalFlag = flag,
                            TopicRelativeURL = base.Url.RouteUrl("ShowTopic", new
                            {
                                id = topic.Id,
                                area = "forum",
                                title = topic.Title.ToSlug()
                            })
                        });
                        base.Context.SaveChanges();
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
                Dictionary<string, string> path = new Dictionary<string, string>();
                HomeController.BuildPath(topic, path, base.Url);
                model.Path = path;
                model.TopicId = topic.Id;
                model.TopicTitle = topic.Title;
                model.Subject = post.Subject;
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Undelete(int id)
        {
            Post post = GetRepository<Post>().Read(id);
            if (post.Topic.Forum.HasAccess(AccessFlag.Moderator) && post.Flag == PostFlag.Deleted)
            {
                Dictionary<string, string> path = new Dictionary<string, string>();
                HomeController.BuildPath(post.Topic, path, base.Url);
                return View(new DeleteMessageViewModel
                {
                    Id = post.Id,
                    Delete = true,
                    Reason = post.DeleteReason,
                    TopicId = post.Topic.Id,
                    TopicTitle = post.Topic.Title,
                    Path = path,
                    Subject = post.Subject
                });
            }
            base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.MessageUndelete", new
            {
                post.Subject
            }));
            return RedirectToRoute("NoAccess");
        }

        [HttpPost]
        [Authorize]
        [NotBanned]
        public ActionResult Undelete(DeleteMessageViewModel model)
        {
            IRepository<Post> repository = GetRepository<Post>();
            Post post = repository.Read(model.Id);
            PostFlag flag = post.Flag;
            if (post != null)
            {
                Topic topic = post.Topic;
                if (base.ModelState.IsValid)
                {
                    if (post.Position == 0)
                    {
                        throw new NotImplementedException("Deleting a topic!?");
                    }
                    if (!model.Delete && post.Topic.Forum.HasAccess(AccessFlag.Moderator))
                    {
                        post.Undelete(base.ActiveUser, model.Reason);
                        base.Context.SaveChanges();
                        eventPublisher.Publish(new PostFlagUpdatedEvent
                        {
                            PostId = post.Id,
                            OriginalFlag = flag,
                            TopicRelativeURL = base.Url.RouteUrl("ShowTopic", new
                            {
                                id = topic.Id,
                                area = "forum",
                                title = topic.Title.ToSlug()
                            })
                        });
                        base.Context.SaveChanges();
                        return RedirectToAction("topic", "moderate", new RouteValueDictionary
                    {
                        {
                            "id",
                            topic.Id
                        }
                    });
                    }
                }
                Dictionary<string, string> path = new Dictionary<string, string>();
                HomeController.BuildPath(topic, path, base.Url);
                model.Path = path;
                model.TopicId = topic.Id;
                model.TopicTitle = topic.Title;
                model.Subject = post.Subject;
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Report(int id)
        {
            Post post = GetRepository<Post>().Read(id);
            if (post.Topic.Forum.HasAccess(AccessFlag.Read))
            {
                ReportMessageViewModel reportMessageViewModel = new ReportMessageViewModel();
                reportMessageViewModel.Subject = post.Subject;
                reportMessageViewModel.Id = post.Id;
                reportMessageViewModel.TopicId = post.Topic.Id;
                reportMessageViewModel.TopicTitle = post.Topic.Title;
                ReportMessageViewModel reportMessageViewModel2 = reportMessageViewModel;
                Dictionary<string, string> path = new Dictionary<string, string>();
                HomeController.BuildPath(post.Topic, path, base.Url);
                reportMessageViewModel2.Path = path;
                return View(reportMessageViewModel2);
            }
            base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.Forum", new
            {
                post.Topic.Forum.Name
            }));
            return RedirectToRoute("NoAccess");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Report(ReportMessageViewModel model)
        {
            IRepository<Post> repository = GetRepository<Post>();
            Post post = repository.Read(model.Id);
            if (post.Topic.Forum.HasAccess(AccessFlag.Read))
            {
                if (base.ModelState.IsValid)
                {
                    IRepository<PostReport> repository2 = GetRepository<PostReport>();
                    PostReport newEntity = new PostReport(post, model.Reason, base.ActiveUser, feedback: false);
                    repository2.Create(newEntity);
                    base.Context.SaveChanges();
                    return RedirectToRoute("ShowTopic", new RouteValueDictionary
                {
                    {
                        "id",
                        post.Topic.Id
                    },
                    {
                        "title",
                        post.Topic.Title.ToSlug()
                    }
                });
                }
                return View(model);
            }
            base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.Forum", new
            {
                post.Topic.Forum.Name
            }));
            return RedirectToRoute("NoAccess");
        }
    }

}