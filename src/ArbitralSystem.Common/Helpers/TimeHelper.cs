using System;

namespace ArbitralSystem.Common.Helpers
{
    public static class TimeHelper
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        public static long DateTimeToTimeStamp(DateTime value, bool isJavaTimeStamp = true)
        {
            var elapsedTime = value - Epoch;
            return isJavaTimeStamp ? (long) elapsedTime.TotalMilliseconds : (long) elapsedTime.TotalSeconds;
        }

        public static long GetTimeStampNow(bool isJavaTimeStamp = true)
        {
            return DateTimeToTimeStamp(DateTime.Now);
        }
        
        public static long GetTimeStampUtcNow(bool isJavaTimeStamp = true)
        {
            return DateTimeToTimeStamp(DateTime.UtcNow);
        }
        
        public static DateTime TimeStampToUtcDateTime(long unixTimeStamp, bool isJavaTimeStamp = true)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = !isJavaTimeStamp
                ? dtDateTime.AddSeconds(unixTimeStamp)
                : dtDateTime.AddMilliseconds(unixTimeStamp);
            return dtDateTime;
        }
        
        public static DateTime TimeStampToDateTime(double unixTimeStamp, bool isJavaTimeStamp = true)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = !isJavaTimeStamp
                ? dtDateTime.AddSeconds(unixTimeStamp)
                : dtDateTime.AddMilliseconds(unixTimeStamp);
            return dtDateTime;
        }
    }
}