using SimpleTrading.Abstraction.BidAsk;
using System;

namespace DealingAdmin.Abstractions.Models
{
    public class BidAskModel
    {
        public string Id { get; set; }

        public string Date { get; set; }

        public double Bid { get; set; }

        public double Ask { get; set; }

        public bool TimeWarning { get; set; }
    }


    public class UnfilteredBidAskModel
    {
        public string Id { get; set; }

        public string Date { get; set; }

        public double Bid { get; set; }

        public double Ask { get; set; }

        public string Provider { get; set; }

        public bool TimeWarning { get; set; }
    }

    public static class PriceModels
    {
        public static UnfilteredBidAskModel ToUnfilteredBidAskModel(this IUnfilteredBidAsk bidAsk)
        {
            return new UnfilteredBidAskModel
            {
                Id = bidAsk.Id,
                Date = bidAsk.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Bid = bidAsk.Bid,
                Ask = bidAsk.Ask,
                Provider = bidAsk.LiquidityProvider,
                TimeWarning = DateTime.Now - bidAsk.DateTime > TimeSpan.FromMinutes(3),
            };
        }

        public static BidAskModel ToBidAskModel(this IBidAsk bidAsk)
        {
            return new BidAskModel
            {
                Id = bidAsk.Id,
                Date = bidAsk.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Bid = bidAsk.Bid,
                Ask = bidAsk.Ask,
                TimeWarning = DateTime.Now - bidAsk.DateTime > TimeSpan.FromMinutes(3),
            };
        }
    }
}