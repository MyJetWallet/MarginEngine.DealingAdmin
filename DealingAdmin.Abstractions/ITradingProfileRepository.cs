using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface ITradingProfileRepository
    {
        Task<IEnumerable<ITradingProfile>> GetAllAsync();

        Task UpdateAsync(ITradingProfile tradingGroup);
    }
}