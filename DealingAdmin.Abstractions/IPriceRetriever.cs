using DealingAdmin.Abstractions.Models;
using System.Collections.Generic;

namespace DealingAdmin.Abstractions
{
    public interface IPriceRetriever
    {
        IEnumerable<BidAskModel> GetAllBidAsks();

        BidAskModel GetBidAsk(string instrumentId);

        UnfilteredBidAskModel GetUnfilteredBidAsk(string instrumentId, string providerId);
    }
}