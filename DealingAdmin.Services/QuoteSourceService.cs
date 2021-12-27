using System.ComponentModel.DataAnnotations;
using DealingAdmin.Abstractions;
using DealingAdmin.Abstractions.Models;
using SimpleTrading.Abstraction.Trading.Settings;
using SimpleTrading.MyNoSqlRepositories.InstrumentSourcesMaps;
using SimpleTrading.QuotesFeedRouter.Abstractions;

namespace DealingAdmin.Services
{
    public class QuoteSourceService : IQuoteSourceService
    {
        private readonly InstrumentSourcesMapsMyNoSqlRepository _instrumentSourcesMapsRepository;

        private readonly IInstrumentsCache _instrumentsCache;

        public QuoteSourceService(
             IInstrumentsCache instrumentsCache,
             InstrumentSourcesMapsMyNoSqlRepository instrumentSourcesMapsRepository)
        {
            _instrumentsCache = instrumentsCache;
            _instrumentSourcesMapsRepository = instrumentSourcesMapsRepository;
        }

        public async Task<IEnumerable<IQuoteFeedSource>> GetInstrumentSourcesMaps()
        {
            return await _instrumentSourcesMapsRepository.GetAllAsync();
        }

        public async Task CreateOrUpdate(QuoteSourceModel quoteSource)
        {
            var instrument = _instrumentsCache.Get(quoteSource.InstrumentId);

            if (instrument == null)
            {
                throw new ValidationException($"Instrument '{quoteSource.InstrumentId}' not found");
            }

            await _instrumentSourcesMapsRepository.UpdateAsync(quoteSource);
        }

        public async Task DeleteById(string instrumentId)
        {
            var instrument = _instrumentsCache.Get(instrumentId);

            if (instrument == null)
            {
                throw new ValidationException($"Instrument '{instrumentId}' not found");
            }

            await _instrumentSourcesMapsRepository.Delete(instrumentId);
        }
    }
}