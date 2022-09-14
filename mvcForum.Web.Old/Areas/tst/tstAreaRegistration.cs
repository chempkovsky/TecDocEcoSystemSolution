using System.Web.Mvc;

namespace mvcForum.Web.Areas.tst
{
    public class tstAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "tst";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "tst_default",
                "tst/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}