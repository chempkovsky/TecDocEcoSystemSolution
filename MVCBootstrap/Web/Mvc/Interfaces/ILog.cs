using System;

namespace MVCBootstrap.MVC.Interfaces {

	public interface ILog {
		void Log(EventType type, String message);
		void Log(EventType type, String message, Exception ex);
	}
}