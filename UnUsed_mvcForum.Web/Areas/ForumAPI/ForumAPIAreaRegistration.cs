// mvcForum.Web.Areas.ForumAPI.ForumAPIAreaRegistration
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAPI
{

    public class ForumAPIAreaRegistration : AreaRegistration
    {
        public override string AreaName => "forumapi";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("ForumAPI_default", "forumapi/{mode}/{controller}/{action}/{id}", new
            {
                controller = "Home",
                action = "Index",
                mode = "json",
                id = UrlParameter.Optional
            }, new string[1]
            {
            "mvcForum.Web.Areas.ForumAPI.Controllers"
            });
        }
    }

}