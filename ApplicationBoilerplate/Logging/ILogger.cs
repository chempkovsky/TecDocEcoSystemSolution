using System;

namespace ApplicationBoilerplate.Logging {

	public interface ILogger {
		void Log(EventType type, String message);
		void Log(EventType type, String message, Exception ex);
	}
}