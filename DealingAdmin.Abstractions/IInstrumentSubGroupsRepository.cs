using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface IInstrumentSubGroupsRepository
    {
        Task<IEnumerable<IInstrumentSubGroup>> GetAllAsync();

        Task<IEnumerable<IInstrumentSubGroup>> GetByGroupIdAsync(
            string groupId);

        Task UpdateAsync(IInstrumentSubGroup item);

        Task DeleteAsync(string groupId, string subGroupId);
    }
}