// mvcForum.Web.Areas.ForumAdmin.Controllers.BoardController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Interfaces.Services;
using mvcForum.Web.Areas.ForumAdmin.Helpers;
using mvcForum.Web.Areas.ForumAdmin.ViewModels;
using mvcForum.Web.Controllers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{

    public class BoardController : ForumAdminBaseController
    {
        private readonly IRepository<mvcForum.Core.Board> boardRepo;

        private readonly IMembershipService membershipService;

        public BoardController(IWebUserProvider userProvider, IContext context, IMembershipService membershipService)
            : base(userProvider, context)
        {
            boardRepo = GetRepository<mvcForum.Core.Board>();
            this.membershipService = membershipService;
        }

        public ActionResult Create()
        {
            if (base.Request.IsAjaxRequest())
            {
                return PartialView("CreateBoardPartial", new BoardViewModel());
            }
            return View(new BoardViewModel());
        }

        [HttpPost]
        public ActionResult Create(BoardViewModel model)
        {
            if (membershipService.IsAccountInRole(base.ActiveUser.EmailAddress, "Solution Administrator"))
            {
                if (base.ModelState.IsValid)
                {
                    mvcForum.Core.Board board = new mvcForum.Core.Board();
                    board.Name = model.Name;
                    mvcForum.Core.Board newEntity = board;
                    boardRepo.Create(newEntity);
                    base.Context.SaveChanges();
                    if (base.Request.IsAjaxRequest())
                    {
                        return Json(true);
                    }
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }
            base.TempData.Add("Reason", "You do not have access to creating new boards");
            return RedirectToAction("NoAccess", "Forum");
        }

        public ActionResult Update(int id)
        {
            mvcForum.Core.Board board = GetRepository<mvcForum.Core.Board>().Read(id);
            return View(new mvcForum.Web.Areas.ForumAdmin.ViewModels.Board
            {
                Id = id,
                Path = BreadcrumbHelper.BuildPath(board, base.Url)
            });
        }

        public ActionResult Delete(int id)
        {
            BoardViewModel model = new BoardViewModel(boardRepo.Read(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, bool confirm)
        {
            mvcForum.Core.Board board = boardRepo.Read(id);
            if (base.ModelState.IsValid && confirm)
            {
                boardRepo.Delete(board);
                base.Context.SaveChanges();
                if (base.Request.IsAjaxRequest())
                {
                    return Json(true);
                }
                return RedirectToAction("Index", "Home");
            }
            if (base.Request.IsAjaxRequest())
            {
                return Json(false);
            }
            BoardViewModel model = new BoardViewModel(board);
            return View(model);
        }

        public ActionResult AttachmentSettings()
        {
            return View();
        }

        public ActionResult Features()
        {
            return View();
        }

        public ActionResult AvatarSettings()
        {
            return View();
        }

        public ActionResult PrivateMessagingSettings()
        {
            return View();
        }

        public ActionResult PostSettings()
        {
            return View();
        }

        public ActionResult SignatureSettings()
        {
            return View();
        }
    }

}