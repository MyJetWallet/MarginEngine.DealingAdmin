using Microsoft.AspNetCore.Components.Forms;
using SimpleTrading.Abstraction.Trading.Settings;
using System;
using System.IO;
using System.Threading.Tasks;

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
                    new TimeSpan(0, 0, 0),
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

            if (day > dowFrom)
            {
                if (day < dowTo)
                {
                    return true;
                }

                return (day == dowTo && time < timeTo);
            }
            else if (day == dowFrom && time > timeFrom)
            {
                if (dowTo == day)
                {
                    return time < timeTo;
                }
                return true;
            }

            return false;
        }

        public static async Task<byte[]> FileToBytesAsync(IBrowserFile file)
        {
            using var memStream = new MemoryStream();

            // although file.OpenReadStream is itself a stream,
            // using it directly causes "Synchronous reads are not supported" error
            await file.OpenReadStream().CopyToAsync(memStream);

            // at the end of the copy method, we are at the end of both the input and output stream
            // and need to reset the one we want to work with.
            memStream.Seek(0, SeekOrigin.Begin);

            return memStream.ToArray();
        }
    }
}
