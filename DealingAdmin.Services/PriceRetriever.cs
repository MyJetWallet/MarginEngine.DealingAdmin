using DealingAdmin.Abstractions.Models;
using DotNetCoreDecorators;
using SimpleTrading.Abstraction.BidAsk;

namespace DealingAdmin.Services
{
    public class PriceRetriever
    {
        private readonly IBidAskCache _bidAskCache;
        private readonly ISubscriber<IBidAsk> _bidAskSubscriber;

        private readonly PriceAggregator priceAggregator = new PriceAggregator();

        public PriceRetriever(IBidAskCache bidAskCache, ISubscriber<IBidAsk> bidAskSubscriber)
        {
            _bidAskCache = bidAskCache;
            _bidAskSubscriber = bidAskSubscriber;
            Init();
        }

        public void Init()
        {
            var bidAsks = _bidAskCache.Get();
            priceAggregator.UpdateBidAskCache(bidAsks);

            _bidAskSubscriber.Subscribe(bidAsk =>
            {
                priceAggregator.UpdateBidAsk(bidAsk);
                return new ValueTask();
            });
        }

        public IEnumerable<BidAskModel> GetAllBidAsks() =>
            priceAggregator.GetBidAskCache().Select(itm => itm.ToBidAskModel());

        public BidAskModel GetBidAsk(string instrumentId) =>
            priceAggregator.GetBidAsk(instrumentId)?.ToBidAskModel();
    }
}