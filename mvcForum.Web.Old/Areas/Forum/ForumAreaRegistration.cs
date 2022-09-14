// mvcForum.Web.Areas.Forum.ForumAreaRegistration
using mvcForum.Web;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.Forum
{

    public class ForumAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Forum";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            new ForumRouteRegistration().RegisterRoutes(context);
        }
    }

}