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

    public class TradingProfileInstrumentModel : ITradingProfileInstrument
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public double MinOperationVolume { get; set; }

        [Required]
        public double MaxOperationVolume { get; set; }

        [Required]
        public double MaxPositionVolume { get; set; }

        [Required]
        public int OpenPositionMinDelayMs { get; set; }

        [Required]
        public int OpenPositionMaxDelayMs { get; set; }

        [Required]
        public bool TpSlippage { get; set; }

        [Required]
        public bool SlSlippage { get; set; }
        
        [Required]
        public bool IsTrending { get; set; }

        [Required]
        public bool OpenPositionSlippage { get; set; }

        [Required]
        public int[] Leverages { get; set; }

        public double? StopOutPercent { get; set; }

        public static TradingProfileInstrumentModel Create(ITradingProfileInstrument src)
        {
            return new TradingProfileInstrumentModel
            {
                Id = src.Id,
                MaxOperationVolume = src.MaxOperationVolume,
                MaxPositionVolume = src.MaxPositionVolume,
                MinOperationVolume = src.MinOperationVolume,
                Leverages = src.Leverages,
                SlSlippage = src.SlSlippage,
                TpSlippage = src.TpSlippage,
                IsTrending = src.IsTrending,
                OpenPositionSlippage = src.OpenPositionSlippage,
                OpenPositionMaxDelayMs = src.OpenPositionMaxDelayMs,
                OpenPositionMinDelayMs = src.OpenPositionMinDelayMs,
                StopOutPercent = src.StopOutPercent,
            };
        }
    }
}