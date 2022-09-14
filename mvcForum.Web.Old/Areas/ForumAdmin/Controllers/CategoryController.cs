// mvcForum.Web.Areas.ForumAdmin.Controllers.CategoryController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Web.Areas.ForumAdmin.Helpers;
using mvcForum.Web.Areas.ForumAdmin.ViewModels;
using mvcForum.Web.Controllers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using mvcForum.Web.ViewModels.Create;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{

    public class CategoryController : ForumAdminBaseController
    {
        private readonly IRepository<mvcForum.Core.Board> boardRepo;

        private readonly IRepository<mvcForum.Core.Category> categoryRepo;

        private readonly IMembershipService membershipService;

        public CategoryController(IWebUserProvider userProvider, IContext context, IMembershipService membershipService)
            : base(userProvider, context)
        {
            boardRepo = GetRepository<mvcForum.Core.Board>();
            categoryRepo = GetRepository<mvcForum.Core.Category>();
            this.membershipService = membershipService;
        }

        public ActionResult Create(int id)
        {
            mvcForum.Core.Board board = boardRepo.Read(id);
            membershipService.IsAccountInRole(base.ActiveUser.EmailAddress, "Board");
            CreateCategoryViewModel createCategoryViewModel = new CreateCategoryViewModel();
            createCategoryViewModel.BoardId = board.Id;
            CreateCategoryViewModel model = createCategoryViewModel;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateCategoryViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                mvcForum.Core.Board board = boardRepo.Read(model.BoardId);
                membershipService.IsAccountInRole(base.ActiveUser.EmailAddress, "Board");
                mvcForum.Core.Category newEntity = new mvcForum.Core.Category(board, model.Name, model.SortOrder);
                categoryRepo.Create(newEntity);
                base.Context.SaveChanges();
                return RedirectToAction("Edit", "Board", new RouteValueDictionary
            {
                {
                    "id",
                    board.Id
                }
            });
            }
            return View(model);
        }

        public ActionResult Update(int id)
        {
            mvcForum.Core.Category category = categoryRepo.Read(id);
            return View(new mvcForum.Web.Areas.ForumAdmin.ViewModels.Category
            {
                Id = id,
                Path = BreadcrumbHelper.BuildPath(category, base.Url)
            });
        }

        public ActionResult Delete(int id)
        {
            CategoryViewModel model = new CategoryViewModel(categoryRepo.Read(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, bool confirm)
        {
            mvcForum.Core.Category category = categoryRepo.Read(id);
            if (base.ModelState.IsValid && confirm)
            {
                categoryRepo.Delete(category);
                base.Context.SaveChanges();
                if (base.Request.IsAjaxRequest())
                {
                    return Json(true);
                }
                return RedirectToAction("Edit", "Board", new RouteValueDictionary
            {
                {
                    "id",
                    category.Board.Id
                }
            });
            }
            if (base.Request.IsAjaxRequest())
            {
                return Json(true);
            }
            CategoryViewModel model = new CategoryViewModel(category);
            return View(model);
        }

        [NonAction]
        private void AddForums(mvcForum.Core.Category category, CategoryViewModel model)
        {
            List<ForumViewModel> list = new List<ForumViewModel>();
            foreach (mvcForum.Core.Forum item in from x in category.Forums
                                                 orderby x.SortOrder
                                                 select x)
            {
                list.Add(new ForumViewModel(item, 0));
            }
            model.Forums = new ReadOnlyCollection<ForumViewModel>(list);
        }
    }

}