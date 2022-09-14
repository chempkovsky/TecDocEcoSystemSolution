// mvcForum.Web.Controllers.ForumAdminBaseController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Web.Controllers;
using mvcForum.Web.Interfaces;
using System.Web.Mvc;

namespace mvcForum.Web.Controllers
{

    [Authorize(Roles = "Board Administrator,Solution Administrator")]
    public abstract class ForumAdminBaseController : ForumBaseController
    {
        protected ForumAdminBaseController(IWebUserProvider userProvider, IContext context)
            : base(userProvider, context)
        {
        }
    }

}