using DealingAdmin.Abstractions.Models;
using SimpleTrading.QuotesFeedRouter.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface IQuoteSourceService
    {
        Task CreateOrUpdate(QuoteSourceModel quoteSource);
        
        Task DeleteById(string instrumentId);
        
        Task<IEnumerable<IQuoteFeedSource>> GetInstrumentSourcesMaps();
    }
}