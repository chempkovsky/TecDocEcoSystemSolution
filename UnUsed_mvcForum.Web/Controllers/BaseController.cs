// mvcForum.Web.Controllers.BaseController
using MVCBootstrap.Web.Mvc.Controllers;
using mvcForum.Core;
using mvcForum.Core.Abstractions.Interfaces;
using mvcForum.Web.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Profile;

namespace mvcForum.Web.Controllers
{

    public class BaseController : LocalizedBaseController
    {
        protected readonly IWebUserProvider userProvider;

        protected bool Authenticated => base.Request.IsAuthenticated;

        protected ForumUser ActiveUser => userProvider.ActiveUser;

        protected BaseController()
            : this(null)
        {
        }

        protected BaseController(IWebUserProvider userProvider)
        {
            this.userProvider = ((userProvider == null) ? DependencyResolver.Current.GetService<IWebUserProvider>() : userProvider);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.Controller != null)
            {
                try
                {
                    IConfiguration service = DependencyResolver.Current.GetService<IConfiguration>();
                    string text = service.DefaultCulture;
                    string text2 = service.DefaultTimezone;
                    if (Authenticated)
                    {
                        text = ActiveUser.Culture;
                        text2 = ActiveUser.Timezone;
                        if (ProfileManager.Enabled)
                        {
                            if (ProfileBase.Properties["Language"] != null)
                            {
                                text = (string)filterContext.HttpContext.Profile.GetPropertyValue("Culture");
                            }
                            if (ProfileBase.Properties["Timezone"] != null)
                            {
                                text2 = (string)filterContext.HttpContext.Profile.GetPropertyValue("Timezone");
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        SetCulture(new CultureInfo(text));
                    }
                    if (!string.IsNullOrWhiteSpace(text2))
                    {
                        SetTimezone(filterContext.HttpContext, TimeZoneInfo.FindSystemTimeZoneById(text2));
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