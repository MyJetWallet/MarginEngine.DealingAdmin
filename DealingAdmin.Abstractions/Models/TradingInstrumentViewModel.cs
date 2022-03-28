using SimpleTrading.Abstraction.Trading.Instruments;
using System.Linq;

namespace DealingAdmin.Abstractions.Models
{
    public class TradingInstrumentViewModel : TradingInstrumentModel
    {
        public string LiquidityProviderId { get; set; }

        public bool DefaultLiquidityProvider { get; set; } = true;

        public static TradingInstrumentViewModel Create(
            ITradingInstrument src,
            string liquidityProviderId,
            bool isDefaultLiquidityProvider)
        {
            return new TradingInstrumentViewModel
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
                LiquidityProviderId = liquidityProviderId,
                DefaultLiquidityProvider = isDefaultLiquidityProvider
            };
        }
    }
}