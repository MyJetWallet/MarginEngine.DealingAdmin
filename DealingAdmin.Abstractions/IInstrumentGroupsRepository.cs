using System.Collections.Generic;
using System.Threading.Tasks;
using DealingAdmin.Abstractions;

namespace DealingAdmin.Abstractions
{
    public interface IInstrumentGroupsRepository
    {
        Task<IEnumerable<IInstrumentGroup>> GetAllAsync();

        Task UpdateAsync(IInstrumentGroup item);
    }
}