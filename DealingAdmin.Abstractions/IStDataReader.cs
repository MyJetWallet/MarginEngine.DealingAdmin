using SimpleTrading.Abstraction.Trading.Positions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface IStDataReader
    {
        Task<IEnumerable<ITradeOrder>> GetActivePositionsAsync();
        Task<IEnumerable<ITradeOrder>> GetActivePositionsAsync(string traderId, string accountId);
        Task<IEnumerable<ITradeOrder>> GetClosedPositionsAsync(DateTime dateFrom, DateTime dateTo);
        Task<IEnumerable<ITradeOrder>> GetClosedPositionsAsync(string traderId, string accountId, DateTime dateFrom, DateTime dateTo);
        Task<IEnumerable<ITradeOrder>> GetPendingOrdersAsync();
        Task<IEnumerable<ITradeOrder>> GetPendingOrdersAsync(string traderId, string accountId);
    }
}