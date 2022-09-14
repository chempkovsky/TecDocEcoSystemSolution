using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{
    // mvcForum.Web.Areas.ForumAdmin.Controllers.UserController
    using ApplicationBoilerplate.DataProvider;
    using mvcForum.Core;
    using mvcForum.Core.Specifications;
    using mvcForum.Web.Areas.ForumAdmin.Helpers;
    using mvcForum.Web.Areas.ForumAdmin.ViewModels;
    using mvcForum.Web.Controllers;
    using mvcForum.Web.Extensions;
    using mvcForum.Web.Interfaces;
    using mvcForum.Web.ViewModels;
    using mvcForum.Web.ViewModels.Create;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class UserController : ForumAdminBaseController
    {
        private readonly IRepository<mvcForum.Core.ForumUser> userRepo;

        private readonly IRepository<mvcForum.Core.Group> groupRepo;

        private readonly IRepository<GroupMember> gmRepo;

        private readonly IRepository<BannedIP> bipRepo;

        public UserController(IWebUserProvider userProvider, IContext context)
            : base(userProvider, context)
        {
            userRepo = GetRepository<mvcForum.Core.ForumUser>();
            groupRepo = GetRepository<mvcForum.Core.Group>();
            gmRepo = GetRepository<GroupMember>();
            bipRepo = GetRepository<BannedIP>();
        }

        public ActionResult Create()
        {
            CreateForumUserViewModel model = new CreateForumUserViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateForumUserViewModel model)
        {
            return View(model);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Update(int id)
        {
            mvcForum.Core.ForumUser forumUser = GetRepository<mvcForum.Core.ForumUser>().Read(id);
            return View(new mvcForum.Web.Areas.ForumAdmin.ViewModels.ForumUser
            {
                Id = forumUser.Id,
                Path = BreadcrumbHelper.BuildPath(forumUser, base.Url)
            });
        }

        [NonAction]
        private void PopulateForumUserViewModel(ForumUserViewModel model, IEnumerable<mvcForum.Core.Group> groups, IEnumerable<mvcForum.Core.Group> memberOf)
        {
            List<GroupViewModel> list = new List<GroupViewModel>();
            foreach (mvcForum.Core.Group group in groups)
            {
                list.Add(new GroupViewModel(group));
            }
            model.Groups = from x in list
                           orderby x.Name
                           select x;
            List<GroupViewModel> list2 = new List<GroupViewModel>();
            foreach (mvcForum.Core.Group item in memberOf)
            {
                list2.Add(new GroupViewModel(item));
            }
            model.MemberOf = list2;
        }

        [HttpPost]
        public ActionResult Edit(ForumUserViewModel model)
        {
            mvcForum.Core.ForumUser forumUser = userRepo.Read(model.Id);
            bool isValid = base.ModelState.IsValid;
            model.Name = forumUser.Name;
            model.FirstVisit = forumUser.FirstVisit;
            model.LastVisit = forumUser.LastVisit;
            model.LastIP = forumUser.LastIP;
            IEnumerable<mvcForum.Core.Group> groups = groupRepo.ReadAll();
            PopulateForumUserViewModel(model, groups, forumUser.Groups());
            return View(model);
        }

        [HttpPost]
        public ActionResult RemoveMember(int id, int groupId, bool confirm)
        {
            if (confirm)
            {
                mvcForum.Core.ForumUser user = userRepo.Read(id);
                IEnumerable<GroupMember> enumerable = gmRepo.ReadMany(new GroupMemberSpecifications.SpecificUser(user));
                foreach (GroupMember item in enumerable)
                {
                    if (item.Group.Id == groupId && item.ForumUser.Id == id)
                    {
                        gmRepo.Delete(item);
                        break;
                    }
                }
                base.Context.SaveChanges();
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddMember(int id, int groupId)
        {
            mvcForum.Core.ForumUser forumUser = userRepo.Read(id);
            mvcForum.Core.Group group = groupRepo.Read(groupId);
            if (!gmRepo.ReadMany(new GroupMemberSpecifications.SpecificUserAndGroup(forumUser, group)).Any())
            {
                gmRepo.Create(new GroupMember(group, forumUser));
                base.Context.SaveChanges();
            }
            return RedirectToAction("Edit", new
            {
                id = forumUser.Id
            });
        }

        public ActionResult Delete(int id)
        {
            ForumUserViewModel model = new ForumUserViewModel(userRepo.Read(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, bool confirm)
        {
            mvcForum.Core.ForumUser forumUser = userRepo.Read(id);
            if (base.ModelState.IsValid && confirm)
            {
                userRepo.Delete(forumUser);
                base.Context.SaveChanges();
                return RedirectToAction("Index");
            }
            ForumUserViewModel model = new ForumUserViewModel(forumUser);
            return View(model);
        }

        public ActionResult BanIP(int userId, string ip)
        {
            BannedIP bannedIP = bipRepo.ReadOne(new BannedIPSpecifications.SpecificIP(ip));
            if (bannedIP == null)
            {
                BannedIP newEntity = new BannedIP(ip);
                bipRepo.Create(newEntity);
                base.Context.SaveChanges();
            }
            return RedirectToAction("Edit", new
            {
                id = userId
            });
        }
    }

}