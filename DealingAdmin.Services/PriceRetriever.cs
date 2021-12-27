using DealingAdmin.Abstractions;
using DealingAdmin.Abstractions.Models;
using SimpleTrading.Abstraction.BidAsk;
using SimpleTrading.ServiceBus.PublisherSubscriber.BidAsk;
using SimpleTrading.ServiceBus.PublisherSubscriber.UnfilteredBidAsk;

namespace DealingAdmin.Services
{
    public class PriceRetriever : IPriceRetriever
    {
        private readonly IBidAskCache _bidAskCache;
        private readonly BidAskMyServiceBusSubscriber _bidAskSubscriber;
        private readonly UnfilteredBidAskMyServiceBusSubscriber _unfilteredBidAskSubscriber;

        private readonly PriceAggregator priceAggregator = new PriceAggregator();

        public PriceRetriever(
            IBidAskCache bidAskCache,
            BidAskMyServiceBusSubscriber bidAskSubscriber,
            UnfilteredBidAskMyServiceBusSubscriber unfilteredBidAskSubscriber)
        {
            _bidAskCache = bidAskCache;
            _bidAskSubscriber = bidAskSubscriber;
            _unfilteredBidAskSubscriber = unfilteredBidAskSubscriber;

            var bidAsks = _bidAskCache.Get();

            priceAggregator.UpdateBidAskCache(bidAsks);

            _bidAskSubscriber.Subscribe(bidAsk =>
            {
                priceAggregator.UpdateBidAsk(bidAsk);
                return new ValueTask();
            });

            _unfilteredBidAskSubscriber.Subscribe(bidAsk =>
            {
                var now = DateTime.UtcNow;
                var timeDelay = now.Subtract(bidAsk.DateTime);
                priceAggregator.UpdateUnfilteredBidAsk(bidAsk);
                return new ValueTask();
            });
        }

        public IEnumerable<BidAskModel> GetAllBidAsks() =>
            priceAggregator.GetBidAskCache().Select(itm => itm.ToBidAskModel());

        public BidAskModel GetBidAsk(string instrumentId) =>
            priceAggregator.GetBidAsk(instrumentId)?.ToBidAskModel();

        public UnfilteredBidAskModel GetUnfilteredBidAsk(string instrumentId, string providerId) =>
            priceAggregator.GetUnfilteredBidAsk(instrumentId, providerId)?.ToUnfilteredBidAskModel();
    }
}