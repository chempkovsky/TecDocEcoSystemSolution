using System;
using System.Web.Mvc;
using System.Net;

namespace MVCBootstrap.Web.Mvc.Filters {

	[AttributeUsage(AttributeTargets.Method)]
	public class GoogleAnalyticsAttribute : ActionFilterAttribute {

		public GoogleAnalyticsAttribute()
			: base() {
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext) {
			base.OnResultExecuted(filterContext);






			//try {
			//    WebRequest connection = WebRequest.Create(utmUrl);

			//    ((HttpWebRequest)connection).UserAgent = Request.UserAgent;
			//    connection.Headers.Add("Accept-Language", Request.Headers.Get("Accept-Language"));

			//    using (WebResponse resp = connection.GetResponse()) {
			//        // Ignore response
			//    }
			//}
			//catch (Exception ex) {
			//    //if (Request.QueryString.Get("utmdebug") != null) {
			//    //    throw new Exception("Error contacting Google Analytics", ex);
			//    //}
			//}
		}
	}
}