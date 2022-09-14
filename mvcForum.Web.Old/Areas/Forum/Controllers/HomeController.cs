// mvcForum.Web.Areas.Forum.Controllers.HomeController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces.Data;
using mvcForum.Core.Specifications;
using mvcForum.Web.Controllers;
using mvcForum.Web.Extensions;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web.Areas.Forum.Controllers
{

    public class HomeController : ThemedForumBaseController
    {
        private readonly IConfiguration config;

        private readonly IBoardRepository boardRepo;

        private readonly IForumRepository forumRepo;

        private readonly IUserService userService;

        public HomeController(IConfiguration config, IWebUserProvider userProvider, IContext context, IBoardRepository boardRepo, IForumRepository forumRepo, IUserService userService)
            : base(userProvider, context)
        {
            this.config = config;
            this.boardRepo = boardRepo;
            this.forumRepo = forumRepo;
            this.userService = userService;
        }

        public ActionResult Index()
        {
            Board board = null;
            string empty = string.Empty;
            if (!string.IsNullOrWhiteSpace(empty) && int.TryParse(empty, out int result))
            {
                board = boardRepo.ReadOneOptimized(new BoardSpecifications.ById(result));
            }
            if (board == null)
            {
                IEnumerable<Board> source = boardRepo.ReadManyOptimized(new BoardSpecifications.Enabled());
                if (!source.Any())
                {
                    return RedirectToAction("index", "basicinstall", new
                    {
                        area = "forumadmin"
                    });
                }
                board = source.First();
            }
            BoardViewModel boardViewModel = new BoardViewModel(board);
            boardViewModel.Path = new Dictionary<string, string>();
            boardViewModel.ShowOnline = config.ShowOnlineUsers;
            if (boardViewModel.ShowOnline)
            {
                IEnumerable<ForumUser> onlineUsers = userService.GetOnlineUsers();
                boardViewModel.OnlineUsers = onlineUsers.OrderBy(delegate (ForumUser u)
                {
                    if (!u.UseFullName)
                    {
                        return u.Name;
                    }
                    return u.FullName;
                });
            }
            List<CategoryViewModel> list = new List<CategoryViewModel>();
            foreach (Category item in from c in board.Categories
                                      orderby c.SortOrder
                                      select c)
            {
                CategoryViewModel categoryViewModel = new CategoryViewModel(item);
                list.Add(categoryViewModel);
                IEnumerable<mvcForum.Core.Forum> source2 = forumRepo.ReadManyOptimized(new ForumSpecifications.SpecificCategoryNoParentForum(item));
                categoryViewModel.Forums = source2.OrderBy((mvcForum.Core.Forum f) => f.SortOrder).Select((mvcForum.Core.Forum f) => new ForumViewModel(f, config.TopicsPerPage)
                {
                    SubForums = f.SubForums.Select((mvcForum.Core.Forum sf) => new ForumViewModel(sf, config.TopicsPerPage))
                });
            }
            boardViewModel.Categories = new ReadOnlyCollection<CategoryViewModel>(list);
            return View(boardViewModel);
        }

        [NonAction]
        public static void BuildPath(mvcForum.Core.Forum forum, Dictionary<string, string> path, UrlHelper urlHelper)
        {
            if (forum.ParentForum != null)
            {
                BuildPath(forum.ParentForum, path, urlHelper);
            }
            else
            {
                BuildPath(forum.Category, path, urlHelper);
            }
            string key = urlHelper.RouteUrl("ShowForum", new RouteValueDictionary
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
            path.Add(key, forum.Name);
        }

        [NonAction]
        public static void BuildPath(Category category, Dictionary<string, string> path, UrlHelper urlHelper)
        {
            string key = urlHelper.RouteUrl("ShowCategory", new RouteValueDictionary
        {
            {
                "id",
                category.Id
            },
            {
                "title",
                category.Name.ToSlug()
            }
        });
            path.Add(key, category.Name);
        }

        [NonAction]
        public static void BuildPath(Topic topic, Dictionary<string, string> path, UrlHelper urlHelper)
        {
            BuildPath(topic.Forum, path, urlHelper);
            string key = urlHelper.RouteUrl("ShowTopic", new RouteValueDictionary
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
            path.Add(key, topic.Title);
        }

        [Authorize]
        [ActionName("Mark As Read")]
        public ActionResult MarkAsRead(int id)
        {
            IRepository<Board> repository = GetRepository<Board>();
            Board board = repository.Read(id);
            foreach (Category category in board.Categories)
            {
                foreach (mvcForum.Core.Forum forum in category.Forums)
                {
                    if (forum.HasAccess(AccessFlag.Read))
                    {
                        forum.Track();
                    }
                }
            }
            return RedirectToAction("index");
        }
    }

}