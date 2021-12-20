using System;

namespace DealingAdmin
{
    public static class FormatUtils
    {
        public static string TimeSpanToString(TimeSpan ts)
        {
            return string.Format("{0} {1} {2} {3}",
                ts.Days > 0 ? string.Format("{0:0} day{1} ", ts.Days, ts.Days == 1 ? string.Empty : "s") : string.Empty,
                ts.Hours > 0 ? string.Format("{0:0}h", ts.Hours) : string.Empty,
                ts.Minutes > 0 ? string.Format("{0:0}m ", ts.Minutes) : string.Empty,
                ts.Seconds > 0 ? string.Format("{0:0}s", ts.Seconds) : string.Empty);
        }

        public static string DateTimeFormat(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string DateTimeNamedWithMsFormat(DateTime dt)
        {
            return dt.ToString("ddd, dd MMM yyy HH:mm:ss.fff");
        }
    }
}
