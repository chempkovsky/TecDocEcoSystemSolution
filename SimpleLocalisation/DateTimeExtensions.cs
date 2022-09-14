using System;

namespace SimpleLocalisation {

	/// <summary>
	/// Extensions for the <see cref="DateTime"/> struct.
	/// </summary>
	public static class DateTimeExtensions {
		/// <summary>
		/// Adjusts the date to the local (server) <see cref="TimeZoneInfo"/>.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns>A date time adjusted to the local (server) time zone.</returns>
		public static DateTime AdjustToTimeZone(this DateTime date) {
			return date.AdjustToTimeZone(TimeZoneInfo.Local);
		}

		/// <summary>
		/// Adjusts the date to the <see cref="TimeZoneInfo"/> specified.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="timeZoneInfo">The time zone info.</param>
		/// <returns>A date time adjusted to the given time zone.</returns>
		public static DateTime AdjustToTimeZone(this DateTime date, TimeZoneInfo timeZoneInfo) {
			return TimeZoneInfo.ConvertTimeFromUtc(date.ToUniversalTime(), timeZoneInfo);
		}

		/// <summary>
		/// Adjusts the date to the current <see cref="TimeZoneInfo"/>.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="context">The <see cref="ICultureContext"/>.</param>
		/// <returns>A date time adjusted to the current time zone.</returns>
		public static DateTime AdjustToTimeZone(this DateTime date, ICultureContext context) {
			return date.AdjustToTimeZone(context.TimeZone);
		}
	}
}