using System;
using System.Text;
using System.Web.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.Profile;
using System.Globalization;

namespace MVCBootstrap.Web.Mvc.Controllers {

	public class BaseController : LocalizedBaseController {
		private readonly IUserProvider userProvider;

		protected BaseController() : this(null) { }

		protected BaseController(IUserProvider userProvider)
			: base() {

			this.userProvider = (userProvider == null ? DependencyResolver.Current.GetService<IUserProvider>() : userProvider);
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext) {
			base.OnActionExecuting(filterContext);

			if (filterContext == null) {
				throw new ArgumentNullException("filterContext");
			}

			if (filterContext.Controller != null) {
				if (this.Authenticated) {
					if (ProfileManager.Enabled) {
						try {
							if (ProfileBase.Properties["Language"] != null) {
								String userCulture = (String)filterContext.HttpContext.Profile.GetPropertyValue("Culture");
								if (!String.IsNullOrWhiteSpace(userCulture)) {
									this.SetCulture(new CultureInfo(userCulture));
								}
							}
							if (ProfileBase.Properties["Timezone"] != null) {
								String userTimezone = (String)filterContext.HttpContext.Profile.GetPropertyValue("Timezone");
								if (!String.IsNullOrWhiteSpace(userTimezone)) {
									this.SetTimezone(filterContext.HttpContext, TimeZoneInfo.FindSystemTimeZoneById(userTimezone));
								}
							}
						}
						catch {
							// TODO: Log this!!
						}
					}
				}
			}
		}

		/// <summary>
		/// Boolean indicating whether or not the request is done by an authenticated user.
		/// </summary>
		protected Boolean Authenticated {
			get {
				return Request.IsAuthenticated;
			}
		}

		/// <summary>
		/// Get the authenticated user.
		/// </summary>
		protected User ActiveUser {
			get {
				return this.userProvider.ActiveUser;
			}
		}

		/// <summary>
		/// Method using Newtonsoft's Json library to convert the data to JSON output.
		/// The JavascriptDateTimeConverter and StringEnumConverter is used as formatters.
		/// The output content type is 'application/json' and the encoding is UTF-8.
		/// </summary>
		/// <param name="data">The data that will be converted to JSON.</param>
		/// <returns></returns>
		protected ContentResult CustomJson(Object data) {
			return this.CustomJson(data, String.Empty, null);
		}

		/// <summary>
		/// Method using Newtonsoft's Json library to convert the data to JSON output.
		/// The JavascriptDateTimeConverter and StringEnumConverter is used as formatters.
		/// The output encoding is UTF-8.
		/// </summary>
		/// <param name="data">The data that will be converted to JSON.</param>
		/// <param name="contentType">The output content type.</param>
		/// <returns></returns>
		protected ContentResult CustomJson(Object data, String contentType) {
			return this.CustomJson(data, contentType, null);
		}

		/// <summary>
		/// Method using Newtonsoft's Json library to convert the data to JSON output.
		/// The JavascriptDateTimeConverter and StringEnumConverter is used as formatters.
		/// </summary>
		/// <param name="data">The data that will be converted to JSON.</param>
		/// <param name="contentType">The output content type.</param>
		/// <param name="encoding">The output encoding.</param>
		/// <returns></returns>
		protected ContentResult CustomJson(Object data, String contentType, Encoding encoding) {
			if (String.IsNullOrWhiteSpace(contentType)) {
				contentType = "application/json";
			}
			if (encoding == null) {
				encoding = Encoding.UTF8;
			}
			return Content(JsonConvert.SerializeObject(data, new IsoDateTimeConverter() { DateTimeFormat = "O" }, new StringEnumConverter()), contentType, encoding);
		}
	}
}