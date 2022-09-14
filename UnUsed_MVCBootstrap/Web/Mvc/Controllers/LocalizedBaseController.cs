// MVCBootstrap.Web.Mvc.Controllers.LocalizedBaseController
using ApplicationBoilerplate.Logging;
using SimpleLocalisation;
using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Controllers
{

    public abstract class LocalizedBaseController : Controller
    {
        public const string TimeZoneInfoKey = "TZI";

        protected readonly TextManager textManager;

        protected readonly ILogger logger;

        protected TimeZoneInfo CurrentTimeZoneInfo => (TimeZoneInfo)base.HttpContext.Items["TZI"];

        protected CultureInfo CurrentCultureInfo => Thread.CurrentThread.CurrentCulture;

        protected LocalizedBaseController()
            : this(null)
        {
        }

        protected LocalizedBaseController(TextManager textManager)
        {
            this.textManager = ((textManager == null) ? DependencyResolver.Current.GetService<TextManager>() : textManager);
        }

        protected string GetLocalizedText(string key)
        {
            TextManager obj = textManager;
            string fullName = GetType().FullName;
            return obj.Get(key, null, fullName);
        }

        protected string GetLocalizedText(string key, object values)
        {
            TextManager obj = textManager;
            string fullName = GetType().FullName;
            return obj.Get(key, values, fullName);
        }

        protected string GetLocalizedText(string key, object values, string @namespace)
        {
            return textManager.Get(key, values, @namespace);
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
                    string text = ConfigurationManager.AppSettings["DefaultTimezone"];
                    string id = string.IsNullOrWhiteSpace(text) ? TimeZoneInfo.Local.Id : text;
                    string text2 = ConfigurationManager.AppSettings["DefaultCulture"];
                    string name = string.IsNullOrWhiteSpace(text2) ? CultureInfo.CurrentUICulture.Name : text2;
                    SetCulture(new CultureInfo(name));
                    SetTimezone(filterContext.HttpContext, TimeZoneInfo.FindSystemTimeZoneById(id));
                }
                catch
                {
                }
            }
        }

        protected void SetCulture(CultureInfo cu)
        {
            Thread.CurrentThread.CurrentCulture = cu;
            Thread.CurrentThread.CurrentUICulture = cu;
        }

        protected void SetTimezone(HttpContextBase context, TimeZoneInfo tzi)
        {
            context.Items["TZI"] = tzi;
        }
    }

}
