// mvcForum.Web.Areas.ForumAdmin.Controllers.ForumAccessController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Specifications;
using mvcForum.Web.Controllers;
using mvcForum.Web.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{

    public class ForumAccessController : ForumAdminBaseController
    {
        private readonly IRepository<mvcForum.Core.Forum> forumRepo;

        private readonly IRepository<ForumAccess> accessRepo;

        private readonly IRepository<Group> groupRepo;

        private readonly IRepository<AccessMask> masksRepo;

        public ForumAccessController(IWebUserProvider userProvider, IContext context)
            : base(userProvider, context)
        {
            forumRepo = GetRepository<mvcForum.Core.Forum>();
            accessRepo = GetRepository<ForumAccess>();
            groupRepo = GetRepository<Group>();
            masksRepo = GetRepository<AccessMask>();
        }

        [HttpPost]
        public ActionResult Update(int id)
        {
            mvcForum.Core.Forum forum = forumRepo.Read(id);
            IEnumerable<ForumAccess> enumerable = accessRepo.ReadMany(new ForumAccessSpecifications.SpecificForum(forum));
            foreach (ForumAccess item in enumerable)
            {
                accessRepo.Delete(item);
            }
            base.Context.SaveChanges();
            IEnumerable<Group> enumerable2 = groupRepo.ReadAll().ToList();
            foreach (Group item2 in enumerable2)
            {
                string text = $"Mask{item2.Id}";
                if (text.Length > 4 && !string.IsNullOrWhiteSpace(base.Request.Form[text]) && int.TryParse(base.Request.Form[text], out int result))
                {
                    AccessMask mask = masksRepo.Read(result);
                    ForumAccess newEntity = new ForumAccess(forum, item2, mask);
                    accessRepo.Create(newEntity);
                    base.Context.SaveChanges();
                }
            }
            return RedirectToAction("Edit", "Forum", new RouteValueDictionary
        {
            {
                "id",
                forum.Id
            }
        });
        }
    }

}