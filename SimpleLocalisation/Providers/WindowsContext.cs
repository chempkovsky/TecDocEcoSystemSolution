using System;
using System.Globalization;

namespace SimpleLocalisation.Providers {

	public class WindowsContext : ICultureContext {

		public Language Language {
			get {
				return new Language(CultureInfo.CurrentUICulture.Name);
			}
		}

		public TimeZoneInfo TimeZone {
			get {
				return TimeZoneInfo.Local;
			}
		}
	}
}
