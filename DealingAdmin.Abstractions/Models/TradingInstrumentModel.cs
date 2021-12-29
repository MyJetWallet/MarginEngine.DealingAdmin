using SimpleTrading.Abstraction.Trading.Instruments;
using SimpleTrading.Abstraction.Trading.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DealingAdmin.Abstractions.Models
{
    public class TradingInstrumentModel : ITradingInstrument
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public int Digits { get; set; }
        
        public string Base { get; set; }
        
        public string Quote { get; set; }

        public double TickSize { get; set; }

        public string SwapScheduleId { get; set; }
        
        public string GroupId { get; set; }

        public string? SubGroupId { get; set; }

        public int? Weight { get; set; }

        public IEnumerable<TradingInstrumentDayOffModel> DaysOff { get; set; }
        
        public int? DayTimeout { get; set; }
        
        public int? NightTimeout { get; set; }

        public bool TradingDisabled { get; set; }

        public string TokenKey { get; set; }

        IEnumerable<ITradingInstrumentDayOff> ITradingInstrument.DaysOff => DaysOff;
    }

    public class TradingInstrumentModelExt : TradingInstrumentModel
    {
        public BidAskModel BidAsk { get; set; }
        
        public static TradingInstrumentModelExt Create(ITradingInstrument src, BidAskModel bidAsk)
        {
            return new TradingInstrumentModelExt
            {
                Id = src.Id,
                Name = src.Name,
                Digits = src.Digits,
                Base = src.Base,
                Quote = src.Quote,
                TickSize = src.TickSize,
                SwapScheduleId = src.SwapScheduleId,
                GroupId = src.GroupId,
                SubGroupId = src.SubGroupId,
                Weight = src.Weight,
                DayTimeout = src.DayTimeout,
                NightTimeout = src.NightTimeout,
                TradingDisabled = src.TradingDisabled,
                DaysOff = src.DaysOff.Select(TradingInstrumentDayOffModel.Create),
                BidAsk = bidAsk
            };
        }
    }

    public class TradingInstrumentDayOffModel  : ITradingInstrumentDayOff
    {
        public int DowFrom { get; set; }

        DayOfWeek ITradingInstrumentDayOff.DowFrom => (DayOfWeek)DowFrom;
        
        public string TimeFrom { get; set; }
       
        TimeSpan ITradingInstrumentDayOff.TimeFrom => TimeSpan.Parse(TimeFrom);

        public int DowTo { get; set; }
        
        DayOfWeek ITradingInstrumentDayOff.DowTo => (DayOfWeek)DowTo;
        
        public string TimeTo { get; set; }
        
        TimeSpan ITradingInstrumentDayOff.TimeTo => TimeSpan.Parse(TimeTo);

        public static TradingInstrumentDayOffModel Create(ITradingInstrumentDayOff src)
        {
            return new TradingInstrumentDayOffModel
            {
                DowFrom = (int)src.DowFrom,
                DowTo = (int)src.DowTo,
                TimeFrom = src.TimeFrom.ToString("c"),
                TimeTo = src.TimeTo.ToString("c"),
            };
        }
    }
}