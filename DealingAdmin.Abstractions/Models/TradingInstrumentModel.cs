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

        public List<TradingInstrumentDayOffModel> DaysOff { get; set; }
        
        public int? DayTimeout { get; set; }
        
        public int? NightTimeout { get; set; }

        public bool TradingDisabled { get; set; }

        public string TokenKey { get; set; }

        public int? MarginCallPercent { get; set; }

        IEnumerable<ITradingInstrumentDayOff> ITradingInstrument.DaysOff => DaysOff;

        public static TradingInstrumentModel Create(ITradingInstrument src)
        {
            return new TradingInstrumentModel
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
                DaysOff = src.DaysOff.Select(TradingInstrumentDayOffModel.Create).ToList(),
                MarginCallPercent = src.MarginCallPercent,
            };
        }
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
                DaysOff = src.DaysOff.Select(TradingInstrumentDayOffModel.Create).ToList(),
                MarginCallPercent = src.MarginCallPercent,
                BidAsk = bidAsk
            };
        }
    }

    public class TradingInstrumentDayOffModel  : ITradingInstrumentDayOff
    {
        public DayOfWeek DowFrom { get; set; }

        public TimeSpan TimeFrom { get; set; }
       
        public DayOfWeek DowTo { get; set; }
        
        public TimeSpan TimeTo { get; set; }
        
        public static TradingInstrumentDayOffModel Create(ITradingInstrumentDayOff src)
        {
            return new TradingInstrumentDayOffModel
            {
                DowFrom = src.DowFrom,
                DowTo = src.DowTo,
                TimeFrom = src.TimeFrom,
                TimeTo = src.TimeTo,
            };
        }
    }
}