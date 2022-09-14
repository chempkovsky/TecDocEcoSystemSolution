// mvcForum.Web.Areas.ForumAdmin.ForumAdminAreaRegistration
using mvcForum.Web.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace mvcForum.Web.Areas.ForumAdmin
{

    public class ForumAdminAreaRegistration : AreaRegistration
    {
        public override string AreaName => "ForumAdmin";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            IEnumerable<string> source = new string[1]
            {
            "mvcForum.Web.Areas.ForumAdmin.Controllers"
            }.Concat((from c in DependencyResolver.Current.GetServices<IAntiSpamConfigurationController>()
                      select c.GetType().Namespace).Distinct().Concat((from c in DependencyResolver.Current.GetServices<ISearchConfigurationController>()
                                                                       select c.GetType().Namespace).Distinct()));
            context.MapRoute("ForumAdmin_default", "forumadmin/{controller}/{action}/{id}", new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional,
                area = "ForumAdmin"
            }, source.Distinct().ToArray());
        }
    }

}