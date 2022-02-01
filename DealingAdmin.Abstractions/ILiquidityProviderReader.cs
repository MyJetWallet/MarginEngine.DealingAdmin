using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface ILiquidityProviderReader
    {
        Task<IEnumerable<string>> GetLiquidityProviders();
    }
}