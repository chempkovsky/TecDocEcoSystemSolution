// MVCBootstrap.DateTimeExtensions
using System;

namespace MVCBootstrap
{

    public static class DateTimeExtensions
    {
        public static DateTime ToUtc(this DateTime dt)
        {
            if (dt.Kind == DateTimeKind.Unspecified)
            {
                return new DateTime(dt.Ticks, DateTimeKind.Utc);
            }
            if (dt.Kind != DateTimeKind.Utc)
            {
                return TimeZoneInfo.ConvertTimeToUtc(dt);
            }
            return dt;
        }
    }

}
