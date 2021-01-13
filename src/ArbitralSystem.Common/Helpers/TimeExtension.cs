using System;

namespace ArbitralSystem.Common.Helpers
{
    public static class TimeExtension
    {
        public static DateTime ToUtcDateTime(this long timestamp)
        {
            return  TimeHelper.TimeStampToUtcDateTime(timestamp);
        }
        public static long ToUtcTimeStamp(this DateTime dateTime)
        {
            return TimeHelper.DateTimeToTimeStamp(dateTime.ToUniversalTime());
        }
        
        public static long ToTimeStamp(this DateTime dateTime)
        {
            return TimeHelper.DateTimeToTimeStamp(dateTime);
        }
    }
}