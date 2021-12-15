using System.Collections.Concurrent;
using DealingAdmin.Abstractions;
using SimpleTrading.Abstraction.BidAsk;

namespace DealingAdmin.Services
{
    public class PriceAggregator : IPriceAggregator
    {
        private ConcurrentDictionary<string, IBidAsk> bidAskCache = new ConcurrentDictionary<string, IBidAsk>();

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

        public IEnumerable<IBidAsk> GetBidAskCache() => bidAskCache.Values;
    }
}