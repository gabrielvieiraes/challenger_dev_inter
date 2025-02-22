using TimeZoneConverter;

namespace Aevo.ChallengeDev.WebApi.Utils
{
    public class TimezoneUtil
    {
        public static DateTime ConvertDateToUtc(DateTime date, string timezone)
        {
            date = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);

            var sourceTimeZone = TZConvert.GetTimeZoneInfo(timezone);
            var destinationTimeZone = TZConvert.GetTimeZoneInfo("UTC");

            return TimeZoneInfo.ConvertTime(date, sourceTimeZone, destinationTimeZone);
        }

        public static DateTime ConvertDateToTimezone(DateTime date, string timezone)
        {
            date = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);

            var utcTimeZone = TZConvert.GetTimeZoneInfo("UTC");
            var timeZone = TZConvert.GetTimeZoneInfo(timezone);

            return TimeZoneInfo.ConvertTime(date, utcTimeZone, timeZone);
        }

        public static DateTime ConvertDateNowToUTC(string timezoneIdentifier)
        {
            var timeZone = TZConvert.GetTimeZoneInfo(timezoneIdentifier);
            var utcTimeZone = TZConvert.GetTimeZoneInfo("UTC");

            var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            var nowTimezone = TimeZoneInfo.ConvertTime(now, utcTimeZone, timeZone);
            nowTimezone = nowTimezone.Date;

            return TimeZoneInfo.ConvertTime(nowTimezone, timeZone, utcTimeZone);
        }
    }
}
