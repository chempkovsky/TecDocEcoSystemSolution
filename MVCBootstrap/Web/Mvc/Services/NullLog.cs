using System;

using ApplicationBoilerplate.Logging;

namespace MVCBootstrap.Web.Mvc.Services {

	public class NullLog : ILogger {

		public void Log(EventType type, string message) { }

		public void Log(EventType type, string message, Exception ex) { }
	}
}