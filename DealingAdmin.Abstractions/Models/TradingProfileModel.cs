using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SimpleTrading.Abstraction.Trading.Profiles;

namespace DealingAdmin.Abstractions.Models
{
    public class TradingProfileModel : ITradingProfile
    {
        public string Id { get; set; }

        public double MarginCallPercent { get; set; }

        public double StopOutPercent { get; set; }

        public double PositionToppingUpPercent { get; set; }

        IEnumerable<ITradingProfileInstrument> ITradingProfile.Instruments => Instruments;
        
        public List<TradingProfileInstrumentModel> Instruments { get; set; }

        public static TradingProfileModel Create(ITradingProfile src)
        {
            return new TradingProfileModel
            {
                Id = src.Id,
                MarginCallPercent = src.MarginCallPercent,
                StopOutPercent = src.StopOutPercent,
                PositionToppingUpPercent = src.PositionToppingUpPercent,
                Instruments = src.Instruments.Select(TradingProfileInstrumentModel.Create).ToList()
            };
        }

    }
}