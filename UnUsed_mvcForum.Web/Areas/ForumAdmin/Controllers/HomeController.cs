// mvcForum.Web.Areas.ForumAdmin.Controllers.HomeController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using mvcForum.Web.Areas.ForumAdmin.ViewModels.List;
using mvcForum.Web.Controllers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{

    public class HomeController : ForumAdminBaseController
    {
        public HomeController(IWebUserProvider userProvider, IContext context)
            : base(userProvider, context)
        {
        }

        public ActionResult Index()
        {
            ForumSettings forumSettings = GetRepository<ForumSettings>().ReadOne(new ForumSettingsSpecifications.SpecificKey("InstallDate"));
            DateTime dateTime = DateTime.Parse(forumSettings.Value);
            Statistics statistics = new Statistics();
            statistics.TopicCount = GetRepository<Topic>().ReadMany(new TopicSpecifications.Visible()).Count();
            statistics.PostCount = GetRepository<Post>().ReadMany(new PostSpecifications.Visible()).Count();
            statistics.AttachmentCount = GetRepository<Attachment>().ReadAll().Count();
            statistics.UserCount = GetRepository<ForumUser>().ReadMany(new ForumUserSpecifications.NotDeleted()).Count();
            statistics.AttachmentSize = GetRepository<Attachment>().ReadAll().Sum((Attachment a) => a.Size);
            statistics.InstallDate = dateTime;
            statistics.Days = DateTime.UtcNow.Subtract(dateTime).Days;
            Statistics model = statistics;
            return View(model);
        }

        [NonAction]
        internal static void BuildPath(Board board, Dictionary<string, string> path, string area)
        {
            path.Add($"/{area}/board/edit/{board.Id}", board.Name);
        }

        [NonAction]
        internal static void BuildPath(Category category, Dictionary<string, string> path, string area)
        {
            BuildPath(category.Board, path, area);
            path.Add($"/{area}/category/edit/{category.Id}", category.Name);
        }

        [NonAction]
        internal static void BuildPath(mvcForum.Core.Forum forum, Dictionary<string, string> path, string area)
        {
            BuildPath(forum.Category, path, area);
            if (forum.ParentForum != null)
            {
                BuildForumPath(forum.ParentForum, path, area);
            }
            path.Add($"/{area}/forum/edit/{forum.Id}", forum.Name);
        }

        [NonAction]
        internal static void BuildForumPath(mvcForum.Core.Forum forum, Dictionary<string, string> path, string area)
        {
            if (forum.ParentForum != null)
            {
                BuildForumPath(forum.ParentForum, path, area);
            }
            path.Add($"/{area}/forum/edit/{forum.Id}", forum.Name);
        }

        [NonAction]
        internal static void AddCategories(Board board, BoardViewModel model)
        {
            List<CategoryViewModel> list = new List<CategoryViewModel>();
            foreach (Category category in board.Categories)
            {
                list.Add(new CategoryViewModel(category));
            }
            model.Categories = new ReadOnlyCollection<CategoryViewModel>(list);
        }
    }

}