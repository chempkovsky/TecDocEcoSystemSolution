// mvcForum.Web.Areas.ForumAdmin.Controllers.AntiSpamController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Web.Areas.ForumAdmin.ViewModels;
using mvcForum.Web.Controllers;
using mvcForum.Web.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAdmin.Controllers
{

    public class AntiSpamController : ForumAdminBaseController
    {
        private readonly IEnumerable<IAntiSpamConfigurationController> controllers;

        public AntiSpamController(IWebUserProvider userProvider, IContext context, IEnumerable<IAntiSpamConfigurationController> controllers)
            : base(userProvider, context)
        {
            this.controllers = controllers;
        }

        public ActionResult Index()
        {
            return View(from c in controllers
                        select new AddOn
                        {
                            Name = c.Name,
                            Description = c.Description,
                            ControllerName = c.GetType().Name.Substring(0, c.GetType().Name.Length - "Controller".Length)
                        });
        }
    }

}