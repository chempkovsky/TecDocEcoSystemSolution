// CarShop.Areas.HelpPage.HelpPageAreaRegistration
using CarShop.Areas.HelpPage;
using System.Web.Http;
using System.Web.Mvc;

namespace CarShop.Areas.HelpPage
{
    public class HelpPageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HelpPage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HelpPage_default",
                "HelpPage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            HelpPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}