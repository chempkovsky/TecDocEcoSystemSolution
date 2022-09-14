// MVCBootstrap.Web.Mvc.Controllers.BaseController
using MVCBootstrap;
using MVCBootstrap.Web.Mvc.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Profile;

namespace MVCBootstrap.Web.Mvc.Controllers
{


    public class BaseController : LocalizedBaseController
    {
        private readonly IUserProvider userProvider;

        protected bool Authenticated => base.Request.IsAuthenticated;

        protected User ActiveUser => userProvider.ActiveUser;

        protected BaseController()
            : this(null)
        {
        }

        protected BaseController(IUserProvider userProvider)
        {
            this.userProvider = ((userProvider == null) ? DependencyResolver.Current.GetService<IUserProvider>() : userProvider);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.Controller != null && Authenticated && ProfileManager.Enabled)
            {
                try
                {
                    if (ProfileBase.Properties["Language"] != null)
                    {
                        string text = (string)filterContext.HttpContext.Profile.GetPropertyValue("Culture");
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            SetCulture(new CultureInfo(text));
                        }
                    }
                    if (ProfileBase.Properties["Timezone"] != null)
                    {
                        string text2 = (string)filterContext.HttpContext.Profile.GetPropertyValue("Timezone");
                        if (!string.IsNullOrWhiteSpace(text2))
                        {
                            SetTimezone(filterContext.HttpContext, TimeZoneInfo.FindSystemTimeZoneById(text2));
                        }
                    }
                }
                catch
                {
                }
            }
        }

        protected ContentResult CustomJson(object data)
        {
            return CustomJson(data, string.Empty, null);
        }

        protected ContentResult CustomJson(object data, string contentType)
        {
            return CustomJson(data, contentType, null);
        }

        protected ContentResult CustomJson(object data, string contentType, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                contentType = "application/json";
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return Content(JsonConvert.SerializeObject(data, new IsoDateTimeConverter
            {
                DateTimeFormat = "O"
            }, new StringEnumConverter()), contentType, encoding);
        }
    }

}
