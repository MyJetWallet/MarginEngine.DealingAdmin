using SimpleTrading.Abstraction.Trading.Profiles;

namespace DealingAdmin.Abstractions.Models
{
    public class TradingProfileInstrumentModel : ITradingProfileInstrument
    {
        public string Id { get; set; }

        public double MinOperationVolume { get; set; }

        public double MaxOperationVolume { get; set; }

        public double MaxPositionVolume { get; set; }

        public int OpenPositionMinDelayMs { get; set; }

        public int OpenPositionMaxDelayMs { get; set; }

        public bool TpSlippage { get; set; }

        public bool SlSlippage { get; set; }
        
        public bool IsTrending { get; set; }

        public bool OpenPositionSlippage { get; set; }

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