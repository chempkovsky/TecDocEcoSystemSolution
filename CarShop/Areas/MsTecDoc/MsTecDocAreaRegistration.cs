using System.Web.Mvc;

namespace CarShop.Areas.MsTecDoc
{
    public class MsTecDocAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MsTecDoc";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MsTecDoc_default",
                "MsTecDoc/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}