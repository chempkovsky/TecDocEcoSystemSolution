// mvcForum.Web.Areas.Forum.Controllers.CategoryController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Core.Interfaces.Data;
using mvcForum.Core.Specifications;
using mvcForum.Web.Areas.Forum.Controllers;
using mvcForum.Web.Controllers;
using mvcForum.Web.Extensions;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.Forum.Controllers
{

    public class CategoryController : ThemedForumBaseController
    {
        private readonly IConfiguration config;

        private readonly IForumRepository forumRepo;

        public CategoryController(IWebUserProvider userProvider, IContext context, IConfiguration config, IForumRepository forumRepo)
            : base(userProvider, context)
        {
            this.config = config;
            this.forumRepo = forumRepo;
        }

        public ActionResult Index(int id, string title)
        {
            Category category = GetRepository<Category>().Read(id);
            if (title != category.Name.ToSlug())
            {
                return RedirectPermanent(base.Url.RouteUrl("ShowCategory", new
                {
                    area = "forum",
                    title = category.Name.ToSlug(),
                    id = category.Id
                }));
            }
            CategoryViewModel categoryViewModel = new CategoryViewModel(category);
            List<ForumViewModel> list = new List<ForumViewModel>();
            IEnumerable<mvcForum.Core.Forum> source = forumRepo.ReadManyOptimized(new ForumSpecifications.SpecificCategoryNoParentForum(category));
            foreach (mvcForum.Core.Forum item in from f in source
                                   orderby f.SortOrder
                                   select f)
            {
                List<ForumViewModel> list2 = new List<ForumViewModel>();
                foreach (mvcForum.Core.Forum item2 in item.SubForums.OrderBy((mvcForum.Core.Forum f) => f.SortOrder))
                {
                    list2.Add(new ForumViewModel(item2, config.TopicsPerPage));
                }
                list.Add(new ForumViewModel(item, config.TopicsPerPage)
                {
                    SubForums = new ReadOnlyCollection<ForumViewModel>(list2),
                    Category = categoryViewModel
                });
            }
            categoryViewModel.Forums = new ReadOnlyCollection<ForumViewModel>(list);
            categoryViewModel.Path = new Dictionary<string, string>();
            HomeController.BuildPath(category, categoryViewModel.Path, base.Url);
            return View(categoryViewModel);
        }

        [Authorize]
        [ActionName("Mark As Read")]
        public ActionResult MarkAsRead(int id)
        {
            Category category = GetRepository<Category>().Read(id);
            foreach (mvcForum.Core.Forum forum in category.Forums)
            {
                if (forum.HasAccess(AccessFlag.Read))
                {
                    forum.Track();
                }
            }
            return RedirectToAction("index", new
            {
                area = "forum",
                id = id,
                title = category.Name.ToSlug()
            });
        }
    }

}