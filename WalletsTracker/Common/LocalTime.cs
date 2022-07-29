using System;

namespace WalletsTracker.Common
{
    public static class LocalTime
    {
        public static DateTime Now()
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            return localTime.DateTime;
        }
    }
}
