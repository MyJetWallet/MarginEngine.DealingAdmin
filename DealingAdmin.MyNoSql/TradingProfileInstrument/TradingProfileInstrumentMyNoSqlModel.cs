using DealingAdmin.Abstractions;

namespace DealingAdmin.MyNoSql.TradingProfileInstrument
{
    public class TradingProfileInstrumentMyNoSqlModel : ITradingProfileInstrument
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

        public static TradingProfileInstrumentMyNoSqlModel Create(ITradingProfileInstrument src)
        {
            return new TradingProfileInstrumentMyNoSqlModel
            {
                Id = src.Id,
                MaxOperationVolume = src.MaxOperationVolume,
                MaxPositionVolume = src.MaxPositionVolume,
                MinOperationVolume = src.MinOperationVolume,
                Leverages = src.Leverages,
                SlSlippage = src.SlSlippage,
                TpSlippage = src.TpSlippage,
                OpenPositionSlippage = src.OpenPositionSlippage,
                OpenPositionMaxDelayMs = src.OpenPositionMaxDelayMs,
                OpenPositionMinDelayMs = src.OpenPositionMinDelayMs,
                IsTrending = src.IsTrending,
                StopOutPercent = src.StopOutPercent
            };
        }
    }
}