namespace DealingAdmin.Abstractions.Models
{
    public class TraderBrandSearchModel
    {
        public string TraderId { get; set; }

        public string Brand { get; set; }

        public bool SearchMatch { get; set; }

        public static TraderBrandSearchModel FromTraderBrand(TraderBrandModel src, bool searchMatch)
        {
            return new TraderBrandSearchModel
            {
                TraderId = src.TraderId,
                Brand = src.Brand,
                SearchMatch = searchMatch
            };
        }
        
    }
}