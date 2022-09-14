using System.Web.Mvc;

namespace CarShop.Areas.SuppRestSvc
{
    public class SuppRestSvcAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SuppRestSvc";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SuppRestSvc_default",
                "SuppRestSvc/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}