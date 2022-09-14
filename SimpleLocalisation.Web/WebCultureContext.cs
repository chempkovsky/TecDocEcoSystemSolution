using System;
using SimpleLocalisation.Providers;
using System.Web;

namespace SimpleLocalisation.Web {

	public class WebCultureContext : WindowsContext {

		public new TimeZoneInfo TimeZone {
			get {
				if (HttpContext.Current != null && HttpContext.Current.Items.Contains("TZI")) {
					return HttpContext.Current.Items["TZI"] as TimeZoneInfo;
				}
				return TimeZoneInfo.Local;
			}
		}
	}
}