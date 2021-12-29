using SimpleTrading.Abstraction.Trading.Settings;
using System;

namespace DealingAdmin
{
    public static class CommonUtils
    {
        public static bool IsDateBelongToDayOff(DateTime dt, ITradingInstrumentDayOff dayOff)
        {
            if (dayOff.DowFrom > dayOff.DowTo)
            {
                var checkTilSunday = IsDateBelongToDayOff(
                   dt,
                   dayOff.DowFrom,
                   dayOff.TimeFrom,
                   DayOfWeek.Saturday,
                   new TimeSpan(23, 59, 59, 59));

                var checkFromSunday = IsDateBelongToDayOff(
                    dt,
                    DayOfWeek.Sunday,
                    new TimeSpan(),
                    dayOff.DowTo,
                    dayOff.TimeTo);

                return checkTilSunday || checkFromSunday;
            }

            return IsDateBelongToDayOff(
                dt,
                dayOff.DowFrom,
                dayOff.TimeFrom,
                dayOff.DowTo,
                dayOff.TimeTo);
        }

        /// <summary>
        /// Check for Instrument DayOff
        /// (dowFrom should be bigger than dowTo)
        /// </summary>
        private static bool IsDateBelongToDayOff(
                DateTime dt,
                DayOfWeek dowFrom,
                TimeSpan timeFrom,
                DayOfWeek dowTo,
                TimeSpan timeTo)
        {
            var day = dt.DayOfWeek;
            var time = dt.TimeOfDay;

            if (day > dowFrom && day < dowTo)
            {
                return true;
            }
            else if (day == dowFrom && time > timeFrom)
            {
                if (dowTo == day)
                {
                    return time < timeTo;
                }
                return true;
            }
            else if (day == dowTo && time < timeTo)
            {
                return true;
            }

            return false;
        }
    }
}
