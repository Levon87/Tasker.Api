using System;

namespace Common.Core.Extensions
{
    public static class TimeStampHelpers
    {
        public static double Today =>
            DateTime.UtcNow.Date.Subtract(new DateTime(1970, 1, 1))
                .TotalMilliseconds;

        public static double Now =>
            DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))
                .TotalMilliseconds;

        public static DateTime ConvertTimestampToDatetime(
            double unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(unixTimeStamp);
            return dateTime;
        }

        public static double ConvertDateTimeToTimestamp(
            DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1))
                .TotalMilliseconds;
        }
    }
}