// mvcForum.Web.Areas.ForumAdmin.Controllers.AccessMaskController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Web.Areas.ForumAdmin.Helpers;
using mvcForum.Web.Areas.ForumAdmin.ViewModels;
using mvcForum.Web.Controllers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels.Create;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{

    public class AccessMaskController : ForumAdminBaseController
    {
        private readonly IRepository<mvcForum.Core.Board> boardRepo;

        private readonly IRepository<mvcForum.Core.AccessMask> masksRepo;

        public AccessMaskController(IWebUserProvider userProvider, IContext context)
            : base(userProvider, context)
        {
            boardRepo = GetRepository<mvcForum.Core.Board>();
            masksRepo = GetRepository<mvcForum.Core.AccessMask>();
        }

        public ActionResult Create(int id)
        {
            mvcForum.Core.Board board = boardRepo.Read(id);
            CreateAccessMaskViewModel createAccessMaskViewModel = new CreateAccessMaskViewModel();
            createAccessMaskViewModel.BoardId = board.Id;
            CreateAccessMaskViewModel model = createAccessMaskViewModel;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateAccessMaskViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                mvcForum.Core.Board board = boardRepo.Read(model.BoardId);
                mvcForum.Core.AccessMask newEntity = new mvcForum.Core.AccessMask(board, model.Name, AccessFlag.Read);
                masksRepo.Create(newEntity);
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

        [Authorize(Roles = "Board Administrator,Solution Administrator")]
        public ActionResult Update(int id)
        {
            mvcForum.Core.AccessMask accessMask = masksRepo.Read(id);
            return View(new mvcForum.Web.Areas.ForumAdmin.ViewModels.AccessMask
            {
                Id = accessMask.Id,
                Path = BreadcrumbHelper.BuildPath(accessMask, base.Url)
            });
        }
    }

}