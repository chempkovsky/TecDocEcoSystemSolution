using System;
using System.Web;

using ApplicationBoilerplate.Logging;

namespace MVCBootstrap.Web.Mvc.Services {

	public class ASPNETTraceLog : ILogger {

		public void Log(EventType type, String message) {
			this.Log(type, type.ToString(), message, null);
		}

		public void Log(EventType type, String message, Exception ex) {
			this.Log(type, type.ToString(), message, ex);
		}

		private void Log(EventType type, String category, String message, Exception ex) {
			// Are we in a web context?
			if (HttpContext.Current != null) {
				// Yes, tracing enabled?
				if (HttpContext.Current.Trace.IsEnabled) {
					// Yes, error, fatal or warning?
					if (type == EventType.Error || type == EventType.Fatal || type == EventType.Warning) {
						// Yes, let's trace a warning then!
						HttpContext.Current.Trace.Warn(category, message, ex);
					}
					else {
						// No, let's just write the trace then!
						HttpContext.Current.Trace.Write(category, message, ex);
					}
				}
			}
		}
	}
}