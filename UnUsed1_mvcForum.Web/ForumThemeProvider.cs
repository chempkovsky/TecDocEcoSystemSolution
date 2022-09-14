// mvcForum.Web.ForumThemeProvider
using ApplicationBoilerplate.DataProvider;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Web.Interfaces;
using MVCThemes.Interfaces;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace mvcForum.Web
{

    public class ForumThemeProvider : IThemeProvider
    {
        private const string themeKey = "mcvForumTheme";

        public void SetTheme(ActionExecutingContext filterContext)
        {
            IConfiguration service = DependencyResolver.Current.GetService<IConfiguration>();
            string theme = service.Theme;
            if (service.AllowUserDefinedTheme)
            {
                IWebUserProvider service2 = DependencyResolver.Current.GetService<IWebUserProvider>();
                if (service2 != null && service2.Authenticated)
                {
                    DependencyResolver.Current.GetService<IRepository<ForumUser>>();
                    ForumUser activeUser = service2.ActiveUser;
                    if (activeUser != null && !string.IsNullOrWhiteSpace(activeUser.Theme))
                    {
                        theme = activeUser.Theme;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(theme) && Directory.Exists(Path.Combine(filterContext.HttpContext.Server.MapPath("~/themes"), theme)))
            {
                filterContext.HttpContext.Items.Add("mcvForumTheme", theme);
            }
        }

        private string GetTheme(IDictionary items)
        {
            object obj = items["mcvForumTheme"];
            if (obj != null && !string.IsNullOrWhiteSpace((string)obj))
            {
                return (string)obj;
            }
            return string.Empty;
        }

        public string GetTheme(ControllerContext controllerContext)
        {
            return GetTheme(controllerContext.HttpContext.Items);
        }

        public string GetTheme()
        {
            return GetTheme(HttpContext.Current.Items);
        }
    }

}