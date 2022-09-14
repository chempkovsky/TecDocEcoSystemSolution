using System;
using System.Globalization;

namespace SimpleLocalisation {

	/// <summary>
	/// Interface for getting the current language and time-zone.
	/// </summary>
	public interface ICultureContext {
		/// <summary>
		/// The current language.
		/// </summary>
		Language Language { get; }
		/// <summary>
		/// The current time-zone.
		/// </summary>
		TimeZoneInfo TimeZone { get; }
	}
}