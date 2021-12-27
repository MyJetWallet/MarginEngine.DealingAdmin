using SimpleTrading.QuotesFeedRouter.Abstractions;

namespace DealingAdmin.Abstractions.Models
{
    public class QuoteSourceModel : IQuoteFeedSource
    {
        public string SourceId { get; set; }

        public string InstrumentId { get; set; }
    }
}