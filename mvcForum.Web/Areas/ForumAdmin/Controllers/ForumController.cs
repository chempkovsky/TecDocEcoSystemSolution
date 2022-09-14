using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{
    // mvcForum.Web.Areas.ForumAdmin.Controllers.ForumController
    using ApplicationBoilerplate.DataProvider;
    using mvcForum.Core;
    using mvcForum.Web.Areas.ForumAdmin.Helpers;
    using mvcForum.Web.Areas.ForumAdmin.ViewModels;
    using mvcForum.Web.Controllers;
    using mvcForum.Web.Interfaces;
    using System.Web.Mvc;

    public class ForumController : ForumAdminBaseController
    {
        private readonly IRepository<mvcForum.Core.Category> categoryRepo;

        private readonly IRepository<mvcForum.Core.Forum> forumRepo;

        private readonly IRepository<mvcForum.Core.Group> groupRepo;

        private readonly IRepository<ForumAccess> accessRepo;

        public ForumController(IWebUserProvider userProvider, IContext context)
            : base(userProvider, context)
        {
            categoryRepo = GetRepository<mvcForum.Core.Category>();
            forumRepo = GetRepository<mvcForum.Core.Forum>();
            groupRepo = GetRepository<mvcForum.Core.Group>();
            accessRepo = GetRepository<ForumAccess>();
        }

        public ActionResult Update(int id)
        {
            mvcForum.Core.Forum forum = forumRepo.Read(id);
            return View(new mvcForum.Web.Areas.ForumAdmin.ViewModels.Forum
            {
                Id = id,
                CategoryId = forum.Category.Id,
                Path = BreadcrumbHelper.BuildPath(forum, base.Url),
                BoardId = forum.Category.Board.Id
            });
        }
    }

}