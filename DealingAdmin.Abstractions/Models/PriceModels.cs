using SimpleTrading.Abstraction.BidAsk;

namespace DealingAdmin.Abstractions.Models
{
    public class BidAskModel
    {
        public string Id { get; set; }

        public string Date { get; set; }

        public double Bid { get; set; }

        public double Ask { get; set; }
    }


    public class UnfilteredBidAskModel
    {
        public string Id { get; set; }

        public string Date { get; set; }

        public double Bid { get; set; }

        public double Ask { get; set; }

        public string Provider { get; set; }
    }

    public static class PriceModels
    {
        public static UnfilteredBidAskModel ToUnfilteredBidAskModel(this IUnfilteredBidAsk bidAsk)
        {
            return new UnfilteredBidAskModel
            {
                Id = bidAsk.Id,
                Date = bidAsk.DateTime.ToString("s"),
                Bid = bidAsk.Bid,
                Ask = bidAsk.Ask,
                Provider = bidAsk.LiquidityProvider
            };
        }

        public static BidAskModel ToBidAskModel(this IBidAsk bidAsk)
        {
            return new BidAskModel
            {
                Id = bidAsk.Id,
                Date = bidAsk.DateTime.ToString("s"),
                Bid = bidAsk.Bid,
                Ask = bidAsk.Ask
            };
        }
    }
}