// mvcForum.Web.Areas.Forum.Controllers.ModerateController
using ApplicationBoilerplate.DataProvider;
using ApplicationBoilerplate.Events;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Events;
using mvcForum.Core.Interfaces.Data;
using mvcForum.Core.Specifications;
using mvcForum.Web;
using mvcForum.Web.Controllers;
using mvcForum.Web.Extensions;
using mvcForum.Web.Helpers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.Forum.Controllers
{


    public class ModerateController : ThemedForumBaseController
    {
        private readonly IConfiguration config;

        private readonly IEventPublisher eventPublisher;

        private readonly IBoardRepository boardRepo;

        private readonly IForumRepository forumRepo;

        private readonly IRepository<PostReport> prRepo;

        private readonly ITopicRepository topicRepo;

        public ModerateController(IWebUserProvider userProvider, IContext context, IConfiguration config, IEventPublisher eventPublisher, IBoardRepository boardRepo, IForumRepository forumRepo, ITopicRepository topicRepo)
            : base(userProvider, context)
        {
            this.config = config;
            this.eventPublisher = eventPublisher;
            this.boardRepo = boardRepo;
            this.forumRepo = forumRepo;
            this.topicRepo = topicRepo;
            prRepo = base.context.GetRepository<PostReport>();
        }

        public ActionResult Index(int? id, int? page)
        {
            ModerateViewModel moderateViewModel = new ModerateViewModel();
            IEnumerable<Board> source = boardRepo.ReadManyOptimized(new BoardSpecifications.Enabled());
            Board board = source.First();
            List<mvcForum.Core.Forum> list = new List<mvcForum.Core.Forum>();
            foreach (Category category in board.Categories)
            {
                foreach (mvcForum.Core.Forum item in from f in category.Forums
                                       where !f.ParentForumId.HasValue
                                       select f)
                {
                    GetAccessibleForums(item, list);
                }
            }
            if (list.Count == 0)
            {
                base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ModeratorForums"));
                return new HttpStatusCodeResult(403);
            }
            moderateViewModel.AccessibleForums = from f in list
                                                 select new ForumViewModel(f, config.TopicsPerPage) into vm
                                                 orderby vm.Name
                                                 select vm;
            if (id.HasValue)
            {
                int num = 1;
                if (page.HasValue && page.Value > 0)
                {
                    num = page.Value;
                }
                mvcForum.Core.Forum forum = forumRepo.ReadOneOptimized(new ForumSpecifications.ById(id.Value));
                if (forum != null && !forum.HasAccess(AccessFlag.Moderator))
                {
                    base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ModeratorForum", new
                    {
                        forum.Name
                    }));
                    return RedirectToRoute("NoAccess");
                }
                if (forum != null)
                {
                    moderateViewModel.SelectedForum = new ForumViewModel(forum, config.TopicsPerPage);
                    int count = forum.Topics.Count;
                    moderateViewModel.SelectedForum.Paging = new PagingModel
                    {
                        ActualCount = count,
                        Count = count,
                        Page = num,
                        Pages = (int)Math.Ceiling((decimal)count / (decimal)config.TopicsPerPage)
                    };
                    moderateViewModel.SelectedForum.Topics = from x in (from t in forum.Topics
                                                                        where t.TypeValue == 2
                                                                        orderby t.LastPosted descending
                                                                        select t).Concat((from t in forum.Topics
                                                                                          where t.TypeValue == 1
                                                                                          orderby t.LastPosted descending
                                                                                          select t).Concat(from t in forum.Topics
                                                                                                           where t.TypeValue == 4
                                                                                                           orderby t.LastPosted descending
                                                                                                           select t)).Skip((num - 1) * config.TopicsPerPage).Take(config.TopicsPerPage)
                                                             select new TopicViewModel(x, new MessageViewModel[0], x.PostCount, config.MessagesPerPage, showDeleted: true);
                }
            }
            return View(moderateViewModel);
        }

        [HttpPost]
        public ActionResult Execute(int id, int[] topics, string action)
        {
            switch (action.ToLower())
            {
                case "delete":
                    return Redirect(string.Format("/forum/moderate/delete/{0}?{1}", id, string.Join("&", from t in topics
                                                                                                         select $"topics={t}")));
                case "move":
                    return Redirect(string.Format("/forum/moderate/move/{0}?{1}", id, string.Join("&", from t in topics
                                                                                                       select $"topics={t}")));
                case "merge":
                    return Redirect(string.Format("/forum/moderate/merge/{0}?{1}", id, string.Join("&", from t in topics
                                                                                                        select $"topics={t}")));
                case "split":
                    return RedirectToAction("split", new
                    {
                        id = id,
                        topicId = topics.First()
                    });
                default:
                    return new HttpStatusCodeResult(404);
            }
        }

        public ActionResult Merge(int id, int[] topics)
        {
            if (topics != null && topics.Length > 0)
            {
                mvcForum.Core.Forum forum = base.Context.GetRepository<mvcForum.Core.Forum>().Read(id);
                if (topics.Length < 2)
                {
                    base.TempData.Add("Feedback", ForumHelper.GetString<ForumConfigurator>("Merge.MissingTopics"));
                    return RedirectToAction("index", new
                    {
                        id
                    });
                }
                if (!forum.HasAccess(AccessFlag.Moderator))
                {
                    base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ModeratorForum", new
                    {
                        forum.Name
                    }));
                    return RedirectToRoute("NoAccess");
                }
                MergeViewModel mergeViewModel = new MergeViewModel();
                mergeViewModel.Forum = new ForumViewModel(forum, config.TopicsPerPage);
                mergeViewModel.Topics = from t in forum.Topics
                                        where topics.Contains(t.Id)
                                        select new TopicViewModel(t, new List<MessageViewModel>(), 0, config.MessagesPerPage, showDeleted: false);
                if (mergeViewModel.Topics.Count() < 2)
                {
                    base.TempData.Add("Feedback", ForumHelper.GetString<ForumConfigurator>("Merge.MissingTopics"));
                    return RedirectToAction("index", new
                    {
                        id
                    });
                }
                return View(mergeViewModel);
            }
            return RedirectToAction("Index", new
            {
                id
            });
        }

        [HttpPost]
        public ActionResult Merge(int id, int[] topics, bool confirm)
        {
            if (topics != null && topics.Length > 0)
            {
                mvcForum.Core.Forum forum = base.Context.GetRepository<mvcForum.Core.Forum>().Read(id);
                if (!forum.HasAccess(AccessFlag.Moderator))
                {
                    base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ModeratorForum", new
                    {
                        forum.Name
                    }));
                    return RedirectToRoute("NoAccess");
                }
                IEnumerable<Topic> source = from t in forum.Topics
                                            where topics.Contains(t.Id)
                                            orderby t.Posted
                                            select t;
                IEnumerable<Post> enumerable = from p in source.SelectMany((Topic t) => t.Posts)
                                               orderby p.Posted
                                               select p;
                Topic topic = source.First();
                int num = 0;
                foreach (Post item in enumerable)
                {
                    item.Position = num;
                    item.Topic = topic;
                    num++;
                }
                base.Context.SaveChanges();
                eventPublisher.Publish(new TopicMergedEvent
                {
                    ForumId = topic.ForumId,
                    TopicId = topic.Id
                });
                Dictionary<int, TopicFlag> dictionary = new Dictionary<int, TopicFlag>();
                foreach (Topic item2 in (from t in source
                                         orderby t.Posted descending
                                         select t).Take(source.Count() - 1))
                {
                    dictionary.Add(item2.Id, item2.Flag);
                    item2.SetFlag(TopicFlag.Deleted);
                }
                base.Context.SaveChanges();
                foreach (Topic item3 in (from t in source
                                         orderby t.Posted descending
                                         select t).Take(source.Count() - 1))
                {
                    eventPublisher.Publish(new TopicFlagUpdatedEvent
                    {
                        OriginalFlag = dictionary[item3.Id],
                        TopicId = item3.Id,
                        ForumRelativeURL = base.Url.RouteUrl("ShowForum", new
                        {
                            id = item3.Forum.Id,
                            title = item3.Forum.Name.ToSlug(),
                            area = "forum"
                        })
                    });
                }
            }
            return RedirectToAction("index", new
            {
                id
            });
        }

        public ActionResult Move(int id, int[] topics)
        {
            if (topics != null && topics.Length > 0)
            {
                mvcForum.Core.Forum forum = base.Context.GetRepository<mvcForum.Core.Forum>().Read(id);
                if (!forum.HasAccess(AccessFlag.Moderator))
                {
                    base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ModeratorForum", new
                    {
                        forum.Name
                    }));
                    return RedirectToRoute("NoAccess");
                }
                MoveViewModel moveViewModel = new MoveViewModel();
                moveViewModel.Forum = new ForumViewModel(forum, config.TopicsPerPage);
                moveViewModel.Topics = from t in (from t in forum.Topics.AsQueryable()
                                                  where topics.Contains(t.Id)
                                                  select t).Where(new TopicSpecifications.Visible().IsSatisfied)
                                                  select new TopicViewModel(t, new List<MessageViewModel>(), 0, config.MessagesPerPage, false);
                return View(moveViewModel);
            }
            return RedirectToAction("Index", new
            {
                id
            });
        }

        [HttpPost]
        public ActionResult Move(int id, int destinationId, int[] topics, bool? leaveTopic)
        {
            if (topics != null && topics.Length > 0)
            {
                mvcForum.Core.Forum forum = base.Context.GetRepository<mvcForum.Core.Forum>().Read(id);
                mvcForum.Core.Forum forum2 = base.Context.GetRepository<mvcForum.Core.Forum>().Read(destinationId);
                if (!forum.HasAccess(AccessFlag.Moderator) || !forum2.HasAccess(AccessFlag.Moderator))
                {
                    base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ModeratorForum", new
                    {
                        forum.Name
                    }));
                    return RedirectToRoute("NoAccess");
                }
                List<Topic> list = new List<Topic>(from t in forum.Topics
                                                   where topics.Contains(t.Id)
                                                   select t);
                bool flag = leaveTopic.HasValue && leaveTopic.Value;
                foreach (Topic item in list)
                {
                    item.Forum = forum2;
                    if (flag)
                    {
                        Topic topic = new Topic();
                        topic.Author = item.Author;
                        topic.AuthorId = item.AuthorId;
                        topic.Title = item.Title;
                        topic.AuthorName = item.Author.Name;
                        topic.Forum = forum;
                        topic.ForumId = forum.Id;
                        topic.OriginalTopic = item;
                        topic.OriginalTopicId = item.Id;
                        topic.Posted = item.Posted;
                        topic.LastPosted = item.LastPosted;
                        topic.Type = TopicType.Regular;
                        topic.PostCount = 0;
                        topic.SpamReporters = 0;
                        topic.SpamScore = 0;
                        topic.ViewCount = 0;
                        Topic topic2 = topic;
                        topic2.SetFlag(TopicFlag.Moved);
                        topicRepo.Create(topic2);
                        Post post = new Post(topic2.Author, topic2, topic2.Title, "Moved ", (from p in item.Posts
                                                                                             orderby p.Posted
                                                                                             select p).First().IP);
                        post.Position = 0;
                        post.ModeratorChanged = false;
                        base.Context.GetRepository<Post>().Create(post);
                    }
                }
                base.Context.SaveChanges();
                foreach (Topic item2 in list)
                {
                    eventPublisher.Publish(new TopicMovedEvent
                    {
                        SourceForumId = forum.Id,
                        DestinationForumId = forum2.Id,
                        TopicId = item2.Id
                    });
                }
            }
            return RedirectToAction("index", new
            {
                id
            });
        }

        public ActionResult Delete(int id, int[] topics)
        {
            if (topics != null && topics.Length > 0)
            {
                mvcForum.Core.Forum forum = base.Context.GetRepository<mvcForum.Core.Forum>().Read(id);
                if (!forum.HasAccess(AccessFlag.Moderator))
                {
                    base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ModeratorForum", new
                    {
                        forum.Name
                    }));
                    return RedirectToRoute("NoAccess");
                }
                DeleteViewModel deleteViewModel = new DeleteViewModel();
                deleteViewModel.Forum = new ForumViewModel(forum, config.TopicsPerPage);
                deleteViewModel.Topics = from t in forum.Topics.Where(delegate (Topic t)
                {
                    if (topics.Contains(t.Id))
                    {
                        return t.FlagValue != 4;
                    }
                    return false;
                })
                                         select new TopicViewModel(t, new List<MessageViewModel>(), 0, config.MessagesPerPage, showDeleted: false);
                if (deleteViewModel.Topics.Count() < 1)
                {
                    return RedirectToAction("index", new
                    {
                        id
                    });
                }
                return View(deleteViewModel);
            }
            return RedirectToAction("index", new
            {
                id
            });
        }

        [HttpPost]
        public ActionResult Delete(int id, int[] topics, bool confirm)
        {
            if (topics != null && topics.Length > 0)
            {
                mvcForum.Core.Forum forum = base.Context.GetRepository<mvcForum.Core.Forum>().Read(id);
                if (!forum.HasAccess(AccessFlag.Moderator))
                {
                    base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ModeratorForum", new
                    {
                        forum.Name
                    }));
                    return RedirectToRoute("NoAccess");
                }
                IEnumerable<Topic> enumerable = (from t in forum.Topics
                                                 where topics.Contains(t.Id)
                                                 select t).ToList();
                foreach (Topic item in enumerable)
                {
                    Topic topic = topicRepo.ReadOne(new TopicSpecifications.MovedTopic(item));
                    TopicFlag flag;
                    if (topic != null)
                    {
                        flag = topic.Flag;
                        topic.SetFlag(TopicFlag.Deleted);
                        base.Context.SaveChanges();
                        eventPublisher.Publish(new TopicFlagUpdatedEvent
                        {
                            TopicId = topic.Id,
                            OriginalFlag = flag,
                            ForumRelativeURL = base.Url.RouteUrl("ShowForum", new
                            {
                                id = forum.Id,
                                title = forum.Name.ToSlug(),
                                area = "forum"
                            })
                        });
                        IRepository<Post> repository = base.Context.GetRepository<Post>();
                        foreach (Post post in topic.Posts)
                        {
                            repository.Delete(post);
                        }
                        topicRepo.Delete(topic);
                        base.Context.SaveChanges();
                    }
                    flag = item.Flag;
                    item.SetFlag(TopicFlag.Deleted);
                    base.Context.SaveChanges();
                    eventPublisher.Publish(new TopicFlagUpdatedEvent
                    {
                        TopicId = item.Id,
                        OriginalFlag = flag,
                        ForumRelativeURL = base.Url.RouteUrl("ShowForum", new
                        {
                            id = forum.Id,
                            title = forum.Name.ToSlug(),
                            area = "forum"
                        })
                    });
                }
            }
            return RedirectToAction("index", new
            {
                id
            });
        }

        public ActionResult Split(int id, int topicId)
        {
            mvcForum.Core.Forum forum = base.Context.GetRepository<mvcForum.Core.Forum>().Read(id);
            if (!forum.HasAccess(AccessFlag.Moderator))
            {
                base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ModeratorForum", new
                {
                    forum.Name
                }));
                return RedirectToRoute("NoAccess");
            }
            Topic topic = (from t in forum.Topics
                           where t.Id == topicId
                           select t).First();
            if (topic == null)
            {
                return RedirectToAction("index", new
                {
                    id
                });
            }
            SplitViewModel splitViewModel = new SplitViewModel();
            splitViewModel.Forum = new ForumViewModel(forum, config.TopicsPerPage);
            splitViewModel.Topic = new TopicViewModel(topic, from p in topic.Posts
                                                             select new MessageViewModel(p), topic.Posts.Count, int.MaxValue, showDeleted: false);
            splitViewModel.OriginalTopicTitle = topic.Title;
            return View(splitViewModel);
        }

        [HttpPost]
        public ActionResult Split(SplitViewModel model)
        {
            mvcForum.Core.Forum forum = forumRepo.Read(model.ForumId);
            if (!forum.HasAccess(AccessFlag.Moderator))
            {
                base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ModeratorForum", new
                {
                    forum.Name
                }));
                return RedirectToRoute("NoAccess");
            }
            Topic topic = (from t in forum.Topics
                           where t.Id == model.TopicId
                           select t).First();
            if (topic == null)
            {
                return RedirectToAction("index", new
                {
                    id = model.ForumId
                });
            }
            if (base.ModelState.IsValid)
            {
                IEnumerable<Post> posts = from p in topic.Posts
                                          where model.PostId.Contains(p.Id)
                                          orderby p.Posted
                                          select p;
                if (posts.Count() == 0)
                {
                    return RedirectToAction("index", new
                    {
                        id = model.ForumId
                    });
                }
                if ((from p in posts
                     where p.Position == 0
                     select p).Any())
                {
                    posts = from p in topic.Posts
                            where !(from pr in posts
                                    select pr.Id).Contains(p.Id)
                            select p;
                }
                Post topicPost = (from p in posts
                                  orderby p.Posted
                                  select p).First();
                posts = from p in posts
                        where p.Id != topicPost.Id
                        orderby p.Posted
                        select p;
                Topic topic2 = new Topic(topicPost.Author, forum, model.NewTopicTitle.Replace("<", "&lt;"));
                topic2.Posted = topicPost.Posted;
                topic2.SetFlag(TopicFlag.None);
                topic2.Type = TopicType.Regular;
                topicRepo.Create(topic2);
                topicPost.Topic = topic2;
                topicPost.Indent = 0;
                topicPost.ReplyToPostId = null;
                topicPost.Position = 0;
                context.SaveChanges();
                int num = 1;
                foreach (Post item in posts)
                {
                    item.Topic = topic2;
                    item.Position = num;
                    num++;
                }
                topic2.PostCount = posts.Visible(config).Count();
                context.SaveChanges();
                topic = topicRepo.Read(model.TopicId);
                num = 0;
                foreach (Post post in topic.Posts)
                {
                    post.Position = num;
                    num++;
                }
                topic.PostCount = topic.Posts.Visible(config).Count() - 1;
                context.SaveChanges();
                eventPublisher.Publish(new TopicSplitEvent
                {
                    OriginalTopicId = topic.Id,
                    NewTopicId = topic2.Id
                });
                return RedirectToAction("index", new
                {
                    id = model.ForumId
                });
            }
            model.Forum = new ForumViewModel(forum, config.TopicsPerPage);
            model.Topic = new TopicViewModel(topic, new List<MessageViewModel>(), topic.Posts.Count, int.MaxValue, showDeleted: false);
            return View(model);
        }

        public ActionResult Topic(int id, int? page)
        {
            Topic topic = topicRepo.ReadOneOptimizedWithPosts(new TopicSpecifications.ById(id));
            if (topic == null)
            {
                return new HttpStatusCodeResult(404);
            }
            if (!topic.Forum.HasAccess(AccessFlag.Moderator))
            {
                base.TempData.Add("Reason", ForumHelper.GetString<ForumConfigurator>("NoAccess.ModeratorForum", new
                {
                    topic.Forum.Name
                }));
                return RedirectToRoute("NoAccess");
            }
            int num = 1;
            if (page.HasValue && page.Value > 0)
            {
                num = page.Value;
            }
            TopicViewModel topicViewModel = new TopicViewModel(topic, from p in (from p in topic.Posts
                                                                                 orderby p.Posted
                                                                                 select p).Skip((num - 1) * config.MessagesPerPage).Take(config.MessagesPerPage)
                                                                      select new MessageViewModel(p), topic.Posts.Count, config.MessagesPerPage, showDeleted: true);
            topicViewModel.Path = new Dictionary<string, string>
        {
            {
                base.Url.Action("index", "moderate", new
                {
                    id = topic.Forum.Id
                }),
                ForumHelper.GetString("Moderate.TitleMain")
            },
            {
                base.Url.Action("topic", "moderate", new
                {
                    id = topic.Id
                }),
                topic.Title
            }
        };
            return View(topicViewModel);
        }

        public ActionResult ReportList()
        {
            IEnumerable<PostReport> enumerable = prRepo.ReadMany(new PostReportSpecifications.NotResolved());
            List<PostReport> list = new List<PostReport>();
            foreach (PostReport item in enumerable)
            {
                if (item.Post.Topic.Forum.HasAccess(AccessFlag.Moderator))
                {
                    list.Add(item);
                }
            }
            return View(from r in list
                        select new PostReportViewModel(r) into r
                        orderby r.Timestamp
                        select r);
        }

        public ActionResult Report(int id)
        {
            PostReport pr = prRepo.Read(id);
            return View(new PostReportViewModel(pr));
        }

        [HttpPost]
        public ActionResult Report(PostReportViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                PostReport postReport = prRepo.Read(model.Id);
                postReport.Resolved = model.Resolved;
                if (postReport.Resolved)
                {
                    postReport.ResolvedBy = base.ActiveUser;
                    postReport.ResolvedTimestamp = DateTime.UtcNow;
                }
                if (postReport.Post.Position == 0)
                {
                    postReport.Post.Topic.SetFlag(model.TopicFlag);
                    postReport.Post.Topic.Type = model.TopicType;
                    postReport.Post.Update(base.ActiveUser, model.Title, model.Content, model.ChangeReason);
                }
                else
                {
                    postReport.Post.SetFlag(model.PostFlag);
                    postReport.Post.Update(base.ActiveUser, model.Subject, model.Content, model.ChangeReason);
                }
                base.Context.SaveChanges();
                eventPublisher.Publish(new PostReportResolvedEvent
                {
                    PostReportId = postReport.Id
                });
                return RedirectToAction("ReportList", "Moderate");
            }
            PostReport report = prRepo.Read(model.Id);
            model.Populate(report);
            return View(model);
        }

        [NonAction]
        private void GetAccessibleForums(mvcForum.Core.Forum forum, List<mvcForum.Core.Forum> accessibleForums)
        {
            if (forum.HasAccess(AccessFlag.Moderator))
            {
                accessibleForums.Add(forum);
            }
            foreach (mvcForum.Core.Forum subForum in forum.SubForums)
            {
                GetAccessibleForums(subForum, accessibleForums);
            }
        }
    }


}