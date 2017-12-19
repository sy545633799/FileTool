using System;

namespace StarEdit.Tools
{
    class TimeTool
    {
        public static DateTime UnixTimeToDateTime(int sec)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return startTime.AddSeconds(sec);
        }
    }
}
