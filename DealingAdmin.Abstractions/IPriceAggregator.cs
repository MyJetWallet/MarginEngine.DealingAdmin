using SimpleTrading.Abstraction.BidAsk;
using System.Collections.Generic;

namespace DealingAdmin.Abstractions
{
    public interface IPriceAggregator
    {
        public void UpdateBidAskCache(IEnumerable<IBidAsk> bidAsks);
        
        public void UpdateBidAsk(IBidAsk bidAsk);

        public IBidAsk GetBidAsk(string instrumentId);

        public IEnumerable<IBidAsk> GetBidAskCache();
    }
}