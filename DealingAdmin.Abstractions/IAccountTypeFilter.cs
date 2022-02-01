using DealingAdmin.Abstractions.Models;
using SimpleTrading.Abstraction.Trading.Positions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface IAccountTypeFilter
    {
        Task<IEnumerable<ITradeOrder>> FilterPositions(IEnumerable<ITradeOrder> positions, AccountType accType);
    }
}