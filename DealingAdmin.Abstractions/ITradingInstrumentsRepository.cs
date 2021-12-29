using SimpleTrading.Abstraction.Trading.Instruments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface ITradingInstrumentsRepository
    {
        Task<IEnumerable<ITradingInstrument>> GetAllAsync();

        Task UpdateAsync(ITradingInstrument item);
    }
}