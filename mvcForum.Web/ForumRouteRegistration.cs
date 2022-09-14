// mvcForum.Web.ForumRouteRegistration
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcForum.Web
{

    public class ForumRouteRegistration
    {
        public void RegisterRoutes(RouteCollection routes)
        {
        }

        public void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute("ShowTopic", "forum/viewtopic/{title}/{id}/{additional}", new
            {
                area = "forum",
                controller = "Topic",
                action = "Index",
                additional = UrlParameter.Optional
            });
            context.MapRoute("ShowCategory", "forum/viewcategory/{title}/{id}", new
            {
                area = "forum",
                controller = "Category",
                action = "Index"
            });
            context.MapRoute("ShowProfile", "forum/viewprofile/{id}/{name}", new
            {
                area = "forum",
                controller = "Profile",
                action = "Index",
                id = UrlParameter.Optional
            });
            context.MapRoute("NoAccess", "forum/noaccess", new
            {
                area = "forum",
                controller = "NoAccess",
                action = "Index"
            });
            context.MapRoute("ShowForum", "forum/viewforum/{title}/{id}", new
            {
                area = "forum",
                controller = "Forum",
                action = "Index"
            });
            context.MapRoute("Forum_default", "forum/{controller}/{action}/{id}", new
            {
                area = "forum",
                controller = "home",
                action = "index",
                id = UrlParameter.Optional
            }, new string[1]{"mvcForum.Web.Areas.Forum.Controllers"});
        }
    }

}