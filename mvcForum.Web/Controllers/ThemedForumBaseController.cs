// mvcForum.Web.Controllers.ThemedForumBaseController
using ApplicationBoilerplate.DataProvider;
using mvcForum.Web.Controllers;
using mvcForum.Web.Interfaces;
using MVCThemes;

namespace mvcForum.Web.Controllers
{

    [Themed]
    public abstract class ThemedForumBaseController : ForumBaseController
    {
        protected ThemedForumBaseController(IWebUserProvider userProvider, IContext context)
            : base(userProvider, context)
        {
        }
    }

}