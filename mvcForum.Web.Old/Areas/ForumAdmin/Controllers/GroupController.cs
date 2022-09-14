// mvcForum.Web.Areas.ForumAdmin.Controllers.GroupController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Web.Areas.ForumAdmin.Helpers;
using mvcForum.Web.Areas.ForumAdmin.ViewModels;
using mvcForum.Web.Controllers;
using mvcForum.Web.Interfaces;
using mvcForum.Web.ViewModels;
using mvcForum.Web.ViewModels.Create;
using System.Collections.Generic;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{

    public class GroupController : ForumAdminBaseController
    {
        private readonly IRepository<mvcForum.Core.Group> groupRepo;

        public GroupController(IWebUserProvider userProvider, IContext context)
            : base(userProvider, context)
        {
            groupRepo = GetRepository<mvcForum.Core.Group>();
        }

        public ActionResult Create()
        {
            return View(new CreateGroupViewModel());
        }

        [HttpPost]
        public ActionResult Create(CreateGroupViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                mvcForum.Core.Group newEntity = new mvcForum.Core.Group(model.Name);
                groupRepo.Create(newEntity);
                base.Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Index()
        {
            IEnumerable<mvcForum.Core.Group> enumerable = groupRepo.ReadAll();
            List<GroupViewModel> list = new List<GroupViewModel>();
            foreach (mvcForum.Core.Group item in enumerable)
            {
                list.Add(new GroupViewModel(item));
            }
            return View(list);
        }

        public ActionResult Update(int id)
        {
            mvcForum.Core.Group group = groupRepo.Read(id);
            return View(new mvcForum.Web.Areas.ForumAdmin.ViewModels.Group
            {
                Id = group.Id,
                Path = BreadcrumbHelper.BuildPath(group, base.Url)
            });
        }

        [HttpPost]
        public ActionResult Edit(GroupViewModel model)
        {
            if (base.ModelState.IsValid)
            {
                mvcForum.Core.Group group = groupRepo.Read(model.Id);
                group.Name = model.Name;
                base.Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            GroupViewModel model = new GroupViewModel(groupRepo.Read(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, bool confirm)
        {
            mvcForum.Core.Group group = groupRepo.Read(id);
            if (base.ModelState.IsValid && confirm)
            {
                groupRepo.Delete(group);
                base.Context.SaveChanges();
                if (base.Request.IsAjaxRequest())
                {
                    return Json(true);
                }
                return RedirectToAction("Index");
            }
            if (base.Request.IsAjaxRequest())
            {
                return Json(false);
            }
            GroupViewModel model = new GroupViewModel(group);
            return View(model);
        }
    }

}