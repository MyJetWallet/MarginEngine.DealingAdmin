using System;

namespace DealingAdmin.Abstractions
{
    public interface ITradingInstrumentDayOff
    {
        DayOfWeek DowFrom { get; }

        TimeSpan TimeFrom { get; }

        DayOfWeek DowTo { get; }

        TimeSpan TimeTo { get; }
    }
}