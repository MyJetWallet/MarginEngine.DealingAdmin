using System.Collections.Concurrent;
using DealingAdmin.Abstractions;
using SimpleTrading.Abstraction.BidAsk;

namespace DealingAdmin.Services
{
    public class PriceAggregator : IPriceAggregator
    {
        private ConcurrentDictionary<string, IBidAsk> bidAskCache = new ConcurrentDictionary<string, IBidAsk>();

        private ConcurrentDictionary<string, Dictionary<string, IUnfilteredBidAsk>> unfilteredBidAskCache
          = new ConcurrentDictionary<string, Dictionary<string, IUnfilteredBidAsk>>();

        public PriceAggregator()
        {
            bidAskCache = new ConcurrentDictionary<string, IBidAsk>();
        }

        public void UpdateBidAskCache(IEnumerable<IBidAsk> bidAsks)
        {
            foreach (var bidAsk in bidAsks)
            {
                UpdateBidAsk(bidAsk);
            }
        }

        public void UpdateBidAsk(IBidAsk bidAsk)
        {
            try
            {
                bidAskCache[bidAsk.Id] = bidAsk;
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR [PriceAggregator] UpdateBidAsk: {e}");
            }
        }

        public void UpdateUnfilteredBidAsk(IUnfilteredBidAsk bidAsk)
        {
            try
            {
                if (!unfilteredBidAskCache.ContainsKey(bidAsk.Id))
                    unfilteredBidAskCache[bidAsk.Id] = new Dictionary<string, IUnfilteredBidAsk>();

                unfilteredBidAskCache[bidAsk.Id][bidAsk.LiquidityProvider] = bidAsk;
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR UpdateUnfilteredBidAsk: {e}");
            }
        }


        public IBidAsk GetBidAsk(string instrumentId)
        {
            if (bidAskCache.TryGetValue(instrumentId, out IBidAsk bidAsk))
            {
                return bidAsk;
            }
            else
            {
                return null;
            }
        }

        public IUnfilteredBidAsk GetUnfilteredBidAsk(string instrumentId, string liquidityProviderId)
        {
            if (unfilteredBidAskCache.ContainsKey(instrumentId)
                && unfilteredBidAskCache[instrumentId].ContainsKey(liquidityProviderId))
            {
                return unfilteredBidAskCache[instrumentId][liquidityProviderId];
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<IBidAsk> GetBidAskCache() => bidAskCache.Values;

        public IEnumerable<IUnfilteredBidAsk> GetUnfilteredBidAskCache() =>
            from instrumentPrices in unfilteredBidAskCache.Values
            from providerPrice in instrumentPrices.Values
            select providerPrice;

    }
}