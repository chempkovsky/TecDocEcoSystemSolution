using System;

namespace MVCBootstrap {

	public static class DateTimeExtensions {

		/// <summary>
		/// Method for converting the input to an UTC date!
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DateTime ToUtc(this DateTime dt) {
			if (dt.Kind == DateTimeKind.Unspecified) {
				return new DateTime(dt.Ticks, DateTimeKind.Utc);
			}
			else if (dt.Kind != DateTimeKind.Utc) {
				return TimeZoneInfo.ConvertTimeToUtc(dt);
			}
			return dt;
		}
	}
}