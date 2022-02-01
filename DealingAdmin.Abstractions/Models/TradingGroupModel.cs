using SimpleTrading.Abstraction.Trading;

namespace DealingAdmin.Abstractions.Models
{
    public class TradingGroupModel : ITradingGroup
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string TradingProfileId { get; set; }
        
        public string MarkupProfileId { get; set; }

        public string SwapProfileId { get; set; }

        public bool TradingDisabled { get; set; }
        
        public string TokenKey { get; set; }

        public static TradingGroupModel Create(ITradingGroup src)
        {
            return new TradingGroupModel
            {
                Id = src.Id,
                Name = src.Name,
                TradingProfileId = src.TradingProfileId,
                MarkupProfileId = src.MarkupProfileId,
                SwapProfileId = src.SwapProfileId,
                TradingDisabled = src.TradingDisabled
            };
        }
    }
}