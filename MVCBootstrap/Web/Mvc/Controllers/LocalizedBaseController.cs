using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

using ApplicationBoilerplate.Logging;

using SimpleLocalisation;

namespace MVCBootstrap.Web.Mvc.Controllers {

	public abstract class LocalizedBaseController : Controller {
		protected readonly TextManager textManager;
		protected readonly ILogger logger;

		public const String TimeZoneInfoKey = "TZI";

		protected LocalizedBaseController() : this(null) { }

		protected LocalizedBaseController(TextManager textManager)
			: base() {

			this.textManager = (textManager == null ? DependencyResolver.Current.GetService<TextManager>() : textManager);
		}

		protected String GetLocalizedText(String key) {
			return this.textManager.Get(key, ns: this.GetType().FullName);
		}

		protected String GetLocalizedText(String key, Object values) {
			return this.textManager.Get(key, ns: this.GetType().FullName, values: values);
		}

		protected String GetLocalizedText(String key, Object values, String @namespace) {
			return this.textManager.Get(key, ns: @namespace, values: values);
		}

		/// <summary>
		/// Get the <c ref="System.TimeZoneInfo">TimeZoneInfo</c> of the current request.
		/// You should only access this property after OnActionExecuting har been called.
		/// </summary>
		protected TimeZoneInfo CurrentTimeZoneInfo {
			get {
				return (TimeZoneInfo)this.HttpContext.Items[TimeZoneInfoKey];
			}
		}

		/// <summary>
		/// Get the <c ref="System.Globalization.CultureInfo">CultureInfo</c> of the current request.
		/// You should only access this property after OnActionExecuting har been called.
		/// </summary>
		protected CultureInfo CurrentCultureInfo {
			get {
				return Thread.CurrentThread.CurrentCulture;
			}
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext) {
			base.OnActionExecuting(filterContext);

			if (filterContext == null) {
				throw new ArgumentNullException("filterContext");
			}

			if (filterContext.Controller != null) {
				try {
					// Get the default timezone from the config file.
					String configuredTZ = ConfigurationManager.AppSettings["DefaultTimezone"];
					String defaultTimezone = String.IsNullOrWhiteSpace(configuredTZ) ? TimeZoneInfo.Local.Id : configuredTZ;
					// Get the default cultureinfo from the config file.
					String configuredCI = ConfigurationManager.AppSettings["DefaultCulture"];
					String defaultCulture = String.IsNullOrWhiteSpace(configuredCI) ? CultureInfo.CurrentUICulture.Name : configuredCI;

					this.SetCulture(new CultureInfo(defaultCulture));
					this.SetTimezone(filterContext.HttpContext, TimeZoneInfo.FindSystemTimeZoneById(defaultTimezone));
				}
				catch {
					// TODO: Log the error!!
				}
			}
		}

		protected void SetCulture(CultureInfo cu) {
			Thread.CurrentThread.CurrentCulture = cu;
			Thread.CurrentThread.CurrentUICulture = cu;
		}

		protected void SetTimezone(HttpContextBase context, TimeZoneInfo tzi) {
			context.Items[TimeZoneInfoKey] = tzi;
		}
	}
}